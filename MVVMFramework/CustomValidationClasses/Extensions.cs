using System;
using System.Reflection;
namespace CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses
{
    internal static class Extensions
    {
        public static T GetValueFromProperty<T>(this string propertyName, object instance)
        {
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            object propertyValue = property.GetValue(instance);
            return (T)propertyValue;
        }
        public static bool IsValidDate(this string thisStr, bool required)
        {
            if (string.IsNullOrWhiteSpace(thisStr) == true && required == true)
                return false;
            bool rets;
            rets = DateTime.TryParse(thisStr, out DateTime thisDate);
            if (rets == false)
                return true;
            if (required == true)
            {
                if (thisDate.Year == 1)
                    return false;
            }
            if (thisDate.Year == 1000 && thisDate.Day == 1 && thisDate.Month == 1)
                return false;
            if (thisDate.Year == 1)
                return false;// because its not really valid
            return true;
        }
    }
}