using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.Strings;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class EnumConverter : IConverterCP
    {
        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            string ThisStr = value.ToString();
            return ThisStr.GetWords();
        }

        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
