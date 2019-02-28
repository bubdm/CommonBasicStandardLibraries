using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
namespace CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses
{
    internal static class Extensions
    {
        public static T GetValueFromProperty<T>(this string PropertyName, object instance)
        {
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(PropertyName);
            object propertyValue = property.GetValue(instance);
            return (T)propertyValue;
        }

        public static bool IsValidDate(this string ThisStr, bool Required)
        {
            if (string.IsNullOrWhiteSpace(ThisStr) == true && Required == true)
                return false;// because its required
                             // Console.WriteLine("Start, " & Required)
                             // Dim ThisDate = DirectCast(ThisStr, Date)
                             //DateTime ThisDate;
            bool rets;

            rets = DateTime.TryParse(ThisStr, out DateTime ThisDate);
            if (rets == false)
                return true;



            if (Required == true)
            {
                if (ThisDate.Year == 1)
                    return false;
            }
            if (ThisDate.Year == 1000 && ThisDate.Day == 1 && ThisDate.Month == 1)
                return false;
            if (ThisDate.Year == 1)
                return false;// because its not really valid
            return true;
        }

        
    }
}
