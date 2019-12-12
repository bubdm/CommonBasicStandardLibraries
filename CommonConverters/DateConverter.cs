using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Globalization;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class DateConverter : IConverterCP
    {
        public bool UseDayOfWeek { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            if (UseDayOfWeek == true)
            {
                DateTime newDate = (DateTime)value;
                return $"{newDate.DayOfWeek.ToString()} {newDate.Month}/{newDate.Day}/{newDate.Year}";
            }

            DateTime thisDate = (DateTime)value;
            if (thisDate == default)
                return "";
            if (thisDate.Year == 1 || thisDate.Year == 1000)
                throw new BasicBlankException("Using default for date with date converter did not work. Rethink");
            string dayString = thisDate.Day.ToString("00");
            string monthString = thisDate.Month.ToString("00");
            string yearString = thisDate.Year.ToString("00");
            return monthString + dayString + yearString;
        }
        public object ConvertBack(object value, Type targetType, object Parameter, CultureInfo culture)
        {
            if (value == null)
                return null!;
            string thisText = value.ToString();
            if (string.IsNullOrWhiteSpace(thisText) == true)
                return null!;

            bool rets;
            rets = DateTime.TryParse(thisText, out DateTime tDate);
            if (rets == true)
                return tDate;
            rets = thisText.IsValidDate(out DateTime? newDate);
            if (rets == false)
                return "01/01/1000";
            return newDate!.Value;
        }
    }
}
