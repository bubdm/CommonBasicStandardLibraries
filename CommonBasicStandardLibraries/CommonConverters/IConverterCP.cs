using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace CommonBasicStandardLibraries.CommonConverters
{
    public interface IConverterCP
    {
        object Convert(object value, Type TargetType, object Parameter, CultureInfo culture);

        object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture);
        //this means if you do converters for both, can use this for public.
        //then use adapter to convert to what is really needed.
    }
}
