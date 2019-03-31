using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Text;
using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.Exceptions;
namespace CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses
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


        public CustomDateRangeAttribute(string DateProperty, EnumLessOrGreater LessOrGreater)
        {
            this.DateProperty = DateProperty;
            this.LessOrGreater = LessOrGreater;
        }

        public bool IsValid(object value, object ObjectSource)
        {
            bool rets;
            var ThisStr = value.ToString();
            rets = ThisStr.IsValidDate(Required);
            if (rets == false)
                return false;

            DateTime? CompareDate;
            CompareDate = DateProperty.GetValueFromProperty<DateTime?>(ObjectSource);

            rets = DateTime.TryParse(ThisStr, out DateTime OldDate);

            if (rets == false)
                return true;



            if (LessOrGreater == EnumLessOrGreater.GreaterOrEqual || LessOrGreater == EnumLessOrGreater.LesserOrEqual)
            {
                // try to do without time part
                if (OldDate == CompareDate.Value)
                    return true;
            }
            if (LessOrGreater == EnumLessOrGreater.GreaterOrEqual || LessOrGreater == EnumLessOrGreater.GreaterThan)
            {
                // Return False
                if (CompareDate >= OldDate)
                    return false;
                return true;
            }
            if (LessOrGreater == EnumLessOrGreater.LesserOrEqual || LessOrGreater == EnumLessOrGreater.LesserThan)
            {
                if (CompareDate <= OldDate)
                    return false;
            }
            return true;
        }

        public override bool IsValid(object CurrentValue)
        {
            throw new BasicBlankException("This one requires the context.  If this is required, rethink");
        }
    }
}
