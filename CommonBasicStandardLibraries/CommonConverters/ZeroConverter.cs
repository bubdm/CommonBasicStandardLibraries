using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;

namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class ZeroConverter : IConverterCP
    {

        public bool DoPutBack { get; set; }

        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            string ThisText = value.ToString();
            if (ThisText == "0")
                return "";
            return ThisText;
        }

        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
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
