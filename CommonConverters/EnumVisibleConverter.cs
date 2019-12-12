using System;
using System.Globalization;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.CommonConverters
{
    /// <summary>
    /// This is used for converters where you have enums and depending on enum values, the ui has to be made visible or invisible.
    /// Not only does this cast to the proper type because of generics but also for wpf, if you use have a delegate that converts to the wpf version, then even
    /// wpf can show whether its visible or not and does not require any dependencies on wpf.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public abstract class EnumVisibleConverter<E> : IConverterCP where E : Enum
    {
        public VisibleTranslation? VisibleDelegate; //can for sure be null.

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool rets;
            E toSend;
            if (parameter == null || parameter is E == false)
                toSend = default!;
            else
                toSend = (E)parameter;
            rets = Convert((E)value, toSend);
            return FinalResults(rets);
        }
        private object FinalResults(bool value)
        {
            if (VisibleDelegate == null)
                return value;
            return VisibleDelegate(value);
        }
        protected abstract bool Convert(E enumSent, E parameter); //the derived classes must implement this function which returns whether the ui will be visible or not.
        protected bool DefaultEnumVisible(E enumSent, E parameter)
        {
            if (enumSent.Equals(parameter))
                return true;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); //this should never convert back
        }
    }
}