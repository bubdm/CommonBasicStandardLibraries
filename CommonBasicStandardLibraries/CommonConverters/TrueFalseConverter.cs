using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;

namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class TrueFalseConverter : IConverterCP
    {

        public bool UseAbb { get; set; } = true;
        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            bool ThisBool = Boolean.Parse(value.ToString());
            if (ThisBool == true)
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

        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            string ThisStr = value.ToString();
            if (UseAbb == true)
            {
                if (ThisStr == "Y")
                    return true;
                if (ThisStr == "N")
                    return false;
            }
            if (ThisStr == "Yes")
                return true;
            return false;

        }
    }
}
