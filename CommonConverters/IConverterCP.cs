using System;
using System.Globalization;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public interface IConverterCP
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        //this means if you do converters for both, can use this for public.
        //then use adapter to convert to what is really needed.
    }
}