using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Reflections
    {
        public static TAttribute? GetAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : class //decided to risk doing this way.
        {
            var attributeName = typeof(TAttribute).Name;

            return propertyInfo.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType()!.Name == attributeName) as TAttribute;
        }
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo)
        {
            var attributeName = typeof(TAttribute).Name;
            return propertyInfo.GetCustomAttributes(true).Any(attr => attr.GetType().Name == attributeName);
        }
        public static bool IsSimpleType(this PropertyInfo property)
        {
            var simpleTypesList = new List<Type>
            {
                //typeof(byte),
                //typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(bool),
                typeof(string),
                typeof(char),
                typeof(Guid),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(short?),
                typeof(ushort?),
                typeof(int?),
                typeof(uint?),
                typeof(long?),
                typeof(ulong?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?),
                typeof(bool?),
                typeof(char?),
                typeof(Guid?),
                typeof(DateTime?),
                typeof(DateTimeOffset?)
            };
            if (property.PropertyType.IsEnum == true)
                return true;
            if (property.PropertyType.IsNullableEnum() == true)
                return true;
            return simpleTypesList.Contains(property.PropertyType);
        }
        private static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
        public static bool IsBool(this PropertyInfo property)
        {
            var simpleTypesList = new List<Type>
            {
                typeof(bool),
                typeof(bool?),
            };
            //enums are not booleans.
            return simpleTypesList.Contains(property.PropertyType);
        }
        public static bool IsInt(this PropertyInfo property)
        {
            var simpleTypesList = new List<Type>
            {
                typeof(int),
                typeof(int?),
            };
            if (property.PropertyType.IsEnum == true)
                return true;
            if (property.PropertyType.IsNullableEnum() == true)
                return true; //i think

            return simpleTypesList.Contains(property.PropertyType);
        }
        public static TAttribute? GetAttribute<TAttribute>(this Type type) where TAttribute : class
        {
            var attributeName = typeof(TAttribute).Name;
            return type.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == attributeName) as TAttribute;
        }
        public static bool HasAttribute<TAttribute>(this Type type)
        {
            var attributeName = typeof(TAttribute).Name;
            return type.GetCustomAttributes(true).Any(attr => attr.GetType().Name == attributeName);
        }
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttribute>(this Type type)
        {
            return type.GetProperties().Where(p => p.HasAttribute<TAttribute>());
        }
        public static bool ClassContainsAttribute<TAttribute>(this Type type)
        {
            return type.GetProperties().Any(Items => Items.HasAttribute<TAttribute>());
        }
        public static IEnumerable<PropertyInfo> GetProperties(this Type type, Func<PropertyInfo, bool> predicate)
        {
            return type.GetProperties().Where(predicate);
        }
        public static CustomBasicList<TAttribute>? GetCustomAttributes<TAttribute>(this Type type) where TAttribute : class
        {
            return type.GetProperties().Where(Items => Items.HasAttribute<TAttribute>() == true).Select(News => News.GetAttribute<TAttribute>()).ToCustomBasicList()!;
        }
    }
}