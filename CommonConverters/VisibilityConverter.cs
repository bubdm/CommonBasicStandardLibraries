using System;
using System.Globalization;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.CommonConverters
{
    public abstract class VisibilityConverter : IConverterCP
    {
        public VisibleTranslation? VisibleDelegate;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FinalResults(true);
            bool rets = bool.TryParse(value.ToString(), out bool fins);
            if (rets == false)
                return FinalResults(true);
            return FinalResults(fins);
        }
        private object FinalResults(bool value)
        {
            if (VisibleDelegate == null)
                return value;
            return VisibleDelegate(value);
        }
        public object ConvertBack(object value, Type targetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}