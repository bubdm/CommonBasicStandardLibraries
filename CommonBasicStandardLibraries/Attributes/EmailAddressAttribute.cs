using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : DataTypeAttribute
    {
        public EmailAddressAttribute(string ErrorMessage) : base(ErrorMessage, EnumDataType.EmailAddress)
        {
        }

        public override bool IsValid(object CurrentValue)
        {
            if (CurrentValue == null)
            {
                return true;
            }

            if (!(CurrentValue is string valueAsString))
            {
                return false;
            }

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            bool found = false;
            for (int i = 0; i < valueAsString.Length; i++)
            {
                if (valueAsString[i] == '@')
                {
                    if (found || i == 0 || i == valueAsString.Length - 1)
                    {
                        return false;
                    }
                    found = true;
                }
            }

            return found;
        }
    }
}
