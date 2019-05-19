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


        private bool Dids = false;
        //private static Random r;

        private Func<double> r;

        //if i need the times to shuffle, can do it.
        private int PrivateID;
        public EnumFormula Formula = EnumFormula.TwisterStandard;

        //looks like i still get to do the seed part.
        //that is necessary because i found out from tests that if i don't, then theirs can still be same numbers.
        //i tried with the new guid and that worked.

        private readonly object ThisObj = new object(); //so it has to lock when initializing.

        internal int Next(int Max) //since i did quite a bit
        {
            //return (int) Math.Floor(_random()*(max - min + 1) + min);
            return (int) Math.Floor(r() * (Max));
        }

        internal int Next(int Min, int Max) //since i did quite a bit
        {
            //return (int) Math.Floor(_random()*(max - min + 1) + min);
            return (int)Math.Floor(r() * (Max - Min) + Min); //should have been minus and not plus.
        }

        public int GetSeed()
        {
            return PrivateID;
        }

        private void ShowError()
        {
            throw new BasicBlankException("Random number could not be generated, range to narrow");
        }        

        public string GenerateRandomPassword()
        {
            RandomPasswordParameterClass ThisPassword = new RandomPasswordParameterClass();
            return GenerateRandomPassword(ThisPassword);
        }

        public string GenerateRandomPassword(RandomPasswordParameterClass ThisPassword)
        {
            DoRandomize();
            CustomBasicList<int> TempResults = new CustomBasicList<int>();
            int x;
            int picked;
            if (ThisPassword.HowManyNumbers > 0)
            {
                var NumberList = Enumerable.Range(48, 10).ToList();
                if (ThisPassword.EliminateSimiliarCharacters == true)
                {
                    NumberList.Remove(48); // because that is a 0
                    NumberList.Remove(49); // because 1 is close to l or I
                }

                var loopTo = ThisPassword.HowManyNumbers;
                for (x = 1; x <= loopTo; x++)
                {
                    picked = Next(NumberList.Count);
                    TempResults.Add(NumberList[picked]); // number picked
                }
            }

            if (ThisPassword.UpperCases > 0)
            {
                var UpperList = Enumerable.Range(65, 26).ToList();
                if (ThisPassword.EliminateSimiliarCharacters == true)
                {
                    UpperList.Remove(79); // O
                    UpperList.Remove(73); // I
                }

                var loopTo1 = ThisPassword.UpperCases;
                for (x = 1; x <= loopTo1; x++)
                {
                    picked = Next(UpperList.Count);
                    TempResults.Add(UpperList[picked]);
                }
            }
            if (ThisPassword.LowerCases > 0)
            {
                var LowerList = Enumerable.Range(97, 26).ToList();
                if (ThisPassword.EliminateSimiliarCharacters == true)
                {
                    LowerList.Remove(111);
                    LowerList.Remove(108); // because l is too close to 1
                    LowerList.Remove(105);
                }

                var loopTo2 = ThisPassword.LowerCases;
                for (x = 1; x <= loopTo2; x++)
                {
                    picked = Next(LowerList.Count);
                    TempResults.Add(LowerList[picked]);
                }
            }

            if (ThisPassword.HowManySymbols > 0)
            {
                var loopTo3 = ThisPassword.HowManySymbols;
                for (x = 1; x <= loopTo3; x++)
                {
                    picked = Next(ThisPassword.SymbolList.Count);

                    string ThisSym = ThisPassword.SymbolList[picked];

                    picked = VBCompat.AscW(ThisSym);

                    TempResults.Add(picked); // i think
                }
            }

            TempResults.ShuffleList(); //i now have this new list.  i might as well use this especially if i need random functions.
            string ResultString = "";
            foreach (var ThisItem in TempResults)
                ResultString += VBCompat.ChrW(ThisItem);
            return ResultString;
        }       
        

        public CustomBasicList<int> GenerateRandomNumberList(int MaximumNumber, int HowMany, int StartingPoint = 0, int Increments = 1)
        {
            CustomBasicList<int> FirstList;
            if (Increments <= 1)
                Increments = 1;
            if (StartingPoint >= MaximumNumber)
                throw new ArgumentOutOfRangeException("MaximumNumber");// the arguments are out of range
            FirstList = GetPossibleIntegerList(StartingPoint, MaximumNumber, Increments);
            if (FirstList.Count < 2)
                throw new ArgumentOutOfRangeException("MaximumNumber");
            DoRandomize();
            CustomBasicList<int> FinalList = new CustomBasicList<int>();
            int x;
            var loopTo = HowMany;
            for (x = 1; x <= loopTo; x++)
            {
                // can have repeating numbers
                var ask1 = Next(FirstList.Count);
                FinalList.Add(FirstList[ask1]);
            }
            return FinalList;
        }

        private CustomBasicList<int> GetPossibleIntegerList(int MinValue, int MaximumValue, int Increments)
        {
            CustomBasicList<int> NewList = new CustomBasicList<int>
            {
                MinValue
            };
            int UpTo;
            UpTo = MinValue;
            do
            {
                UpTo += Increments;
                if (UpTo >= MaximumValue)
                {
                    NewList.Add(MaximumValue);
                    return NewList;
                }
                else
                    NewList.Add(UpTo);
            }
            while (true);
        }

        public int GetRandomNumber(int MaxNumber, int StartingPoint = 1, List<int> PreviousList = null)
        {
            if (PreviousList == null == false)
            {
                if (PreviousList.Count == 0)
                    PreviousList = null;
            }
            DoRandomize();
            int RandNum;
            if (StartingPoint > MaxNumber)
                ShowError();
            if (PreviousList == null == true)
            {
                
                RandNum = Next(StartingPoint, MaxNumber + 1); //plus 1 was worse.  trying -1
                return RandNum;
            }
            

            HashSet<int> rndIndexes = PreviousList.Where(a => a >= StartingPoint && a <= MaxNumber).Distinct().ToHashSet();



            int HowManyPossible = MaxNumber - StartingPoint + 1 - rndIndexes.Count();
            if (HowManyPossible < 1)
                ShowError();

            for (int i = 1; i <= StartingPoint - 1; i++)
            {
                rndIndexes.Add(i);
            }
            bool rets;
            do
            {
                int index = Next(MaxNumber);
                rets = rndIndexes.Add(index + 1);
                if (rets == true)
                    return index + 1; //because 0 based

            } while (true);
            

        }

        private void DoRandomize() //by this moment, you have to already have your interface that you need for random numbers.
        {
            lock(ThisObj)
            {
                if (Dids == true)
                    return;
                PrivateID = Guid.NewGuid().GetHashCode(); //this may not be too bad for the new behavior for the random
                SetUpRandom();
                //r = new Random(PrivateID);

                Dids = true;
            }
            
        }

        public void SetRandomSeed(int Value) //this can come from anywhere.  saved data, etc.
        {
            PrivateID = Value; //so it can be saved and used for testing (to more easily replay the game).
            //r = new Random(Value);

            SetUpRandom();

            Dids = true; //this means that this will use the same value every time.  useful for debugging.
        }

        private void SetUpRandom()
        {
            if (Formula == EnumFormula.NetStandard)
            {
                //use random
                Random Temps = new Random(PrivateID);
                r = Temps.NextDouble;
                return;
            }

            MersenneTwister ts = new MersenneTwister((uint) PrivateID);

            switch (Formula)
            {
                case EnumFormula.Twister2:
                    
                case EnumFormula.TwisterStandard:
                    r = ts.GenRandReal2;
                    break;
                case EnumFormula.TwisterHigh:
                    r = ts.GenRandRes53;
                    break;
                case EnumFormula.Twister1:
                    r = ts.GenRandReal1;
                    break;
                
                case EnumFormula.Twister3:
                    r = ts.GenRandReal3;
                    break;
                default:
                    break;
            }

            //else
            //throw new NotImplementedException("Has to test standard before implementing twister");
        }

        private int PrivateHowManyPossible(int MaxNumber, int StartingNumber, int PreviousCount, int SetCount)
        {
            int Count = MaxNumber - (StartingNumber - 1);
            Count -= PreviousCount;
            Count -= SetCount;
            return Count;
        }

        public CustomBasicList<int> GenerateRandomList(int MaxNumber, int HowMany = -1, int StartingNumber = 1, List<int> PreviousList = null, List<int> SetToContinue = null, bool PutBefore = false)
        {
            DoRandomize();
            if (HowMany > MaxNumber)
                throw new ArgumentOutOfRangeException(nameof(HowMany)); //in this case, its obvious.
            if (MaxNumber == 0)
                throw new ArgumentNullException(nameof(MaxNumber));
            if (StartingNumber > MaxNumber)
                throw new ArgumentOutOfRangeException(nameof(StartingNumber));

            bool IsMax = false;
            if (HowMany == -1)
            {
                HowMany = MaxNumber;
                IsMax = true;
            }

            int AdjustedMany = HowMany;
            AdjustedMany += StartingNumber - 1;
            if (PreviousList != null && PreviousList.Exists(x => x > MaxNumber || x <= StartingNumber - 1))
                throw new ArgumentOutOfRangeException(nameof(PreviousList));

            if (SetToContinue != null && SetToContinue.Exists(x => x > MaxNumber || x <= StartingNumber - 1))
                throw new ArgumentException(nameof(SetToContinue));

            int OldC;

            OldC = StartingNumber - 1;
            int Counts;
            CustomBasicList<int> OldList = new CustomBasicList<int>();
            int PreC = 0;
            int SetC = 0;
            if (PreviousList != null)
            {
                if (PreviousList.Count == 0)
                    throw new ArgumentOutOfRangeException(nameof(PreviousList), "If you sent previouslist, must contain at least one item");
                Counts = PreviousList.Distinct().Count();
                if (Counts != PreviousList.Count)
                    throw new ArgumentException("Previous List Had Duplicate Numbers", nameof(PreviousList));
                OldC += PreviousList.Count;
                AdjustedMany += PreviousList.Count;
                OldList.AddRange(PreviousList);
                PreC = PreviousList.Count();
            }

            if (SetToContinue != null)
            {
                if (SetToContinue.Count == 0)
                    throw new ArgumentOutOfRangeException(nameof(SetToContinue), "If you sent settocontinue, must contain at least one item");
                Counts = SetToContinue.Distinct().Count();
                if (Counts != SetToContinue.Count)
                    throw new ArgumentException("Set List Had Duplicate Numbers", nameof(SetToContinue));
                AdjustedMany += SetToContinue.Count;
                OldList.AddRange(SetToContinue);
                OldC += SetToContinue.Count;
                SetC = SetToContinue.Count();
            }
            if (SetToContinue != null && PreviousList != null)
            {
                Counts = OldList.Distinct().Count();
                if (Counts != PreviousList.Count + SetToContinue.Count)
                    throw new Exception("When combining the set list and previous list, there was duplicates.  This means can't do another list of non duplicate numbers");
            }
            bool IsSingle = false;

            int Total;

            Total = PrivateHowManyPossible(MaxNumber, StartingNumber, PreC, SetC);
            if (IsMax == false && Total < HowMany)
                throw new Exception("Since you are not choosing match, not reconciling.  Will cause a never ending loop or you get less than expected");
            if (Total == 1)
                IsSingle = true;

            if (Total < 1)
                throw new Exception("Does not reconcile to randomize.   Will result in a never ending loop");


            if (IsSingle == true)
            {
                List<int> TempList = Enumerable.Range(StartingNumber, MaxNumber - StartingNumber + 1).ToList();
                if (OldList.Count > TempList.Count)
                    throw new Exception("Unable to get the one number remaining.  Something is corrupted.  Rethink");
                if (OldList.Count == 0)
                    return new CustomBasicList<int> { StartingNumber };

                int PossibleItem = 0;



                foreach (int Index in TempList)
                {
                    if (OldList.Contains(Index) == false)
                    {
                        if (PossibleItem > 0)
                            throw new Exception("Getting single item failed.  Rethink");
                        PossibleItem = Index;
                    }
                }
                if (PossibleItem == 0)
                    throw new Exception("The single item not found");
                if (SetToContinue == null)
                    return new CustomBasicList<int> { PossibleItem }; //should not bother doing the random items because there is only one.
                CustomBasicList<int> FinalList = new CustomBasicList<int>();
                if (PutBefore == true)
                {
                    FinalList.AddRange(SetToContinue);
                    FinalList.Add(PossibleItem);
                }
                else
                {
                    FinalList.Add(PossibleItem);
                    FinalList.AddRange(SetToContinue);
                }
                return FinalList;
            }


            HashSet<int> rndIndexes = new HashSet<int>();
            for (int i = 1; i <= StartingNumber - 1; i++)
            {
                rndIndexes.Add(i);
            }
            bool rets;
            if (PreviousList != null)
            {
                foreach (int Index in PreviousList)
                {
                    rets = rndIndexes.Add(Index);
                    if (rets == false)
                        throw new Exception("Previous List Failed.  Rethink");
                }

            }
            if (SetToContinue != null)
            {
                foreach (int Index in SetToContinue)
                {
                    rets = rndIndexes.Add(Index);
                    if (rets == false)
                        throw new Exception("Set To Continue Failed.  Rethink");
                }
            }
            while (rndIndexes.Count != AdjustedMany)
            {
                int index = Next(MaxNumber);
                rndIndexes.Add(index + 1);
            }
            for (int i = 1; i <= StartingNumber - 1; i++)
            {
                rndIndexes.Remove(i);
            }
            if (PreviousList != null)
            {
                foreach (int Index in PreviousList)
                {
                    rndIndexes.Remove(Index);
                }
            }
            CustomBasicList<int> ThisList = rndIndexes.ToCustomBasicList();
            if (SetToContinue != null && PutBefore == false)
            {
                foreach (int Index in SetToContinue)
                {
                    ThisList.RemoveSpecificItem(Index);
                    ThisList.Add(Index);
                }
            }
            return ThisList;
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
            return r() * 100 < likelihood;
        }

    }
}
