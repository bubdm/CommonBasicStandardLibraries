using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Dates
    {
        public static string GetLongDate(this DateTime ThisDate)
        {
            // Imports System.Runtime.CompilerServices
            return ThisDate.DayOfWeek.ToString() + " " + ThisDate.Month + "/" + ThisDate.Day + "/" + ThisDate.Year;
        }

        public static DateTime WhenIsThanksgivingThisYear()
        {
            // needs month and this year
            // has to be at least 22nd
            // Dim TempDate = New Date(Year(Now), 11, 22)
            var ThisDate = DateTime.Now;
            int x;
            for (x = 22; x <= 30; x++)
            {
                // Dim TempDate = New Date(Year(Now), 11, x)
                var TempDate = new DateTime(ThisDate.Year, 11, x);
                if (TempDate.DayOfWeek == DayOfWeek.Thursday)
                    return TempDate;
            }
            throw new Exception("Cannot find when thanksgiving is this year");
        }


        public static bool IsBetweenThanksgivingAndChristmas(this DateTime ThisDate)
        {
            if (ThisDate.Month == 12)
            {
                if (ThisDate.Day <= 25)
                    return true;
                return false;
            }
            if (ThisDate.Month == 11)
            {
                if (ThisDate.Day < 22)
                    return false;
                var TempDate = DateTime.Now;
                ThisDate = new DateTime(TempDate.Year, 11, ThisDate.Day);
                var TDate = WhenIsThanksgivingThisYear();
                TDate = TDate.AddDays(1);
                if (ThisDate >= TDate)
                    return true;
            }
            return false;
        }

    }
}
