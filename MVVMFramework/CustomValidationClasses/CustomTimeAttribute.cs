using CommonBasicStandardLibraries.Attributes;
using System;
namespace CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses
{
    public class CustomTimeAttribute : ValidationAttribute
    {
        private readonly bool _required;



        public CustomTimeAttribute(bool required)
        {
            _required = required;
        }

        
        public override bool IsValid(object value)
        {

            if (value == null)
            {
                if (_required)
                {
                    ErrorMessage = "Required field for time format";
                    return false;
                }
                return true;
            }
            string thisStr = value.ToString();
            if (thisStr == "")
            {
                if (_required)
                {
                    ErrorMessage = "Required field for time format";
                    return false;
                }
                return true;
            }

            bool rets;
            rets = DateTime.TryParse(thisStr, out DateTime _);
            if (rets == false)
            {
                ErrorMessage = "Must in time format like 8:00 am";
                return false;
            }
            return true;
        }
    }
}