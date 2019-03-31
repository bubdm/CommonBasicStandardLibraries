using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] //will make this only on property
    public abstract class ValidationAttribute : Attribute
    {

        public ValidationAttribute()
        {

        }
        public ValidationAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        public abstract bool IsValid(object CurrentValue);

        public string ErrorMessage { get; set; }

    }

}
