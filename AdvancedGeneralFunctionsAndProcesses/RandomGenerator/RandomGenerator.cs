using System;
using System.Collections.Generic;
using System.Linq;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    /// <summary>
    /// Was static but can't be static anymore since it could be used in unit testing.
    /// and could have to have specific values sent to it.
    /// </summary>
    public partial class RandomGenerator
    {
        public enum EnumFormula
        {
            NetStandard, TwisterStandard, TwisterHigh, Twister1, Twister2, Twister3
        }

        //if i decide to include others, will do as well.


        private bool _dids = false;
        //private static Random r;

        private Func<double>? _r;

        //if i need the times to shuffle, can do it.
        private int _privateID;
        public EnumFormula Formula = EnumFormula.TwisterStandard;

        //looks like i still get to do the seed part.
        //that is necessary because i found out from tests that if i don't, then theirs can still be same numbers.
        //i tried with the new guid and that worked.

        private readonly object _thisObj = new object(); //so it has to lock when initializing.

        internal int Next(int max) //since i did quite a bit
        {
            //return (int) Math.Floor(_random()*(max - min + 1) + min);
            return (int) Math.Floor(_r!() * (max));
        }

        internal int Next(int min, int max) //since i did quite a bit
        {
            //return (int) Math.Floor(_random()*(max - min + 1) + min);
            return (int)Math.Floor(_r!() * (max - min) + min); //should have been minus and not plus.
        }

        public int GetSeed()
        {
            return _privateID;
        }

        private void ShowError()
        {
            throw new BasicBlankException("Random number could not be generated, range to narrow");
        }        

        public string GenerateRandomPassword()
        {
            RandomPasswordParameterClass thisPassword = new RandomPasswordParameterClass();
            return GenerateRandomPassword(thisPassword);
        }

        public string GenerateRandomPassword(RandomPasswordParameterClass thisPassword)
        {
            DoRandomize();
            CustomBasicList<int> tempResults = new CustomBasicList<int>();
            int x;
            int picked;
            if (thisPassword.HowManyNumbers > 0)
            {
                var NumberList = Enumerable.Range(48, 10).ToList();
                if (thisPassword.EliminateSimiliarCharacters == true)
                {
                    NumberList.Remove(48); // because that is a 0
                    NumberList.Remove(49); // because 1 is close to l or I
                }

                var loopTo = thisPassword.HowManyNumbers;
                for (x = 1; x <= loopTo; x++)
                {
                    picked = Next(NumberList.Count);
                    tempResults.Add(NumberList[picked]); // number picked
                }
            }

            if (thisPassword.UpperCases > 0)
            {
                var UpperList = Enumerable.Range(65, 26).ToList();
                if (thisPassword.EliminateSimiliarCharacters == true)
                {
                    UpperList.Remove(79); // O
                    UpperList.Remove(73); // I
                }

                var loopTo1 = thisPassword.UpperCases;
                for (x = 1; x <= loopTo1; x++)
                {
                    picked = Next(UpperList.Count);
                    tempResults.Add(UpperList[picked]);
                }
            }
            if (thisPassword.LowerCases > 0)
            {
                var LowerList = Enumerable.Range(97, 26).ToList();
                if (thisPassword.EliminateSimiliarCharacters == true)
                {
                    LowerList.Remove(111);
                    LowerList.Remove(108); // because l is too close to 1
                    LowerList.Remove(105);
                }

                var loopTo2 = thisPassword.LowerCases;
                for (x = 1; x <= loopTo2; x++)
                {
                    picked = Next(LowerList.Count);
                    tempResults.Add(LowerList[picked]);
                }
            }

            if (thisPassword.HowManySymbols > 0)
            {
                var loopTo3 = thisPassword.HowManySymbols;
                for (x = 1; x <= loopTo3; x++)
                {
                    picked = Next(thisPassword.SymbolList.Count);

                    string ThisSym = thisPassword.SymbolList[picked];

                    picked = VBCompat.AscW(ThisSym);

                    tempResults.Add(picked); // i think
                }
            }

            tempResults.ShuffleList(); //i now have this new list.  i might as well use this especially if i need random functions.
            string ResultString = "";
            foreach (var ThisItem in tempResults)
                ResultString += VBCompat.ChrW(ThisItem);
            return ResultString;
        }       
        

        public CustomBasicList<int> GenerateRandomNumberList(int maximumNumber, int howMany, int startingPoint = 0, int increments = 1)
        {
            CustomBasicList<int> firstList;
            if (increments <= 1)
                increments = 1;
            if (startingPoint >= maximumNumber)
                throw new ArgumentOutOfRangeException("MaximumNumber");// the arguments are out of range
            firstList = GetPossibleIntegerList(startingPoint, maximumNumber, increments);
            if (firstList.Count < 2)
                throw new ArgumentOutOfRangeException("MaximumNumber");
            DoRandomize();
            CustomBasicList<int> finalList = new CustomBasicList<int>();
            int x;
            var loopTo = howMany;
            for (x = 1; x <= loopTo; x++)
            {
                // can have repeating numbers
                var ask1 = Next(firstList.Count);
                finalList.Add(firstList[ask1]);
            }
            return finalList;
        }

        private CustomBasicList<int> GetPossibleIntegerList(int minValue, int maximumValue, int increments)
        {
            CustomBasicList<int> newList = new CustomBasicList<int>
            {
                minValue
            };
            int upTo;
            upTo = minValue;
            do
            {
                upTo += increments;
                if (upTo >= maximumValue)
                {
                    newList.Add(maximumValue);
                    return newList;
                }
                else
                    newList.Add(upTo);
            }
            while (true);
        }

        public int GetRandomNumber(int maxNumber, int startingPoint = 1, CustomBasicList<int>? PreviousList = null)
        {
            if (PreviousList != null)
            {
                if (PreviousList.Count == 0)
                    PreviousList = null;
            }
            DoRandomize();
            int randNum;
            if (startingPoint > maxNumber)
                ShowError();
            if (PreviousList == null)
            {
                randNum = Next(startingPoint, maxNumber + 1); //plus 1 was worse.  trying -1
                return randNum;
            }
            HashSet<int> rndIndexes = PreviousList.Where(a => a >= startingPoint && a <= maxNumber).Distinct().ToHashSet();
            int howManyPossible = maxNumber - startingPoint + 1 - rndIndexes.Count();
            if (howManyPossible < 1)
                ShowError();

            for (int i = 1; i <= startingPoint - 1; i++)
            {
                rndIndexes.Add(i);
            }
            bool rets;
            do
            {
                int index = Next(maxNumber);
                rets = rndIndexes.Add(index + 1);
                if (rets == true)
                    return index + 1; //because 0 based

            } while (true);
        }
        private void DoRandomize() //by this moment, you have to already have your interface that you need for random numbers.
        {
            lock(_thisObj)
            {
                if (_dids == true)
                    return;
                _privateID = Guid.NewGuid().GetHashCode(); //this may not be too bad for the new behavior for the random
                SetUpRandom();
                _dids = true;
            }   
        }
        public void SetRandomSeed(int value) //this can come from anywhere.  saved data, etc.
        {
            _privateID = value; //so it can be saved and used for testing (to more easily replay the game).
            SetUpRandom();
            _dids = true; //this means that this will use the same value every time.  useful for debugging.
        }
        private void SetUpRandom()
        {
            if (Formula == EnumFormula.NetStandard)
            {
                Random Temps = new Random(_privateID);
                _r = Temps.NextDouble;
                return;
            }
            MersenneTwister ts = new MersenneTwister((uint) _privateID);
            switch (Formula)
            {
                case EnumFormula.Twister2:
                    
                case EnumFormula.TwisterStandard:
                    _r = ts.GenRandReal2;
                    break;
                case EnumFormula.TwisterHigh:
                    _r = ts.GenRandRes53;
                    break;
                case EnumFormula.Twister1:
                    _r = ts.GenRandReal1;
                    break;
                case EnumFormula.Twister3:
                    _r = ts.GenRandReal3;
                    break;
                default:
                    break;
            }
        }
        private int PrivateHowManyPossible(int maxNumber, int startingNumber, int previousCount, int setCount)
        {
            int count = maxNumber - (startingNumber - 1);
            count -= previousCount;
            count -= setCount;
            return count;
        }
        public CustomBasicList<int> GenerateRandomList(int maxNumber, int howMany = -1, int startingNumber = 1, CustomBasicList<int>? previousList = null, CustomBasicList<int>? setToContinue = null, bool putBefore = false)
        {
            DoRandomize();
            if (howMany > maxNumber)
                throw new ArgumentOutOfRangeException(nameof(howMany)); //in this case, its obvious.
            if (maxNumber == 0)
                throw new ArgumentNullException(nameof(maxNumber));
            if (startingNumber > maxNumber)
                throw new ArgumentOutOfRangeException(nameof(startingNumber));
            bool isMax = false;
            if (howMany == -1)
            {
                howMany = maxNumber;
                isMax = true;
            }
            int adjustedMany = howMany;
            adjustedMany += startingNumber - 1;
            if (previousList != null && previousList.Exists(x => x > maxNumber || x <= startingNumber - 1))
                throw new ArgumentOutOfRangeException(nameof(previousList));

            if (setToContinue != null && setToContinue.Exists(x => x > maxNumber || x <= startingNumber - 1))
                throw new ArgumentException(nameof(setToContinue));
            int oldC;
            oldC = startingNumber - 1;
            int counts;
            CustomBasicList<int> oldList = new CustomBasicList<int>();
            int preC = 0;
            int setC = 0;
            if (previousList != null)
            {
                if (previousList.Count == 0)
                    throw new ArgumentOutOfRangeException(nameof(previousList), "If you sent previouslist, must contain at least one item");
                counts = previousList.Distinct().Count();
                if (counts != previousList.Count)
                    throw new ArgumentException("Previous List Had Duplicate Numbers", nameof(previousList));
                oldC += previousList.Count;
                adjustedMany += previousList.Count;
                oldList.AddRange(previousList);
                preC = previousList.Count();
            }
            if (setToContinue != null)
            {
                if (setToContinue.Count == 0)
                    throw new ArgumentOutOfRangeException(nameof(setToContinue), "If you sent settocontinue, must contain at least one item");
                counts = setToContinue.Distinct().Count();
                if (counts != setToContinue.Count)
                    throw new ArgumentException("Set List Had Duplicate Numbers", nameof(setToContinue));
                adjustedMany += setToContinue.Count;
                oldList.AddRange(setToContinue);
                oldC += setToContinue.Count;
                setC = setToContinue.Count();
            }
            if (setToContinue != null && previousList != null)
            {
                counts = oldList.Distinct().Count();
                if (counts != previousList.Count + setToContinue.Count)
                    throw new Exception("When combining the set list and previous list, there was duplicates.  This means can't do another list of non duplicate numbers");
            }
            bool isSingle = false;
            int total;
            total = PrivateHowManyPossible(maxNumber, startingNumber, preC, setC);
            if (isMax == false && total < howMany)
                throw new Exception("Since you are not choosing match, not reconciling.  Will cause a never ending loop or you get less than expected");
            if (total == 1)
                isSingle = true;
            if (total < 1)
                throw new Exception("Does not reconcile to randomize.   Will result in a never ending loop");
            if (isSingle == true)
            {
                CustomBasicList<int> tempList = Enumerable.Range(startingNumber, maxNumber - startingNumber + 1).ToCustomBasicList();
                if (oldList.Count > tempList.Count)
                    throw new Exception("Unable to get the one number remaining.  Something is corrupted.  Rethink");
                if (oldList.Count == 0)
                    return new CustomBasicList<int> { startingNumber };
                int possibleItem = 0;
                foreach (int index in tempList)
                {
                    if (oldList.Contains(index) == false)
                    {
                        if (possibleItem > 0)
                            throw new Exception("Getting single item failed.  Rethink");
                        possibleItem = index;
                    }
                }
                if (possibleItem == 0)
                    throw new Exception("The single item not found");
                if (setToContinue == null)
                    return new CustomBasicList<int> { possibleItem }; //should not bother doing the random items because there is only one.
                CustomBasicList<int> finalList = new CustomBasicList<int>();
                if (putBefore == true)
                {
                    finalList.AddRange(setToContinue);
                    finalList.Add(possibleItem);
                }
                else
                {
                    finalList.Add(possibleItem);
                    finalList.AddRange(setToContinue);
                }
                return finalList;
            }
            HashSet<int> rndIndexes = new HashSet<int>();
            for (int i = 1; i <= startingNumber - 1; i++)
            {
                rndIndexes.Add(i);
            }
            bool rets;
            if (previousList != null)
            {
                foreach (int index in previousList)
                {
                    rets = rndIndexes.Add(index);
                    if (rets == false)
                        throw new Exception("Previous List Failed.  Rethink");
                }
            }
            if (setToContinue != null)
            {
                foreach (int index in setToContinue)
                {
                    rets = rndIndexes.Add(index);
                    if (rets == false)
                        throw new Exception("Set To Continue Failed.  Rethink");
                }
            }
            while (rndIndexes.Count != adjustedMany)
            {
                int index = Next(maxNumber);
                rndIndexes.Add(index + 1);
            }
            for (int i = 1; i <= startingNumber - 1; i++)
            {
                rndIndexes.Remove(i);
            }
            if (previousList != null)
            {
                foreach (int index in previousList)
                {
                    rndIndexes.Remove(index);
                }
            }
            CustomBasicList<int> thisList = rndIndexes.ToCustomBasicList();
            if (setToContinue != null && putBefore == false)
            {
                foreach (int index in setToContinue)
                {
                    thisList.RemoveSpecificItem(index);
                    thisList.Add(index);
                }
            }
            return thisList;
        }

        /// <summary>
        /// Returns a random bool, either true or false.
        /// </summary>
        /// <param name="likelihood">The default likelihood of success (returning true) is 50%.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>likelihood</c> is less than 0 or greater than 100.</exception>
        /// <returns>Returns a random bool, either true or false.</returns>
        public bool NextBool(int likelihood = 50)
        {
            if (likelihood < 0 || likelihood > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(likelihood), "Likelihood accepts values from 0 to 100.");
            }
            DoRandomize(); //maybe has to be here.
            return _r!() * 100 < likelihood;
        }
    }
}