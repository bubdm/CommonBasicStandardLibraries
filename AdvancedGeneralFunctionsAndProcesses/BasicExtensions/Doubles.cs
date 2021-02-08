using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Linq;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Doubles
    {
        public static CustomBasicList<decimal> SplitMoney(this double amount, int howManyToSplitBy) //decided to use the new custom list for this now.
        {
            decimal tempAmount = decimal.Parse(amount.ToString());
            decimal splits = tempAmount / howManyToSplitBy;
            splits = decimal.Round(splits, 2);
            CustomBasicList<decimal> ThisList = new CustomBasicList<decimal>();
            decimal totalUsed;
            totalUsed = 0;
            var loopTo = howManyToSplitBy;
            for (var x = 1; x <= loopTo; x++)
            {
                totalUsed += splits;
                ThisList.Add(splits);
            }
            if (totalUsed == tempAmount)
                return ThisList;
            decimal lefts;
            decimal diffs;
            diffs = tempAmount - totalUsed;
            lefts = Math.Abs(diffs);
            lefts *= 100; // i think this
            decimal addAmount;
            if (diffs < 0)
                addAmount = -0.01m;
            else
                addAmount = 0.01m;
            var loopTo1 = lefts;
            for (var x = 1; x <= loopTo1; x++)
                ThisList[x - 1] += addAmount;
            return ThisList;
        }
        public static int RoundToHigherNumber(this double thisDou)
        {
            string ThisStr = thisDou.ToString();
            if (ThisStr.Contains(".") == false)
                return int.Parse(thisDou.ToString());
            CustomBasicList<string> ThisList = ThisStr.Split(".").ToCustomBasicList();
            int ThisValue = int.Parse(ThisList.First());
            return ThisValue + 1;
        }
        public static int RoundToLowerNumber(this double thisDou)
        {
            string thisStr = thisDou.ToString();
            if (thisStr.Contains(".") == false)
                return int.Parse(thisDou.ToString());
            CustomBasicList<string> thisList = thisStr.Split(".").ToCustomBasicList();
            return int.Parse(thisList.First());
        }
        public static double MultiplyAndAdd(this double original, double amount)
        {
            double subs = original * amount;
            return subs + original;
        }
        public static int Multiply(this double thisAmount, int howMuch)
        {
            return (int)Math.Ceiling((thisAmount) * howMuch);
        }
    }
}