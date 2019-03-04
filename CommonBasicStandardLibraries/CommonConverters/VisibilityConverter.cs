using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;

namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class VisibilityConverter : IConverterCP
    {
        public VisibleTranslation VisibleDelegate;

        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {

            if (value == null)
                return FinalResults(true);
            bool rets = bool.TryParse(value.ToString(), out bool fins);
            if (rets == false)
                return FinalResults(true);
            return FinalResults(fins);

        }

        private object FinalResults(bool Value)
        {
            if (VisibleDelegate == null)
                return Value;
            return VisibleDelegate(Value);
        }



        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
