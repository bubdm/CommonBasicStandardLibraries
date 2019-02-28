using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.Exceptions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class DateConverter : IConverterCP
    {
        public bool UseDayOfWeek { get; set; }

        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            if (UseDayOfWeek == true)
            {
                DateTime NewDate = (DateTime)value;
                return $"{NewDate.DayOfWeek.ToString()} {NewDate.Month}/{NewDate.Day}/{NewDate.Year}";
            }

            DateTime ThisDate = (DateTime)value;
            if (ThisDate == default)
                return "";
            if (ThisDate.Year == 1 || ThisDate.Year==1000)
                throw new BasicBlankException("Using default for date with date converter did not work. Rethink");
            string DayString = ThisDate.Day.ToString("00");
            string MonthString = ThisDate.Month.ToString("00");
            string YearString = ThisDate.Year.ToString("00");
            return MonthString + DayString + YearString;
        }

        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            string ThisText = value.ToString();
            if (string.IsNullOrWhiteSpace(ThisText) == true)
                return null;

            bool rets;
			rets = DateTime.TryParse(ThisText, out DateTime TDate);
			if (rets == true)
                return TDate;
			rets = ThisText.IsValidDate(out DateTime? NewDate);
			if (rets == false)
                return "01/01/1000";
            return NewDate.Value;
        }
    }
}
