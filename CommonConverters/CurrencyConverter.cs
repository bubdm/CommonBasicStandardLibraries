using System;
using System.Globalization;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class CurrencyConverter : IConverterCP //has to be abstract because it needs to implement the others this does not know about yet.
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal thisDec = decimal.Parse(value.ToString());
            return thisDec.ToString("c");
        }

        public object ConvertBack(object value, Type targetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}