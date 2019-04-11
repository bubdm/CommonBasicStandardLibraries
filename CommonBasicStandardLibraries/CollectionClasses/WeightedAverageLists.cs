using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using System.Collections.Generic;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomSetHelpers;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class WeightedAverageLists<T>
    {
        private readonly Dictionary<T, int> PossibleList = new Dictionary<T, int>();


        private readonly Dictionary<int, int> SubList = new Dictionary<int, int>();

        //this will resolve the random functions.

        private RandomGenerator rs;

        private IResolver PrivateContainer;
        public IResolver MainContainer { set => PrivateContainer = value; }

        //public WeightedAverageLists()
        //{
        //    rs = Resolve<RandomGenerator>();
        //}
        public  WeightedAverageLists<T> AddSubItem(int NumberPossible, int Weight)
        {
            SubList.Add(NumberPossible, Weight);
            return this;
        }

        public CustomBasicList<int> GetSubList(bool NeedsToClear = true)
        {
            SetRandom(ref PrivateContainer, ref rs);
            if (SubList.Count == 0)
                throw new BasicBlankException("You never used the sublist");
            CustomBasicList<int> output = new CustomBasicList<int>();
            foreach (var item in SubList.Keys)
            {
                int HowMany = SubList[item];
                HowMany.Times(Items => output.Add(item));
            }
            if (NeedsToClear == true)
                SubList.Clear(); //so i can use next time.
            output.MainContainer = PrivateContainer;
            return output;
        }

        public void FillExtraSubItems(int LowRange, int HighRange)
        {
            for (int i = LowRange; i <= HighRange; i++)
            {
                if (SubList.ContainsKey(i) == false)
                    SubList.Add(i, 1);
            }
        }

        public int GetSubCount => GetSubList(false).Count;
        
        
        public WeightedAverageLists<T> AddWeightedItem(T ThisItem)
        {
            var output = GetSubList();
            return AddWeightedItem(ThisItem, output);
        }

        public WeightedAverageLists<T> AddWeightedItem(T ThisItem, int Weight)
        {
            if (Weight == 0)
                return this; //can't be chosen because the weight is 0
            PossibleList.Add(ThisItem, Weight); //so if you can't runtime error because its already done.
            return this; //so i have fluency.
        }

        //private void SetRandom()
        //{
        //    if (rs != null)
        //        return;
        //    if (PrivateContainer == null)
        //        PrivateContainer = cons;
        //    if (PrivateContainer != null)
        //        rs = PrivateContainer.Resolve<RandomGenerator>();
        //    else
        //        throw new BasicBlankException("No resolver for random functions.  Possibly rethink");
        //        //rs = new RandomGenerator(); //if no resolver anywhere, then has to manually create new random function.
        //}

        public WeightedAverageLists<T> AddWeightedItem(T ThisItem, int LowRange, int HighRange)
        {
            SetRandom(ref PrivateContainer, ref rs);
            int Chosen = rs.GetRandomNumber(HighRange, LowRange);
            return AddWeightedItem(ThisItem, Chosen);
        }
        public WeightedAverageLists<T> AddWeightedItem(T ThisItem, CustomBasicList<int> PossList)
        {
            SetRandom(ref PrivateContainer, ref rs);
            PossList.MainContainer = PrivateContainer; //just in case.
            int Chosen = PossList.GetRandomItem();
            return AddWeightedItem(ThisItem, Chosen);
        }

        public CustomBasicList<T> GetWeightedList()
        {
            SetRandom(ref PrivateContainer, ref rs);
            CustomBasicList<T> output = new CustomBasicList<T>();
            foreach (var ThisItem in PossibleList.Keys)
            {
                int HowMany = PossibleList[ThisItem];
                HowMany.Times(Items =>
                {
                    output.Add(ThisItem);
                });
            }
            output.MainContainer = PrivateContainer;
            return output;
        }

        public int GetExpectedCount => PossibleList.Sum(Items => Items.Value);
        

    }
}
