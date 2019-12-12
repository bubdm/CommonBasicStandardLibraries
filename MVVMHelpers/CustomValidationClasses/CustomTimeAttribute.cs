using CommonBasicStandardLibraries.Attributes;
using System.Linq;
namespace CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses
{
    public class CustomTimeAttribute : ValidationAttribute
    {
        public enum EnumTimeFormat
        {
            None = 0,
            Minutes = 1,
            Hours = 2,
            Days = 3,
            Seconds = 4 // this is used for lots of things now.
        }
        private bool IsSingleNumberValid(string newStr, EnumTimeFormat format, bool isFirst)
        {
            if (format == EnumTimeFormat.None)
            {
                ErrorMessage = "Must choose a format";
                return false;
            }
            if (format == EnumTimeFormat.Seconds)
            {
                ErrorMessage = "Seconds are not supported.   Its only used so reminders can use seconds for automation only";
                return false;
            }
            bool rets;
            rets = int.TryParse(newStr, out int newNum);
            if (rets == false)
            {
                ErrorMessage = "Must enter a valid number for " + format.ToString();
                return false;
            }
            if (format == EnumTimeFormat.Minutes && isFirst == true && newNum == 0)
            {
                ErrorMessage = "If you are entering minutes, must enter greater than 0";
                return false;
            }
            if (newNum < 0)
            {
                ErrorMessage = "Cannot enter a non positive number";
                return false;
            }
            if (format == EnumTimeFormat.Minutes)
            {
                // if its one hour, can still do 60 because its 0 minutes in this case.
                if (newNum > 59)
                {
                    ErrorMessage = "Minutes must be between 0 and 59.  If you need more, try including a : so would be 1:1 for one hour and one minute";
                    return false;
                }
                return true;
            }
            if (format == EnumTimeFormat.Hours)
            {
                if (newNum > 23)
                {
                    ErrorMessage = "Hours must be between 0 and 23.  If you need more, try including another : to show days";
                    return false;
                }
            }
            return true; // can reach this point because doing days.
        }
        public bool IsValid(string value, EnumTimeFormat format)
        {
            var tempList = value.Split(':').ToList();
            if (tempList.Count == 0)
            {
                ErrorMessage = "Maybe its blank.  Its not valid though";
                return false;
            }
            if (format != EnumTimeFormat.None)
            {
                if (tempList.Count != 1)
                {
                    ErrorMessage = "If entering a specialized format, must be a single item.  Otherwise, needed to specify none";
                    return false;
                }
                return IsSingleNumberValid(value, format, true);
            }
            if (tempList.Count == 1)
                return IsSingleNumberValid(value, EnumTimeFormat.Minutes, true);
            if (tempList.Count > 3)
            {
                ErrorMessage = "Can seperate the maximum by minutes/hours/days";
                return false;
            }
            bool rets;
            tempList.Reverse();
            int x;
            x = 0;
            foreach (var thisItem in tempList)
            {
                x += 1;
                rets = IsSingleNumberValid(thisItem, (EnumTimeFormat)x, false); //this is how i had to handle casting integers to enums.
                if (rets == false)
                    return false;
            }
            return true;
        }
        public override bool IsValid(object value)
        {
            ErrorMessage = "Must use the new format since a person may want just days/hours/days";
            return false;
        }
    }
}