using System;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : DataTypeAttribute
    {
        public EmailAddressAttribute(string errorMessage) : base(errorMessage, EnumDataType.EmailAddress) { }

        public override bool IsValid(object currentValue)
        {
            if (currentValue == null)
            {
                return true;
            }
            if (!(currentValue is string valueAsString))
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