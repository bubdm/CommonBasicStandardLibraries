using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.Exceptions;
using System.Collections.Generic;
using System.Linq;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator.RandomSetHelpers;
namespace CommonBasicStandardLibraries.CollectionClasses
{
    public class WeightedAverageLists<T>
    {
        private readonly Dictionary<T, int> _possibleList = new Dictionary<T, int>();
        private readonly Dictionary<int, int> _subList = new Dictionary<int, int>();
        private RandomGenerator? _rs;

        private IResolver? _privateContainer;
        public IResolver MainContainer { set => _privateContainer = value; }
        public WeightedAverageLists<T> AddSubItem(int numberPossible, int weight)
        {
            _subList.Add(numberPossible, weight);
            return this;
        }
        public CustomBasicList<int> GetSubList(bool needsToClear = true)
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            if (_subList.Count == 0)
                throw new BasicBlankException("You never used the sublist");
            CustomBasicList<int> output = new CustomBasicList<int>();
            foreach (var item in _subList.Keys)
            {
                int HowMany = _subList[item];
                HowMany.Times(Items => output.Add(item));
            }
            if (needsToClear == true)
                _subList.Clear(); //so i can use next time.
            output.MainContainer = _privateContainer;
            return output;
        }
        public void FillExtraSubItems(int lowRange, int highRange)
        {
            for (int i = lowRange; i <= highRange; i++)
            {
                if (_subList.ContainsKey(i) == false)
                    _subList.Add(i, 1);
            }
        }
        public int GetSubCount => GetSubList(false).Count;
        public WeightedAverageLists<T> AddWeightedItem(T thisItem)
        {
            var output = GetSubList();
            return AddWeightedItem(thisItem, output);
        }
        public WeightedAverageLists<T> AddWeightedItem(T thisItem, int weight)
        {
            if (weight == 0)
                return this; //can't be chosen because the weight is 0
            _possibleList.Add(thisItem, weight); //so if you can't runtime error because its already done.
            return this; //so i have fluency.
        }
        public WeightedAverageLists<T> AddWeightedItem(T thisItem, int lowRange, int highRange)
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            int Chosen = _rs.GetRandomNumber(highRange, lowRange);
            return AddWeightedItem(thisItem, Chosen);
        }
        public WeightedAverageLists<T> AddWeightedItem(T thisItem, CustomBasicList<int> possList)
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            possList.MainContainer = _privateContainer; //just in case.
            int chosen = possList.GetRandomItem();
            return AddWeightedItem(thisItem, chosen);
        }
        public CustomBasicList<T> GetWeightedList()
        {
            SetRandom(ref _privateContainer!, ref _rs!);
            CustomBasicList<T> output = new CustomBasicList<T>();
            foreach (var thisItem in _possibleList.Keys)
            {
                int howMany = _possibleList[thisItem];
                howMany.Times(Items =>
                {
                    output.Add(thisItem);
                });
            }
            output.MainContainer = _privateContainer;
            return output;
        }
        public int GetExpectedCount => _possibleList.Sum(items => items.Value);
    }
}