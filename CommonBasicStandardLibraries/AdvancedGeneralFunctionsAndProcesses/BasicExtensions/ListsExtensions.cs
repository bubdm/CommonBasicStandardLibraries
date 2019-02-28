using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ListsExtensions
    {
        //decided to put thelistextensions as now part of the main extension namespace
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> TempList)
        {
            return new HashSet<T>(TempList);
        }

        public static CustomBasicList<T> ToCustomBasicList<T> (this IEnumerable<T> TempList)
        {
            return new CustomBasicList<T>(TempList);
        }



        public static CustomBasicCollection<T> ToCustomBasicCollection<T> (this IEnumerable<T> TempList)
        {
            return new CustomBasicCollection<T>(TempList);
        }

        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> ThisDict, TValue ThisValue)
        {
            if (ThisDict == null)
                throw new ArgumentNullException(nameof(ThisDict));

            foreach(KeyValuePair<TKey, TValue> ThisPair in ThisDict)
            {
                if (ThisValue.Equals(ThisPair.Value) == true || ReferenceEquals(ThisPair.Value, ThisValue) == true)
                    return ThisPair.Key;
            }
            throw new BasicBlankException($"The Value Of {ThisValue} Was Not Found In The Dictionary");
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

		private static void  OldIncrementIntegers<T> (this ICustomBasicList<T> ThisList, Func<T, int> Selector, int StartAt = 1)
		{

			//int q = 1;
			//ref int x = ref q;
			int xx = StartAt;


			
			

			//Action NewAction(out int ThisInt)
			//{
			//	xx += 1;
			//	ThisInt = xx;
			//	return;
			//}

			ThisList.ForEach(ThisItem =>
			{
				//Selector.Invoke()
				//Console.WriteLine(Selector.Invoke(ThisItem)); //start here.
				//the good news is i can get a specific item to do an action on.
				//however would like to be able to change it.
				int q = Selector.Invoke(ThisItem);
				//this means if you need to retrieve a specific value, i can easily do that now.

				xx++;
				//ref int zz = yy;


			}
			);
		}

        //for now, done.  but could add others.




    }
}
