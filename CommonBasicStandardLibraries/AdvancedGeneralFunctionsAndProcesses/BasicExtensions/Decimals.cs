using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Decimals
    {
        //if we need integer or double extensions, still needed


        public static List<decimal> SplitMoney(this decimal Amount, int HowManyToSplitBy)
        {
            decimal Splits = Amount / HowManyToSplitBy;
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
            if (TotalUsed == Amount)
                return ThisList;
            decimal Lefts;
            decimal Diffs;
            Diffs = Amount - TotalUsed;
            Lefts = Math.Abs(Diffs);
            Lefts = Lefts * 100; // i think this
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

        public static bool IsPaidInFull(this decimal AmountPaid, decimal TotalDue)
        {
            decimal RemainingToPay = TotalDue - AmountPaid;
            if (RemainingToPay < 0.01M)
                return true;
            else
                return false;
        }

        public static decimal RoundPrice(this decimal ThisPrice, decimal CentAmount)
        {
            if (CentAmount >= 1)
                throw new Exception("Must be less than 1 dollar");
            if (CentAmount < 0)
                throw new Exception("Must be at least 0");
            // has to be a decimal amount (.99 for example or .89)
            var NewPrice = ThisPrice - 1;
            var ThisStr = NewPrice.ToString("##0.00"); // try this way
                                                       // NewPrice = Format(NewPrice, "##0.00")
            int NewSearch;
            // Dim ThisStr = NewPrice.ToString
            if (ThisStr.Contains(".") == false)
                NewSearch = -1;
            else
                NewSearch = ThisStr.IndexOf(".");
            decimal Centss;
            decimal Diffs;
            if (NewSearch == -1)
            {
                Centss = 0;
                Diffs = CentAmount * 100;
            }
            else
            {
                Centss = decimal.Parse(ThisStr.Substring(NewSearch)); // try this
                Diffs = (CentAmount * 100) - Centss;
            }
            Diffs = Diffs * 0.01m;
            return NewPrice + Diffs;
        }

        private static int HowMany100s(this decimal DeclaredValue, decimal MaxValue, decimal HowMuchFree = 0)
        {
            // if its below 100; that counts as 1 unless the first 100 is free
            // not sure if i need this or not?
            int x;
            int y;
            y = 0;
            if (DeclaredValue > MaxValue)
                throw new Exception("The max value is " + MaxValue + ".  However; the declared value is " + DeclaredValue);
            var loopTo = MaxValue;
            for (x = (int)HowMuchFree; x <= loopTo; x += 100)
            {
                y += 1;
                if (x >= DeclaredValue)
                    return y - 1;
            }
            return y; // maybe will be y by itself in this case
        }
        public static (int Pounds, int Ounces) PoundsOunces(Single GrossWeight)
        {
            var ThisItem = GrossWeight.ToString();
            if (ThisItem.Contains(".") == true)
                return ((int)GrossWeight, 0);

            int Firsts;
            Firsts = int.Parse(ThisItem.PartialString(".", true));
            decimal Seconds;
            Seconds = decimal.Parse("." + ThisItem.PartialString(".", false));
            int SecFin;
            SecFin = (int)Seconds * 16;
            return (Firsts, SecFin);

        }

        public static decimal ChargeBy100s(this decimal DeclaredValue, decimal PerPoundCharge, decimal MaxValue, decimal HowMuchFree = 0)
        {
            if (DeclaredValue <= HowMuchFree)
                return 0;// because its below the free charge
                         // its based on 100's starting at when its free
                         // everything must have a max currently.
            var TempHowMuch = DeclaredValue.HowMany100s(MaxValue, HowMuchFree);
            if (TempHowMuch <= 0)
                throw new Exception("Cannot multiply by 0 or less in order to calculate the charge per 100");
            return TempHowMuch * PerPoundCharge;
        }

        public static string ToCurrency(this decimal ThisDec, int DecimalPlaces = 2)
        {
            return ThisDec.ToString("c" + DecimalPlaces);
        }
    }
}
