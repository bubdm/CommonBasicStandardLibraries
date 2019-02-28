using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ListProgress
    {
        //public static HashSet<T> ToHashSet<T>(this IEnumerable<T> TempList)
        //{
        //    return new HashSet<T>(TempList);
        //}

        public static string ProgressString<T> (this CustomBasicList<T> ThisList, T ThisItem, string MiscText)
        {
            int Index = ThisList.IndexOf(ThisItem);
            if (Index == -1)
                throw new BasicBlankException("Item Not Found");
            Index += 1;
            return $"{ MiscText} # {Index} of {ThisList.Count} on {DateTime.Now}";
        }

        public static void WriteProgress(this string ThisStr)
        {
            Console.WriteLine($"{ThisStr} on {DateTime.Now}");
        }

        public static void WriteProgress<T>(this CustomBasicList<T> ThisList, T ThisItem, string MiscText)
        {
            string ThisProgress = ThisList.ProgressString(ThisItem, MiscText);
            Console.WriteLine(ThisProgress);
        }
    }
}