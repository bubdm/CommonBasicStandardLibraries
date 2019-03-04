using System;
using System.Collections.Generic;
using System.Linq;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
//using MVVMHelpers;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator
{
    public static class RandomGenerator
    {
        private static bool Dids = false;
        private static Random r;
        //if i need the times to shuffle, can do it.

        private static void ShowError()
        {
            throw new Exception("Random number could not be generated, range to narrow");
        }        

        public static string GenerateRandomPassword()
        {
            RandomPasswordParameterClass ThisPassword = new RandomPasswordParameterClass();
            return GenerateRandomPassword(ThisPassword);
        }

        public static string GenerateRandomPassword(RandomPasswordParameterClass ThisPassword)
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
                    picked = r.Next(NumberList.Count);
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
                    picked = r.Next(UpperList.Count);
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
                    picked = r.Next(LowerList.Count);
                    TempResults.Add(LowerList[picked]);
                }
            }

            if (ThisPassword.HowManySymbols > 0)
            {
                var loopTo3 = ThisPassword.HowManySymbols;
                for (x = 1; x <= loopTo3; x++)
                {
                    picked = r.Next(ThisPassword.SymbolList.Count);

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
        

        public static List<int> GenerateRandomNumberList(int MaximumNumber, int HowMany, int StartingPoint = 0, int Increments = 1)
        {
            List<int> FirstList;
            if (Increments <= 1)
                Increments = 1;
            if (StartingPoint >= MaximumNumber)
                throw new ArgumentOutOfRangeException("MaximumNumber");// the arguments are out of range
            FirstList = GetPossibleIntegerList(StartingPoint, MaximumNumber, Increments);
            if (FirstList.Count < 2)
                throw new ArgumentOutOfRangeException("MaximumNumber");
            DoRandomize();
            List<int> FinalList = new List<int>();
            int x;
            var loopTo = HowMany;
            for (x = 1; x <= loopTo; x++)
            {
                // can have repeating numbers
                var ask1 = r.Next(FirstList.Count);
                FinalList.Add(FirstList[ask1]);
            }
            return FinalList;
        }

        private static List<int> GetPossibleIntegerList(int MinValue, int MaximumValue, int Increments)
        {
            List<int> NewList = new List<int>
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

        public static int GetRandomNumber(int MaxNumber, int StartingPoint = 1, List<int> PreviousList = null)
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
                
                RandNum = r.Next(StartingPoint, MaxNumber + 1);
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
                int index = r.Next(MaxNumber);
                rets = rndIndexes.Add(index + 1);
                if (rets == true)
                    return index + 1; //because 0 based

            } while (true);
            

        }

        private static void DoRandomize()
        {
            if (Dids == true)
                return;
            // Randomize()
            r = new Random();
            Dids = true;
        }




        private static int PrivateHowManyPossible(int MaxNumber, int StartingNumber, int PreviousCount, int SetCount)
        {
            int Count = MaxNumber - (StartingNumber - 1);
            Count -= PreviousCount;
            Count -= SetCount;
            return Count;
        }

        public static List<int> GenerateRandomList(int MaxNumber, int HowMany = -1, int StartingNumber = 1, List<int> PreviousList = null, List<int> SetToContinue = null, bool PutBefore = false)
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
            List<int> OldList = new List<int>();
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
                    return new List<int> { StartingNumber };

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
                    return new List<int> { PossibleItem }; //should not bother doing the random items because there is only one.
                List<int> FinalList = new List<int>();
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
                int index = r.Next(MaxNumber);
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
            List<int> ThisList = rndIndexes.ToList();
            if (SetToContinue != null && PutBefore == false)
            {
                foreach (int Index in SetToContinue)
                {
                    ThisList.Remove(Index);
                    ThisList.Add(Index);
                }
            }
            return ThisList;
        }

    }
}
