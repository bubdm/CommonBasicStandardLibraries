using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttribute
    {

        public RequiredAttribute(string ErrorMessage) : base(ErrorMessage) { }
      
        public RequiredAttribute() { }

        public override bool IsValid(object CurrentValue)
        {
            if (CurrentValue == null)
            {
                return false;
            }

            // only check string length if empty strings are not allowed
            return AllowEmptyStrings || !(CurrentValue is string stringValue) || stringValue.Trim().Length != 0;
        }

        /// <summary>
        ///     Gets or sets a flag indicating whether the attribute should allow empty strings.
        /// </summary>
        public bool AllowEmptyStrings { get; set; }



    }
}
