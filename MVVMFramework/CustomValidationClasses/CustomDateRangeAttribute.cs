using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.Exceptions;
using System;
namespace CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses
{
    public enum EnumLessOrGreater
    {
        LesserThan = 1,
        LesserOrEqual = 2,
        GreaterThan = 3,
        GreaterOrEqual = 4
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class CustomDateRangeAttribute : ValidationAttribute
    {
        public string DateProperty { get; } // i think needs the string
        public EnumLessOrGreater LessOrGreater { get; }
        public bool Required { get; set; } = false;
        public CustomDateRangeAttribute(string dateProperty, EnumLessOrGreater lessOrGreater)
        {
            DateProperty = dateProperty;
            LessOrGreater = lessOrGreater;
        }
        public bool IsValid(object value, object objectSource)
        {
            bool rets;
            var thisStr = value.ToString();
            rets = thisStr.IsValidDate(Required);
            if (rets == false)
                return false;
            DateTime? compareDate;
            compareDate = DateProperty.GetValueFromProperty<DateTime?>(objectSource);
            rets = DateTime.TryParse(thisStr, out DateTime OldDate);
            if (rets == false)
                return true;
            if (LessOrGreater == EnumLessOrGreater.GreaterOrEqual || LessOrGreater == EnumLessOrGreater.LesserOrEqual)
            {
                // try to do without time part
                if (OldDate == compareDate!.Value)
                    return true;
            }
            if (LessOrGreater == EnumLessOrGreater.GreaterOrEqual || LessOrGreater == EnumLessOrGreater.GreaterThan)
            {
                if (compareDate >= OldDate)
                    return false;
                return true;
            }
            if (LessOrGreater == EnumLessOrGreater.LesserOrEqual || LessOrGreater == EnumLessOrGreater.LesserThan)
            {
                if (compareDate <= OldDate)
                    return false;
            }
            return true;
        }
        public override bool IsValid(object currentValue)
        {

            


            throw new BasicBlankException("This one requires the context.  If this is required, rethink");
        }
    }
}