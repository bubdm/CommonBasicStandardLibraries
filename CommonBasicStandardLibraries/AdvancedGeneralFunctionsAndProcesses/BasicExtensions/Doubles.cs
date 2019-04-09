using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Doubles
    {
        public static List<decimal> SplitMoney(this double Amount, int HowManyToSplitBy)
        {
            decimal TempAmount = decimal.Parse(Amount.ToString());

            decimal Splits = TempAmount / HowManyToSplitBy;
            // needs rounding
            Splits = decimal.Round(Splits, 2);
            List<decimal> ThisList = new List<decimal>();
            decimal TotalUsed;
            TotalUsed = 0;
            var loopTo = HowManyToSplitBy;
            for (var x = 1; x <= loopTo; x++)
            {
                TotalUsed += Splits;
                ThisList.Add(Splits);
            }
            if (TotalUsed == TempAmount)
                return ThisList;
            decimal Lefts;
            decimal Diffs;
            Diffs = TempAmount - TotalUsed;
            Lefts = Math.Abs(Diffs);
            Lefts *= 100; // i think this
            decimal AddAmount;
            if (Diffs < 0)
                AddAmount = -0.01m;
            else
                AddAmount = 0.01m;
            var loopTo1 = Lefts;
            for (var x = 1; x <= loopTo1; x++)
                ThisList[x - 1] += AddAmount;
            return ThisList;
        }

        public static int RoundToHigherNumber(this double ThisDou)
        {
            string ThisStr = ThisDou.ToString();
            if (ThisStr.Contains(".") == false)
                return int.Parse(ThisDou.ToString());
            CustomBasicList<string> ThisList = ThisStr.Split(".").ToCustomBasicList();
            int ThisValue = int.Parse(ThisList.First());
            return ThisValue + 1;
        }

        public static int RoundToLowerNumber(this double ThisDou)
        {
            string ThisStr = ThisDou.ToString();
            if (ThisStr.Contains(".") == false)
                return int.Parse(ThisDou.ToString());
            CustomBasicList<string> ThisList = ThisStr.Split(".").ToCustomBasicList();
            return int.Parse(ThisList.First());
        }
        public static int Multiply(this double ThisAmount, int HowMuch)
        {
            return (int)Math.Ceiling((ThisAmount) * HowMuch);
        }

    }
}
