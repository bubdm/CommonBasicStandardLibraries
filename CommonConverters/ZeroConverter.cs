using System;
using System.Globalization;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class ZeroConverter : IConverterCP
    {
        public bool DoPutBack { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string thisText = value.ToString();
            if (thisText == "0")
                return "";
            return thisText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DoPutBack == false)
                throw new NotImplementedException();
            if (value == null)
                return 0;
            if (string.IsNullOrWhiteSpace(value.ToString()) == true)
                return 0;
            return value;
        }
    }
}