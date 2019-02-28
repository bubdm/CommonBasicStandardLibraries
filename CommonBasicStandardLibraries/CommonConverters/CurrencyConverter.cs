using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static  CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class CurrencyConverter : IConverterCP //has to be abstract because it needs to implement the others this does not know about yet.
    {
        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            decimal ThisDec = decimal.Parse(value.ToString());
            return ThisDec.ToString("c");
        }

        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
