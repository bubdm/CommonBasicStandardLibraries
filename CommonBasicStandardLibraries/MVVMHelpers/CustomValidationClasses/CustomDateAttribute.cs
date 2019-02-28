using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; //looks like i was not able to do away from the reference to the annotations.
using System.Text;

namespace CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomValidDateAttribute : ValidationAttribute// i don't have to make not overridable because the required one allowed to be overrided
    {
        // maybe i have to allow multiple.  i guess this means more than one property in the class.
        public bool Required { get; set; } = false;


        protected bool GetMyBaseValid(object value)
        {
            return base.IsValid(value);
        }

        // the example showed that since its used for validation, should inherit from that
        public override bool IsValid(object value)
        {
            // my custom logic
            // if it gets to this point, has to be a date.  its used with a converter
            // Return False 'for now


            var ThisStr = (string)value;
            return ThisStr.IsValidDate(Required);
        }
    }
}
