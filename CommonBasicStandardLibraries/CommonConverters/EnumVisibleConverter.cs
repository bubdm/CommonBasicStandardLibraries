using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
        public ConvertTranslation VisibleDelegate;

        public object Convert(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            bool rets;
            E ToSend;
            if (Parameter == null || Parameter is E == false)
                ToSend = default;
            else
                ToSend = (E) Parameter;
            rets = Convert((E)value, ToSend);
            return FinalResults(rets);
        }

        private object FinalResults(bool Value)
        {
            if (VisibleDelegate == null)
                return Value;
            return VisibleDelegate(Value);
        } 

        protected abstract bool Convert(E EnumSent, E Parameter); //the derived classes must implement this function which returns whether the ui will be visible or not.


        public object ConvertBack(object value, Type TargetType, object Parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); //this should never convert back
        }
    }
}