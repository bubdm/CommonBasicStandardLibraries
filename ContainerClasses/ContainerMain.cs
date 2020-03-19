using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
namespace CommonBasicStandardLibraries.ContainerClasses
{
    public enum EnumResolveCategory
    {
        ShowError = 0,
        ManuelPriority = 1, //if priority is used, then you have to specify or still won't work. needs to be for all.
        FIFO = 2,
        LIFO = 3,
        //Factory = 4, //this means that whoever has a factory will get it first.  if more than one factory, then raise error so i have to rethink
    }



    public class ContainerMain : IResolver //this is the main class for the container
    {
        private readonly HashSet<ContainerData> _thisSet = new HashSet<ContainerData>();
        public static EnumResolveCategory ResolveCategory = EnumResolveCategory.ShowError; //default to show error so you have to rethink.

        private int _id;
        private readonly RandomGenerator _rans; //this is most common.
        private HttpClient? _client;
        private void NeedsClient() //i like doing it automatically after all now.
        {
            _needsHttpClient = true;
            if (_client == null)
                _client = new HttpClient();
        }

        private bool _needsHttpClient;//if set to true, then if it requests httpclient, will be a brand new one if not already there
        //decided to not even have the ability to clear container since game package had its own system.

        public ContainerMain()
        {
            _rans = new RandomGenerator();
            //RegisterSingleton(Rans);
        }


        //just in case the web or something that uses servicecollection needs random, might as well give it as well.

        public CustomBasicList<ContainerResults> GetContainerList() //so it can be added to the off the shelf dependency injection system.
        {
            CustomBasicList<ContainerResults> output = new CustomBasicList<ContainerResults>()
            {
                new ContainerResults()
                {
                    PayLoad = _rans,
                    IsSingle = true
                }
            };

            CustomBasicList<ContainerResults> realItems = _thisSet.Select(items =>
            {
                ContainerResults results = new ContainerResults();
                results.PayLoad = items.ThisObject;
                results.IsSingle = items.IsSingle;
                results.TypeFrom = items.TypeIn;
                results.TypeTo = items.TypeOut;
                return results;
            }).ToCustomBasicList();
            output.AddRange(realItems);
            return output;
        }


        public T GetInstance<T>()
        {
            if (typeof(T) == typeof(RandomGenerator)) //this is an exception.
            {
                //i originally had static random generators.  can't do anymore because of testing problems.  so this will do it instead.
                object thisObj = _rans;
                return (T)thisObj;
            }
            if (typeof(T) == typeof(HttpClient))
            {
                if (_needsHttpClient == false)
                {
                    NeedsClient(); //could be done automatically.  hopefully that works.
                }
                object output = _client!;
                return (T)output;
            }
            return (T)GetInstance(typeof(T));
        }
        public T GetInstance<T>(object tag)
        {
            return (T)SimpleInstance(typeof(T), tag);
        }
        private object SimpleInstance(Type thisType, object thisObject)
        {
            //this will only do simple ones period.
            CustomBasicList<ContainerData> tempList = _thisSet.Where(Items => Items.TypeIn == thisType && Items.ExtraDetails == thisObject).ToCustomBasicList();
            if (tempList.Count == 0)
                throw new BasicBlankException($"{thisType.Name} With Tag Not Found");
            //if you send in tag, it must match the tag that was registered with.
            if (tempList.Count > 1)
                throw new BasicBlankException($"Had Duplicates For Instance With Tag.  Name Was {thisType.Name}.  Rethink");
            return GetInstance(tempList.Single());
        }

        private object GetInstance(ContainerData thisResult)
        {
            if (thisResult.ThisObject != null && thisResult.IsSingle == true)
                return thisResult.ThisObject;

            if (thisResult.SimpleFunction != null)
            {
                if (thisResult.IsSingle == true)
                {
                    thisResult.ThisObject = thisResult.SimpleFunction();
                    return thisResult.ThisObject;
                }
                return thisResult.SimpleFunction();
            }
            throw new BasicBlankException("I think I need to know what function to run to get the object");
        }
        private object GetInstance(CustomBasicList<ContainerData> thisList, Type thisType) //this is because you already have more than one that matches.
        {
            if (thisList.Count < 2)
                throw new BasicBlankException("It should have been obvious which one it was");
            if (ResolveCategory == EnumResolveCategory.ShowError)
                throw new BasicBlankException($"There was {thisList.Count} Items That Had A Match.  Was Trying To Resolve {thisType.Name} Either Change The Revolve Category Or Rethink");
            thisList.Sort();
            ContainerData firstItem;
            ContainerData secondItem;
            firstItem = thisList[0];
            secondItem = thisList[1];
            if (firstItem.IntendedTypeOnly == false)
            {
                switch (ResolveCategory)
                {

                    case EnumResolveCategory.ManuelPriority:
                        if (firstItem.Priority == secondItem.Priority)
                            throw new BasicBlankException("Priorities can't have duplicates.");
                        break;
                    case EnumResolveCategory.FIFO:

                    case EnumResolveCategory.LIFO:
                        if (firstItem.ID == secondItem.ID)
                            throw new BasicBlankException("Sorting By ID Failed");
                        break;
                    default:
                        throw new BasicBlankException("GetInstance Rethink");
                }
            }
            else if (firstItem.IntendedTypeOnly == true && secondItem.IntendedTypeOnly == true)
                throw new BasicBlankException("Most likely error did not get caught because can't have 2 classes that are types only that are the same type.  Otherwise, too complex");
            return GetInstance(firstItem);
        }
        //needs to be public after all.
        public object GetInstance(Type thisType) //i want to be forced to use generics.
        {

            if (thisType == typeof(RandomGenerator))
                return _rans; //i think this is best.
            if (thisType == typeof(HttpClient))
            {
                if (_needsHttpClient == false)
                {
                    NeedsClient(); //could be done automatically.  hopefully that works.
                }
                return _client!;
            }
            CustomBasicList<ContainerData> tempList;
            tempList = _thisSet.Where(items => items.TypeIn == thisType).ToCustomBasicList();
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            if (tempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                tempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (tempList.Count > 1)
                return GetInstance(tempList, thisType);
            tempList = _thisSet.Where(Items => Items.TypeOut == thisType).ToCustomBasicList();
            if (tempList.Count > 1 && ResolveCategory == EnumResolveCategory.ManuelPriority)
            {
                tempList.RemoveAllOnly(Items => Items.Priority == 0);
            }
            if (tempList.Count > 1)
                return GetInstance(tempList, thisType);
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            tempList = _thisSet.Where(Items => thisType.IsAssignableFrom(Items.TypeOut)).ToCustomBasicList();
            if (tempList.Count == 1)
                return GetInstance(tempList.Single());
            throw new BasicBlankException($"Looks Like Type {thisType.Name} Was Not Registered.  If I am wrong, rethink");
        }
        private object PrivateInstance(Type thisType)
        {
            var constructor = thisType.GetConstructors().OrderByDescending(Items => Items.GetParameters().Length).FirstOrDefault(); //in the video, its first or default.
            var args = constructor.GetParameters().Select(Items => GetInstance(Items.ParameterType)).ToArray(); //for his test.  i could decide to use my custom collection instead.
            return Activator.CreateInstance(thisType, args);
        }
        public void RegisterInstance<TIn, TOut>(int tPriority = 0, object? tag = null) where TOut : TIn
        {
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = false,
                TypeOut = typeof(TOut), //i think
                TypeIn = typeof(TIn),
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(thisResults, tPriority, tag!);
        }
        public void RegisterType<TIn>(bool isSingleton) //i think if you want to register a type, you are not allowed to use a factory.
        {
            //in this case, needs to be kept simple.  otherwise, gets too hard.
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = isSingleton,
                TypeOut = typeof(TIn),
                TypeIn = typeof(TIn),
                IntendedTypeOnly = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TIn)))
            };
            SetResults(thisResults, 0, null!);
        }
        public void RegisterInstance<TIn>(Func<TIn> thisFunct, int TPriority = 0, object? tag = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object thisObj = thisFunct;
            Func<object> newFunct = (Func<object>)thisObj;
            ContainerData ThisResults = new ContainerData()
            {
                TypeIn = typeof(TIn), //this means this is all the information you will receive this time.
                TypeOut = typeof(TIn),
                WasCustomFunction = true, //i think this is the only case where it was custom except for factories.
                SimpleFunction = newFunct
            };
            SetResults(ThisResults, TPriority, tag);
        }
        public void RegisterStaticVariable<V>(V variable, string tag) where V : IConvertible
        {
            ContainerData ThisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(V),
                TypeOut = typeof(V),
                ThisObject = variable
            };
            SetResults(ThisResults, 0, tag); //i think
        }
        public void RegisterSingleton<TIn>(TIn ourObject, int tPriority = 0, object? tag = null)
        {
            if (ourObject == null)
                throw new BasicBlankException("You can't register an object that does not exist.  Most likely, you tried to register it too soon.");
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = ourObject.GetType(),
                ThisObject = ourObject
            };
            SetResults(thisResults, tPriority, tag);
        }
        private void SetResults(ContainerData thisResults, int tPriority, object? tag)
        {
            thisResults.Priority = tPriority; //2 different things.
            thisResults.ExtraDetails = tag;
            _id++;
            thisResults.ID = _id; //needed for lifo and fifo methods.
            //ThisResults.Check(); //so if something is wrong, will error out at this moment.
            _thisSet.Add(thisResults);
        }

        public void RegisterSingleton<TIn>(Func<TIn> funct, int priority = 0, object? tag = null) //i do like it this way because you have complete control of what you will actually get back.
        {
            object payLoad = funct;
            Func<object> newFunc = (Func<object>)payLoad;
            ContainerData results = new ContainerData()
            {
                IsSingle = true,
                TypeIn = typeof(TIn),
                TypeOut = typeof(TIn), //this means this is all the information you will receive this time.
                SimpleFunction = newFunc
            };
            SetResults(results, priority, tag);
        }

        public void RegisterSingleton<TIn, TOut>(int tPriority = 0, object? tag = null) where TOut : TIn //if you specified priority on any, then all will be priority.
        {
            ContainerData thisResults = new ContainerData()
            {
                TypeOut = typeof(TOut),
                TypeIn = typeof(TIn),
                IsSingle = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeof(TOut)))
            };
            SetResults(thisResults, tPriority, tag);
        }
        T IResolver.Resolve<T>(object tag)
        {
            return GetInstance<T>(tag);
        }
        T IResolver.Resolve<T>()
        {
            return GetInstance<T>();
        }
        public void ReplaceObject<T>(T newObject)
        {
            Type thisType = typeof(T);
            try
            {
                ContainerData thisData = _thisSet.Where(Items => Items.TypeOut == thisType && Items.IsSingle == true).Single();
                thisData.ThisObject = newObject;
            }
            catch (Exception ex)
            {
                throw new BasicBlankException($"Unable to replace object.  The type you were trying to replace is {thisType.Name}.  Error was {ex.Message}");
            }
        }
        public void RegisterInstance<Tin, TOut>(object tag)
        {
            Type typeOut = typeof(TOut);
            Type typeIn = typeof(Tin);
            ContainerData thisResults = new ContainerData()
            {
                IsSingle = false,
                TypeOut = typeOut,
                TypeIn = typeIn,
                IntendedTypeOnly = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(typeOut))
            };
            SetResults(thisResults, 0, tag);
        }
        public void RegisterInstanceType(Type type)
        {

            if (_thisSet.Any(x => x.TypeIn == type && x.TypeOut == type))
            {
                return; //i think it should just ignore in that case.
            }

            ContainerData thisResults = new ContainerData()
            {
                IsSingle = false,
                TypeOut = type,
                TypeIn = type,
                IntendedTypeOnly = true,
                SimpleFunction = new Func<object>(() => PrivateInstance(type))
            };
            SetResults(thisResults, 0, null!);
        }

        bool IResolver.RegistrationExist<T>()
        {
            Type thisType = typeof(T);
            return _thisSet.Any(x => thisType.IsAssignableFrom(x.TypeOut));
        }
    }
}