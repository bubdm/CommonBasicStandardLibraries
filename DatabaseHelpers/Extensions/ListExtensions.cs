using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses;
using System.Collections.Generic;
using System.Linq;
using static CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses.SortInfo;
namespace CommonBasicStandardLibraries.DatabaseHelpers.Extensions
{
    public static class ListExtensions
    {
        public static string GetNewValue(this Dictionary<string, int> thisDict, string parameter)
        {
            bool rets;
            rets = thisDict.TryGetValue(parameter, out int Value);
            if (rets == true)
            {
                Value++;
                thisDict[parameter] = Value;
                return $"{parameter}{Value}";
            }
            thisDict.Add(parameter, 1);
            return $"{parameter}1";
        }
        public static CustomBasicList<int> GetIDList<E>(this CustomBasicList<E> thisList) where E : ISimpleDapperEntity
        {
            return thisList.Select(Items => Items.ID).ToCustomBasicList();
        }
        public static void InitalizeAll<E>(this CustomBasicList<E> thisList) where E : IUpdatableEntity
        {
            thisList.ForEach(Items => Items.Initialize());
        }
        public static CustomBasicList<SortInfo> Append(this CustomBasicList<SortInfo> sortList, string property)
        {
            return sortList.Append(property, EnumOrderBy.Ascending);
        }
        public static CustomBasicList<SortInfo> Append(this CustomBasicList<SortInfo> sortList, string property, EnumOrderBy orderBy)
        {
            sortList.Add(new SortInfo() { Property = property, OrderBy = orderBy });
            return sortList;
        }
        public static CustomBasicList<UpdateFieldInfo> Append(this CustomBasicList<UpdateFieldInfo> tempList, string thisProperty) //if it needs to be something else. rethink
        {
            tempList.Add(new UpdateFieldInfo(thisProperty));
            return tempList;
        }

        public static CustomBasicList<UpdateFieldInfo> Append(this CustomBasicList<UpdateFieldInfo> tempList, CustomBasicList<string> propList)
        {
            propList.ForEach(items =>
            {
                tempList.Add(new UpdateFieldInfo(items));
            });
            return tempList;
        }
        public static CustomBasicList<UpdateEntity> Append(this CustomBasicList<UpdateEntity> tempList, string thisProperty, object value) //if it needs to be something else. rethink
        {
            tempList.Add(new UpdateEntity(thisProperty, value));
            return tempList;
        }
    }
}
