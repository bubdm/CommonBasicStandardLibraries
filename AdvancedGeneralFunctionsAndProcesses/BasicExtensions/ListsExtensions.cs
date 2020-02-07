using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ListsExtensions
    {

        public static string Join(this CustomBasicList<string> list, string delimiter)
        {
            return string.Join(delimiter, list);
        }

        public static void Add<T>(this Dictionary<int, T> thisDict, T thisItem)
        {
            thisDict.Add(thisDict.Count + 1, thisItem); //this is used in cases where we do a dictionary just for the purpose of one based items
        }
        public static T GetRandomItem<T>(this Dictionary<int, T> thisList)
        {
            return thisList.Values.ToCustomBasicList().GetRandomItem();
        }
        //maybe i don't need this anymore.  if the .net standard 2.1 has it, then no need anymore for my own.

        ////decided to put thelistextensions as now part of the main extension namespace
        //public static HashSet<T> ToHashSet<T>(this IEnumerable<T> TempList)
        //{
        //    return new HashSet<T>(TempList);
        //}
        public static CustomBasicList<T> ToCustomBasicList<T>(this IEnumerable<T> tempList)
        {
            return new CustomBasicList<T>(tempList);
        }
        public static CustomBasicList<T> ToCastedList<T>(this IEnumerable<object> tempList) //in this case, you get another list.
        {
            return tempList.Cast<T>().ToCustomBasicList();
        }
        public static CustomBasicCollection<T> ToCustomBasicCollection<T>(this IEnumerable<T> tempList)
        {
            return new CustomBasicCollection<T>(tempList);
        }
        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> thisDict, TValue thisValue)
        {

            if (thisDict == null)
                throw new ArgumentNullException(nameof(thisDict));

            foreach (KeyValuePair<TKey, TValue> thisPair in thisDict)
            {
                if (thisValue!.Equals(thisPair.Value) == true || ReferenceEquals(thisPair.Value, thisValue) == true)
                    return thisPair.Key;
            }
            throw new BasicBlankException($"The Value Of {thisValue} Was Not Found In The Dictionary");
        }
        public static CustomBasicList<string> GetStringListFromGivenProperty<T>(this ICustomBasicList<T> thisList, string propertyName)
        {
            CustomBasicList<string> output = new CustomBasicList<string>();
            thisList.ForEach(thisItem =>
            {
                output.Add(thisItem.GetStringValue(propertyName));
            });
            return output;
        }
        public static CustomBasicList<T> CreateCustomListFromIntegers<T>(CustomBasicList<int> thisList, string propertyName)
            where T : new()
        {
            CustomBasicList<T> output = new CustomBasicList<T>();
            thisList.ForEach(thisItem =>
            {
                T newItem = new T();
                var thisProp = thisItem.GetType().GetRuntimeProperty(propertyName);
                thisProp.SetValue(newItem, thisItem);
                output.Add(newItem);
            });
            return output;
        }
        public static CustomBasicList<T> CreateCustomListFromString<T>(CustomBasicList<string> thisList, string propertyName)
            where T : new()
        {
            CustomBasicList<T> output = new CustomBasicList<T>();
            thisList.ForEach(thisItem =>
            {
                T newItem = new T();
                var thisProp = thisItem.GetType().GetRuntimeProperty(propertyName);
                thisProp.SetValue(newItem, thisItem);
                output.Add(newItem);
            });
            return output;
        }
        private static string GetStringValue<T>(this T thisItem, string propertyName)
        {
            var thisProp = thisItem!.GetType().GetRuntimeProperty(propertyName);
            return (string)thisProp.GetValue(thisItem); //try this way.
        }
        public static void PopulateBlankList(this ICustomBasicList<string> thisList, int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                thisList.Add("");
            }
        }
        public static CustomBasicList<string> CastIntegerListToStringList(this ICustomBasicList<int> thisList)
        {
            CustomBasicList<string> newList = new CustomBasicList<string>();
            thisList.ForEach(x => newList.Add(x.ToString()));
            return newList;
        }
        public static void IncrementIntegers<T>(this ICustomBasicList<T> thisList, UpdateFunct<T> selector, int startAt = 1)
        {
            thisList.ForEach(thisItem =>
            {
                selector.Invoke(thisItem, startAt);
                startAt++;
            }
            );
        }
        public static async Task ReconcileStrings<T>(this ICustomBasicList<string> sentList, ICustomBasicList<T> savedList, Func<T, string> match, Func<string, Task<T>> result)
        {
            CustomBasicList<string> tempList = new CustomBasicList<string>();
            savedList.ForEach(items => tempList.Add(match(items)));
            CustomBasicList<T> removeList = new CustomBasicList<T>();
            CustomBasicList<T> addList = new CustomBasicList<T>();
            tempList.ForEach(items =>
            {
                if (sentList.Contains(items) == false)
                    removeList.Add(savedList[tempList.IndexOf(items)]);
            });

            await sentList.ForEachAsync(async items =>
            {
                if (tempList.Contains(items) == false)
                    addList.Add(await result(items));
            });
            savedList.RemoveGivenList(removeList, System.Collections.Specialized.NotifyCollectionChangedAction.Remove);
            savedList.AddRange(addList);
        }
        public static CustomBasicList<ConditionActionPair<T>> Append<T>(this CustomBasicList<ConditionActionPair<T>> tempList, Predicate<T> match, Action<T, string> action, string value = "") //if it needs to be something else. rethink
        {
            ConditionActionPair<T> ThisC = new ConditionActionPair<T>(match, action, value);
            tempList.Add(ThisC);
            return tempList;
        }
        public static void WriteString<T>(this ICustomBasicList<T> thisList)
        {
            thisList.ForEach(items => Console.WriteLine(items!.ToString()));
        }
        public static bool HasDuplicates<TSource, TKey>(this ICustomBasicList<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)) == false)
                    return true;
            }
            return false;
        }
        public static bool HasOnlyOne<TSource, TKey>(this ICustomBasicList<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            if (source.Count == 0)
                return false; //because there are none.
            foreach (var item in source)
            {
                seenKeys.Add(keySelector(item));
                if (seenKeys.Count > 1)
                    return false;
            }
            return true;
        }
        public static IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
        }
        public static IOrderedEnumerable<IGrouping<TKey, TSource>> GroupOrderAscending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).OrderBy(Items => Items.Count());
        }
        public static int MaximumDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source.Count() == 0)
                throw new BasicBlankException("There has to be at least one item.  If I am wrong, rethink");
            var FirstList = source.GroupBy(keySelector).OrderByDescending(Items => Items.Count());
            return FirstList.First().Count();
        }
        public static int MaximumDuplicates<TSource>(this IEnumerable<TSource> source)
        {
            if (source.Count() == 0)
                throw new BasicBlankException("There has to be at least one item.  If I am wrong, rethink");
            var FirstList = source.GroupBy(Items => Items).OrderByDescending(Items => Items.Count());
            return FirstList.First().Count();
        }
        public static CustomBasicList<TSource> GetDuplicates<TSource, TKey>(this ICustomBasicList<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            CustomBasicList<TSource> output = new CustomBasicList<TSource>();
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)) == false)
                    output.Add(item);
            }
            return output;
        }
        public static bool DoesReconcile<TSource, TKey>(this IEnumerable<TSource> source, IEnumerable<TSource> other, Func<TSource, TKey> keySelector)
        {
            if (source.Count() != other.Count())
                return false; //because not even the same count.
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                seenKeys.Add(keySelector(item));
            }
            foreach (var item in other)
            {
                if (seenKeys.Add(keySelector(item)))
                    return false;
            }
            return true; //may need to test this idea.
            //its case sensitive.  i think its okay since its intended for anything.
        }
        public static bool IsIntOrdered<TSource>(this ICustomBasicList<TSource> source, Func<TSource, int?> keySelector, bool ExcludeUnknowns = true)
        {
            if (source.Count == 0)
                return true; //just act like its in order because there was nothing.
            CustomBasicList<int?> ThisList = source.ExtractIntegers(keySelector);
            int x; //starts at 1
            x = 0;
            if (ExcludeUnknowns == true)
                ThisList.RemoveAllOnly(Items => Items.HasValue == false);
            ThisList.Sort();
            if (ThisList.First() != 1)
                return false; //if the first item is not in order, then its not in order.
            foreach (var item in ThisList)
            {
                x++;
                if (item != x)
                    return false;
            }
            return true;
        }
        //decided to use ienumerable now because sometimes it does not quite implement the custom list but is still needed.
        public static CustomBasicList<int?> ExtractIntegers<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> keySelector)
        {
            CustomBasicList<int?> output = new CustomBasicList<int?>();
            foreach (var item in source)
            {
                output.Add(keySelector(item));
            }
            return output;
        }
        public static CustomBasicList<int> ExtractIntegers<TSource>(this IEnumerable<TSource> source, Func<TSource, int> keySelector)
        {
            CustomBasicList<int> output = new CustomBasicList<int>();
            foreach (var item in source)
            {
                output.Add(keySelector(item));
            }
            return output;
        }
        public static int DistinctCount<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            int count = 0;
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    count++;
                }
            }
            return count;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> //2 choices
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static CustomBasicList<TKey> DistinctItems<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            CustomBasicList<TKey> output = new CustomBasicList<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    output.Add(keySelector(element));
                }
            }
            output.Sort();
            return output;
        }
    }
}