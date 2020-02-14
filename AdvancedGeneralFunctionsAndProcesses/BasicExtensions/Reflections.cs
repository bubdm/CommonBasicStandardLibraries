using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static bool IsSimpleType(this Type type)
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
            if (type.IsEnum == true)
                return true;
            //if (property.PropertyType.IsNullableEnum() == true)
            //    return true;
            return simpleTypesList.Contains(type);
        }

        public static bool IsDateType(this PropertyInfo property)
        {
            //if (bindableProperty == UIElement.VisibilityProperty && typeof(bool).IsAssignableFrom(property.PropertyType))
            //    binding.Converter = _booleanToVisibilityConverter;
            //if (bindableProperty == CustomTextbox.TextProperty && (typeof(DateTime).IsAssignableFrom(property.PropertyType) || typeof(DateTime?).IsAssignableFrom(property.PropertyType)))
            //{
            //    binding.Converter = new DateConverter(); //i think will always use my custom date converter if using custom textbox and its assignable from date and time.
            //}
            if (typeof(DateTime).IsAssignableFrom(property.PropertyType))
            {
                return true;
            }
            if (typeof(DateTime?).IsAssignableFrom(property.PropertyType))
            {
                return true;
            }
            return false;
        }

        public static bool IsIntegerType(this PropertyInfo property)
        {
            if (typeof(int).IsAssignableFrom(property.PropertyType))
            {
                return true;
            }
            if (typeof(int?).IsAssignableFrom(property.PropertyType))
            {
                return true;
            }
            return false;
        }
    

        public static bool IsSimpleType(this PropertyInfo property)
        {
            return property.PropertyType.IsSimpleType();
            //var simpleTypesList = new List<Type>
            //{
            //    //typeof(byte),
            //    //typeof(sbyte),
            //    typeof(short),
            //    typeof(ushort),
            //    typeof(int),
            //    typeof(uint),
            //    typeof(long),
            //    typeof(ulong),
            //    typeof(float),
            //    typeof(double),
            //    typeof(decimal),
            //    typeof(bool),
            //    typeof(string),
            //    typeof(char),
            //    typeof(Guid),
            //    typeof(DateTime),
            //    typeof(DateTimeOffset),
            //    typeof(short?),
            //    typeof(ushort?),
            //    typeof(int?),
            //    typeof(uint?),
            //    typeof(long?),
            //    typeof(ulong?),
            //    typeof(float?),
            //    typeof(double?),
            //    typeof(decimal?),
            //    typeof(bool?),
            //    typeof(char?),
            //    typeof(Guid?),
            //    typeof(DateTime?),
            //    typeof(DateTimeOffset?)
            //};
            //if (property.PropertyType.IsEnum == true)
            //    return true;
            //if (property.PropertyType.IsNullableEnum() == true)
            //    return true;
            //return simpleTypesList.Contains(property.PropertyType);
        }
        internal static bool IsNullableEnum(this Type t)
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
        public static bool IsString(this PropertyInfo property)
        {
            var newTypes = new List<Type> { typeof(string) };
            return newTypes.Contains(property.PropertyType);
        }
        public static bool IsIntOrEnum(this PropertyInfo property)
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
        public static bool IsEnumerable(this PropertyInfo property)
        {
            if (property.IsString())
            {
                return false;
            }
            bool output = false;

            Type[] firstList = property.PropertyType.GetInterfaces();

            foreach (var i in firstList)
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                {
                    output = true;
                }
            }
            return output;
        }
        private static bool CanMapProperty(this PropertyInfo property)
        {
            bool output = false;
            if (property.IsEnumerable())
            {
                output = true;
            }
            if (property.IsSimpleType())
            {
                output = true;
            }
            return output;
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

        public static MethodInfo GetPrivateMethod(this ObservableObject payLoad, string name)
        {
            Type type = payLoad.GetType();
            MethodInfo output = type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);

            if (output != null)
            {
                return output;
            }
            output = type.GetMethod(name);
            if (output != null)
            {
                return output;
            }
            type = type.BaseType;
            output = type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (output == null)
            {
                throw new BasicBlankException($"Method with the name of {name} was not found  Type was {type.Name}");
            }
            return output;
        }


        
        public static CustomBasicList<PropertyInfo> GetMappableProperties(this Type type)
        {
            return type.GetProperties(x => x.CanMapProperty()).ToCustomBasicList();
        }
        public static CustomBasicList<TAttribute>? GetCustomAttributes<TAttribute>(this Type type) where TAttribute : class
        {
            return type.GetProperties().Where(Items => Items.HasAttribute<TAttribute>() == true).Select(News => News.GetAttribute<TAttribute>()).ToCustomBasicList()!;
        }
    }
}