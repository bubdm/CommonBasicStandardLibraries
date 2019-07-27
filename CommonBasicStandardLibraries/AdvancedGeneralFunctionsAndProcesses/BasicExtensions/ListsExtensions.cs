using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Reflection;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ListsExtensions
    {

        public static void Add<T>(this Dictionary<int, T> thisDict, T thisItem)
        {
            thisDict.Add(thisDict.Count + 1, thisItem); //this is used in cases where we do a dictionary just for the purpose of one based items
        }
        public static T GetRandomItem<T>(this Dictionary<int, T> thisList)
        {
            return thisList.Values.ToCustomBasicList().GetRandomItem();
        }

        //decided to put thelistextensions as now part of the main extension namespace
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> TempList)
        {
            return new HashSet<T>(TempList);
        }

        public static CustomBasicList<T> ToCustomBasicList<T> (this IEnumerable<T> TempList)
        {
            return new CustomBasicList<T>(TempList);
        }

        //public static CustomBasicList<S> ToCastedList<T, S>(this IEnumerable<T> Templist)
        //{
        //    CustomBasicList<S> NewList = new CustomBasicList<S>(); //if i do count, then i have enumerate again.
        //    foreach (T ThisT in Templist)
        //    {
        //        object NewObj = ThisT;
        //        NewList.Add((S)NewObj);
        //    }
        //    NewList.TrimExcess();
        //    return NewList;
        //    //return new CustomBasicList<S>(Templist); //hopefully this can work.
        //}

        public static CustomBasicList<T> ToCastedList<T>(this IEnumerable<object> TempList) //in this case, you get another list.
        {
            return TempList.Cast<T>().ToCustomBasicList();
            //return new CustomBasicList<T>(TempList: (IEnumerable<T>) TempList);
        }


        public static CustomBasicCollection<T> ToCustomBasicCollection<T> (this IEnumerable<T> TempList)
        {
            return new CustomBasicCollection<T>(TempList);
        }

       

        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> ThisDict, TValue ThisValue)
        {
            
            if (ThisDict == null)
                throw new ArgumentNullException(nameof(ThisDict));

            foreach (KeyValuePair<TKey, TValue> ThisPair in ThisDict)
            {
                if (ThisValue.Equals(ThisPair.Value) == true || ReferenceEquals(ThisPair.Value, ThisValue) == true)
                    return ThisPair.Key;
            }
            throw new BasicBlankException($"The Value Of {ThisValue} Was Not Found In The Dictionary");
        }
        public static CustomBasicList<string> GetStringListFromGivenProperty<T> (this ICustomBasicList<T> thisList, string propertyName)
        {
            CustomBasicList<string> output = new CustomBasicList<string>();
            thisList.ForEach(thisItem =>
            {
                output.Add(thisItem.GetStringValue(propertyName));
            });
            return output;
        }
        public static CustomBasicList<T> CreateCustomListFromIntegers<T>(CustomBasicList<int> thisList, string propertyName)
            where T: new()
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
            var thisProp = thisItem.GetType().GetRuntimeProperty(propertyName);
            return (string) thisProp.GetValue(thisItem); //try this way.
        }


        public static void PopulateBlankList(this ICustomBasicList<string> ThisList, int HowMany)
        {
            for (int i = 0; i < HowMany; i++)
            {
                ThisList.Add("");
            }
        }

        public static CustomBasicList<string> CastIntegerListToStringList(this ICustomBasicList<int> ThisList)
        {
            CustomBasicList<string> NewList = new CustomBasicList<string>();
            ThisList.ForEach(x => NewList.Add(x.ToString()));
            return NewList;
        }

		//i think the new function dealing with integers should be here since its not very common.

		
		public static void IncrementIntegers<T> (this ICustomBasicList<T> ThisList, UpdateFunct<T> selector, int StartAt = 1)
		{
			ThisList.ForEach(ThisItem =>
			{
				selector.Invoke(ThisItem, StartAt);
				StartAt++;
			}
			);
		}

        public static async Task ReconcileStrings<T>(this ICustomBasicList<string> SentList, ICustomBasicList<T> SavedList, Func<T, string> match, Func<string, Task<T>> result)
        {
            CustomBasicList<string> TempList = new CustomBasicList<string>();
            SavedList.ForEach(Items => TempList.Add(match(Items)));
            CustomBasicList<T> RemoveList = new CustomBasicList<T>();
            CustomBasicList<T> AddList = new CustomBasicList<T>();
            TempList.ForEach(Items =>
            {
                if (SentList.Contains(Items) == false)
                    RemoveList.Add(SavedList[TempList.IndexOf(Items)]);
            });

            await SentList.ForEachAsync(async Items =>
            {
                if (TempList.Contains(Items) == false)
                    AddList.Add(await result(Items));
            });
            SavedList.RemoveGivenList(RemoveList, System.Collections.Specialized.NotifyCollectionChangedAction.Remove);
            SavedList.AddRange(AddList);
            //you have to do both.  there is no other way around this
            //i think its best this time to have as an extension.
        }

        public static CustomBasicList<ConditionActionPair<T>> Append<T> (this CustomBasicList<ConditionActionPair<T>> TempList, Predicate<T> Match, Action<T, string> Action, string Value = "") //if it needs to be something else. rethink
        {
            ConditionActionPair<T> ThisC = new ConditionActionPair<T>(Match, Action, Value);
            TempList.Add(ThisC);
            return TempList;
        }

        public static void WriteString<T>(this ICustomBasicList<T> ThisList)
        {
            ThisList.ForEach(Items => Console.WriteLine(Items.ToString()));
        }

        public static bool HasDuplicates <TSource, TKey>(this ICustomBasicList<TSource> source, Func<TSource, TKey> keySelector)
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
                    //return true;
            }
            return output;
        }

        public static bool DoesReconcile<TSource, TKey> (this IEnumerable<TSource> source, IEnumerable<TSource> other, Func<TSource, TKey> keySelector)
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

        public static bool IsIntOrdered <TSource>(this ICustomBasicList<TSource> source, Func<TSource, int?> keySelector, bool ExcludeUnknowns = true)
        {
            if (source.Count == 0)
                return true; //just act like its in order because there was nothing.
            CustomBasicList<int?> ThisList = source.ExtractIntegers(keySelector);
            
            //starts at 1.
            
            int x;
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
            int Count = 0;
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    Count++;
                    //yield return element;
                }
            }
            return Count;
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
                    //yield return element;
                }
            }
            output.Sort();
            return output;
        }

    }
}
