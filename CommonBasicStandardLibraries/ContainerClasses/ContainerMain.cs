using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Collections.Generic;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.ContainerClasses
{
    public enum EnumResolveCategory
    {
        ShowError = 0,
        ManuelPriority = 1, //if priority is used, then you have to specify or still won't work. needs to be for all.
        FIFO = 2,
        LIFO = 3,
        Factory = 4, //this means that whoever has a factory will get it first.  if more than one factory, then raise error so i have to rethink
    }

    public class ContainerMain: IResolver //this is the main class for the container
    {
        private readonly HashSet<ContainerData> ThisSet = new HashSet<ContainerData>();
        public static EnumResolveCategory ResolveCategory = EnumResolveCategory.ShowError; //default to show error so you have to rethink.

        private int ID;
        private static readonly HashSet<IContainerFactory> FactoryList = new HashSet<IContainerFactory>(); //i think i want anybody to be able to add to this list.  makes the container more powerful.
        public IContainerFactory ParentFactory; //i think its implied that if you set the parent, the parent will handle all duplicates.

        public void ClearContainer() //there are cases i still have to clear container (if doing multiple games and some has to re registered
        {
            ThisSet.Clear();
            FactoryList.Clear();
        }

        public static void AddFactoryToContainer(IContainerFactory ThisFact)
        {
            FactoryList.Add(ThisFact);
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        private CustomBasicList<ContainerData> PossibleList(ContainerData ThisResult, Type ThisType)
        {
            var TempList = ThisSet.Where(Items => Items.TypeIn == ThisResult.TypeIn || Items.TypeOut == ThisResult.TypeOut).ToCustomBasicList();
            TempList.ForEach(Items => Items.RequestedType = ThisType);
            return TempList;
        }

        private object GetInstance(ContainerData ThisResult, Type ThisType)
        {
            if (ThisResult.ThisFact != null && ThisResult.SimpleFunction != null)
                throw new BasicBlankException("You can't both use a function and a factory");
            if (ThisResult.ThisObject != null && ThisResult.IsSingle == true)
                return ThisResult.ThisObject;

            if (ThisResult.ThisFact != null)
            {
                if (ThisResult.IsSingle == true)
                {
                    ThisResult.ThisObject = ThisResult.ThisFact.GetReturnObject(PossibleList(ThisResult, ThisType), ThisResult);
                    return ThisResult.ThisObject;
                }
                else
                    return ThisResult.ThisFact.GetReturnObject(PossibleList(ThisResult, ThisType), ThisResult); //since objects are going to be used, then its more flexible
                //the downside is it takes a while to get used to it.  not sure how i can use generics without causing other problems though.
            }
            if (ThisResult.SimpleFunction != null)
            {
                if (ThisResult.IsSingle == true)
                {
                    ThisResult.ThisObject = ThisResult.SimpleFunction();
                    return ThisResult.ThisObject;
                }
                return ThisResult.SimpleFunction();
            }
            throw new BasicBlankException("I think I need to know what function to run to get the object");
        }

        private object GetInstance(CustomBasicList<ContainerData> ThisList, Type ThisType) //this is because you already have more than one that matches.
        {
            if (ThisList.Count < 2)
                throw new BasicBlankException("It should have been obvious which one it was");
            if (ResolveCategory == EnumResolveCategory.ShowError)
                throw new BasicBlankException($"There was {ThisList.Count} Items That Had A Match.  Either Change The Revolve Category Or Rethink");
            ThisList.Sort();
            ContainerData FirstItem;
            ContainerData SecondItem;
            FirstItem = ThisList[0];
            SecondItem = ThisList[1];
            if (FirstItem.IntendedTypeOnly == false)
            {
                switch (ResolveCategory)
                {

                    case EnumResolveCategory.ManuelPriority:
                        if (FirstItem.Priority == SecondItem.Priority)
                            throw new BasicBlankException("Priorities can't have duplicates.");
                        break;
                    case EnumResolveCategory.FIFO:

                    case EnumResolveCategory.LIFO:
                        if (FirstItem.ID == SecondItem.ID)
                            throw new BasicBlankException("Sorting By ID Failed");
                        break;
                    case EnumResolveCategory.Factory:
                        if (FirstItem.WasCustomFunction == true & SecondItem.WasCustomFunction == true)
                            throw new BasicBlankException("I think there should not have been 2 custom functions.  Maybe sorting by custom function failed.");
                        if (FirstItem.WasCustomFunction == false && ThisList.Exists(Items => Items.WasCustomFunction == true))
                            throw new BasicBlankException("For sure, sorting by custom function failed because the first item obtained was not a custom function");
                        if (FirstItem.WasCustomFunction == false)
                            throw new BasicBlankException("There was no factories or custom funtions.  Therefore it was not appropriate to use factories category in this case");
                        break;
                    default:
                        throw new BasicBlankException("GetInstance Rethink");
                }
            }
            else if (FirstItem.IntendedTypeOnly == true && SecondItem.IntendedTypeOnly == true)
                throw new BasicBlankException("Most likely error did not get caught because can't have 2 classes that are types only that are the same type.  Otherwise, too complex");
            return GetInstance(FirstItem, ThisType);
        }

        private object GetInstance(Type ThisType) //i want to be forced to use generics.
        {
            if (ParentFactory != null)
                return ParentFactory.GetReturnObject(ThisSet.ToCustomBasicList(), ThisType); //this means if i have a parent factory, then the parent is responsible for everything.
            //i could change my mind if i choose to.  i'll have to decide later if that was the right decision or not (?)
            CustomBasicList<ContainerData> TempList;
            TempList = ThisSet.Where(Items => Items.TypeIn == ThisType).ToCustomBasicList();
            bool HadAtLeastOne = false;
            TempList.RemoveAllOnly(Items => Items.ThisFact != null && Items.ThisFact.CanAcceptObject(TempList, ThisType) == false);
            if (TempList.Count == 1)
                return GetInstance(TempList.Single(), ThisType);
            if (TempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                HadAtLeastOne = true;
                TempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (TempList.Count > 1)
                return GetInstance(TempList, ThisType);
            TempList = ThisSet.Where(Items => Items.TypeOut == ThisType).ToCustomBasicList();
            TempList.RemoveAllOnly(Items => Items.ThisFact != null && Items.ThisFact.CanAcceptObject(TempList, ThisType) == false);
            if (TempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                HadAtLeastOne = true;
                TempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (TempList.Count > 1)
                return GetInstance(TempList, ThisType);
            if (TempList.Count == 1)
                return GetInstance(TempList.Single(), ThisType);
            if (TempList.Count == 0 && FactoryList.Count == 0)
                throw new BasicBlankException($"Looks Like Type {ThisType.Name} Was Not Registered And Had No Factories.  If It Was, Rethink");
            TempList = ThisSet.ToCustomBasicList();
            CustomBasicList<IContainerFactory> ResultList = FactoryList.Where(Items => Items.CanAcceptObject(TempList, ThisType) == true).ToCustomBasicList();
            if (ResultList.Count > 1)
                throw new BasicBlankException($"There is more than one result found for requested type {ThisType.Name}.  I think that rethinking is required now");
            if (ResultList.Count == 0 && HadAtLeastOne == false)
                throw new BasicBlankException($"There was nothing registered with the name {ThisType.Name}.  No factories was able to handle this request");
            else if (ResultList.Count == 0)
                throw new BasicBlankException($"Since you set the priority to manuel priority, you are required to manually set the priority to resolve duplicates.  Type Was {ThisType.Name}.  Most likely all the ones with the duplicates has to be set manually, choose another option or rethink");
            return ResultList.Single().GetReturnObject(TempList, ThisType);
        }

        private object PrivateInstance(Type ThisType)
        {
            var constructor = ThisType.GetConstructors().OrderByDescending(Items => Items.GetParameters().Length).FirstOrDefault(); //in the video, its first or default.

            var args = constructor.GetParameters().Select(Items => GetInstance(Items.ParameterType)).ToArray(); //for his test.  i could decide to use my custom collection instead.
            return Activator.CreateInstance(ThisType, args);
        }

        public void RegisterInstance<TIn, TOut>(int TPriority = 0, object ThisObject = null) where TOut : TIn
        {
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = false,
                TypeOut = typeof(TOut), //i think
                TypeIn = typeof(TIn),
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }
        public void RegisterType<TIn>(bool IsSingleton) //i think if you want to register a type, you are not allowed to use a factory.
        {
            //in this case, needs to be kept simple.  otherwise, gets too hard.
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = IsSingleton,
                TypeOut = typeof(TIn),
                TypeIn = typeof(TIn),
                IntendedTypeOnly = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TIn)))
            };
            SetResults(ThisResults, 0, null);
        }
        public void RegisterInstance<TIn>(Func<TIn> ThisFunct, int TPriority = 0, object ThisObject = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object ThisObj = ThisFunct;
            Func<object> NewFunct = (Func<object>)ThisObj;
            ContainerData ThisResults = new ContainerData()
            {
                TypeIn = typeof(TIn), //this means this is all the information you will receive this time.
                TypeOut = typeof(TIn),
                WasCustomFunction = true, //i think this is the only case where it was custom except for factories.
                SimpleFunction = NewFunct
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }

        public void RegisterInstance<TIn>(IContainerFactory ThisFact, int TPriority = 0, object ThisObject = null)
        {
            ContainerData ThisResults = new ContainerData()
            {
                TypeIn = typeof(TIn),
                TypeOut = typeof(TIn),
                WasCustomFunction = true, //i think this should imply its custom obviously.
                ThisFact = ThisFact
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }

        public void RegisterSingleton<TIn>(TIn OurObject, int TPriority = 0, object ThisObject = null)
        {
            if (OurObject == null)
                throw new BasicBlankException("You can't register an object that does not exist.  Most likely, you tried to register it too soon.");
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = OurObject.GetType(),
                ThisObject = OurObject
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }

        private void SetResults(ContainerData ThisResults, int TPriority, object ThisObject)
        {
            ThisResults.Priority = TPriority; //2 different things.
            ThisResults.ExtraDetails = ThisObject;
            ID++;
            ThisResults.ID = ID; //needed for lifo and fifo methods.
            ThisResults.Check(); //so if something is wrong, will error out at this moment.
            ThisSet.Add(ThisResults);
        }

        public void RegisterSingleton<TIn>(Func<TIn> ThisFunct, int TPriority = 0, object ThisObject = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object ThisObj = ThisFunct;
            Func<object> NewFunct = (Func<object>)ThisObj;
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = typeof(TIn), //this means this is all the information you will receive this time.
                SimpleFunction = NewFunct
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }

        public void RegisterSingleton<TIn>(IContainerFactory ThisFact, int TPriority = 0, object ThisObject = null)
        {
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = typeof(TIn),
                ThisFact = ThisFact
            };
            AddFactoryToContainer(ThisFact); //i think i should automatically add a factory for a possibility if i do one.  if i am wrong, can rethink.
            SetResults(ThisResults, TPriority, ThisObject);
        }

        public void RegisterSingleton<TIn, TOut>(int TPriority = 0, object ThisObject = null) where TOut : TIn //if you specified priority on any, then all will be priority.
        {
            ContainerData ThisResults = new ContainerData()
            {
                TypeOut = typeof(TOut),
                TypeIn = typeof(TIn),
                IsSingle = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(ThisResults, TPriority, ThisObject);
        }

        T IResolver.Resolve<T>()
        {
            return GetInstance<T>();
        }
    }
}