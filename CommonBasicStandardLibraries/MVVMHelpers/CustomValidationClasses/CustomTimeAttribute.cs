using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommonBasicStandardLibraries.Attributes;
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

        private bool IsSingleNumberValid(string NewStr, EnumTimeFormat Format, bool IsFirst)
        {
            if (Format == EnumTimeFormat.None)
            {
                ErrorMessage = "Must choose a format";
                return false;
            }
            // decided to do seconds for testing.  also needs the flexibility for seconds anyways.
            if (Format == EnumTimeFormat.Seconds)
            {
                ErrorMessage = "Seconds are not supported.   Its only used so reminders can use seconds for automation only";
                return false;
            }
            bool rets;
            //int NewNum;
            rets = int.TryParse(NewStr, out int NewNum);
            if (rets == false)
            {
                ErrorMessage = "Must enter a valid number for " + Format.ToString();
                return false;
            }
            if (Format == EnumTimeFormat.Minutes && IsFirst == true && NewNum == 0)
            {
                ErrorMessage = "If you are entering minutes, must enter greater than 0";
                return false;
            }


            if (NewNum < 0)
            {
                ErrorMessage = "Cannot enter a non positive number";
                return false;
            }
            // If NewNum <= 0 Then
            // If IsFirst = True Then
            // ErrorMessage = "If entering minutes, must be greater than 0"
            // Else
            // ErrorMessage = "Must enter " & Format.ToString & " as greater than 0"
            // End If
            // Return False
            // End If
            // you can do first even for hours/days because may want one hour

            // If IsFirst = True AndAlso Format <> EnumTimeFormat.Minutes Then
            // ErrorMessage = "There was a bug in using the validation.  Because should never use IsFirst if its not minutes because minutes are the only case where numbers can be entered.  In other cases, can't be IsFirst"
            // Return False
            // End If
            if (Format == EnumTimeFormat.Minutes)
            {
                // if its one hour, can still do 60 because its 0 minutes in this case.
                if (NewNum > 59)
                {
                    ErrorMessage = "Minutes must be between 0 and 59.  If you need more, try including a : so would be 1:1 for one hour and one minute";
                    return false;
                }
                return true;
            }
            if (Format == EnumTimeFormat.Hours)
            {
                if (NewNum > 23)
                {
                    ErrorMessage = "Hours must be between 0 and 23.  If you need more, try including another : to show days";
                    return false;
                }
            }
            return true; // can reach this point because doing days.
        }

        public bool IsValid(string value, EnumTimeFormat Format)
        {
            var TempList = value.Split(':').ToList();
            if (TempList.Count == 0)
            {
                ErrorMessage = "Maybe its blank.  Its not valid though";
                return false;
            }
            if (Format != EnumTimeFormat.None)
            {
                if (TempList.Count != 1)
                {
                    ErrorMessage = "If entering a specialized format, must be a single item.  Otherwise, needed to specify none";
                    return false;
                }
                return IsSingleNumberValid(value, Format, true);
            }
            if (TempList.Count == 1)
                return IsSingleNumberValid(value, EnumTimeFormat.Minutes, true);
            if (TempList.Count > 3)
            {
                ErrorMessage = "Can seperate the maximum by minutes/hours/days";
                return false;
            }
            bool rets;
            TempList.Reverse();
            int x;
            x = 0;
            foreach (var ThisItem in TempList)
            {
                x += 1;
                //y = x;
                rets = IsSingleNumberValid(ThisItem, (EnumTimeFormat)x, false); //this is how i had to handle casting integers to enums.
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
