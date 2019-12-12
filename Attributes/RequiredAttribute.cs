using System;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute(string errorMessage) : base(errorMessage) { }

        public RequiredAttribute() { }

        public override bool IsValid(object currentValue)
        {
            if (currentValue == null)
            {
                return false;
            }
            // only check string length if empty strings are not allowed
            return AllowEmptyStrings || !(currentValue is string stringValue) || stringValue.Trim().Length != 0;
        }
        /// <summary>
        ///     Gets or sets a flag indicating whether the attribute should allow empty strings.
        /// </summary>
        public bool AllowEmptyStrings { get; set; }
    }
}