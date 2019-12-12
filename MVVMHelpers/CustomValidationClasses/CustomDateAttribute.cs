using CommonBasicStandardLibraries.Attributes;
using System;
namespace CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomValidDateAttribute : ValidationAttribute// i don't have to make not overridable because the required one allowed to be overrided
    {
        // maybe i have to allow multiple.  i guess this means more than one property in the class.
        public bool Required { get; set; } = false;

        public override bool IsValid(object value)
        {
            var thisStr = (string)value;
            return thisStr.IsValidDate(Required);
        }
    }
}