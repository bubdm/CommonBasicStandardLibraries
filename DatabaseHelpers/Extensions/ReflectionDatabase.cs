using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.Attributes;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Linq;
using System.Reflection;
namespace CommonBasicStandardLibraries.DatabaseHelpers.Extensions
{
    public static class ReflectionDatabase
    {
        public static string GetJoiner<E, D>()
        {
            Type firstType = typeof(E);
            string secondName;
            Type secondType = typeof(D);
            secondName = secondType.Name;
            CustomBasicList<PropertyInfo> thisList = firstType.GetPropertiesWithAttribute<ForeignKeyAttribute>().ToCustomBasicList();
            CustomBasicList<PropertyInfo> newList = thisList.Where(items =>
            {
                ForeignKeyAttribute thisKey = items.GetCustomAttribute<ForeignKeyAttribute>();
                if (thisKey.ClassName == secondName)
                    return true;
                return false;
            }).ToCustomBasicList();
            if (newList.Count == 0)

                throw new BasicBlankException($"No Key Found Linking {secondName}");
            if (newList.Count > 1)
                throw new BasicBlankException($"Duplicate Key Found Linking {secondName}");
            PropertyInfo thisProp = newList.Single();
            ColumnAttribute thisCol = thisProp.GetCustomAttribute<ColumnAttribute>();
            if (thisCol == null)
                return thisProp.Name;
            return thisCol.ColumnName;
        }
        public static bool CanMapToDatabase(this PropertyInfo property)
        {
            //var attribute = property.GetAttribute<NotMappedAttribute>();
            bool rets = property.HasAttribute<NotMappedAttribute>();
            if (rets == true)
                return false; //because its not mapped.
            return property.IsSimpleType();
        }
        public static string GetTableName<E>() //decided to put here even though you start with no parameters.
        {
            Type thisType = typeof(E);
            return thisType.GetTableName();
        }
        public static string GetTableName(this Type thisType)
        {
            TableAttribute thisTable = thisType.GetCustomAttribute<TableAttribute>();
            if (thisTable == null)
                return thisType.Name;
            return thisTable.TableName;
        }
        public static bool HasJoiner<E>(string className)
        {
            Type thisType = typeof(E);
            return thisType.HasJoiner(className);
        }
        public static bool HasJoiner(this Type thisType, string className)
        {
            CustomBasicList<ForeignKeyAttribute> possibleList = thisType.GetCustomAttributes<ForeignKeyAttribute>()!;
            return possibleList.Any(items => items.ClassName == className);
        }
        public static string GetColumnName(this PropertyInfo ThisProperty)
        {
            ColumnAttribute thisColumn = ThisProperty.GetAttribute<ColumnAttribute>()!;
            if (thisColumn != null)
                return thisColumn.ColumnName;
            return ThisProperty.Name; //i will have to see what i use the interface ones for.
        }
        public static string GetInterfaceName(this PropertyInfo thisProperty)
        {
            InterfaceAttribute thisInterface = thisProperty.GetAttribute<InterfaceAttribute>()!;
            if (thisInterface != null)
                return thisInterface.InterfaceName;
            return ""; //sometimes this is not necessary
        }
        public static CustomBasicList<PropertyInfo> GetJoinedColumns(this Type thisType)
        {
            return thisType.GetPropertiesWithAttribute<PrimaryJoinedDataAttribute>().ToCustomBasicList();
        }
        public static bool IsAutoIncremented<E>()
        {
            Type thisType = typeof(E);
            return thisType.IsAutoIncremented();
        }
        public static bool IsAutoIncremented(this Type thisType)
        {
            return !thisType.ClassContainsAttribute<NoIncrementAttribute>();
        }
    }
}