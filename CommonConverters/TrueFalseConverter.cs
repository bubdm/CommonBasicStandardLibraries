using System;
using System.Globalization;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class TrueFalseConverter : IConverterCP
    {
        public bool UseAbb { get; set; } = true;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool thisBool = bool.Parse(value.ToString());
            if (thisBool == true)
            {
                if (UseAbb == true)
                    return "Y";
                else
                    return "Yes";
            }
            if (UseAbb == true)
                return "N";
            return "No";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string thisStr = value.ToString();
            if (UseAbb == true)
            {
                if (thisStr == "Y")
                    return true;
                if (thisStr == "N")
                    return false;
            }
            if (thisStr == "Yes")
                return true;
            return false;
        }
    }
}