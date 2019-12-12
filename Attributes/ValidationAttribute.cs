using System;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] //will make this only on property
    public abstract class ValidationAttribute : Attribute
    {
        public ValidationAttribute() { }
        public ValidationAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public abstract bool IsValid(object CurrentValue);
        public string ErrorMessage { get; set; } = "";
    }
}