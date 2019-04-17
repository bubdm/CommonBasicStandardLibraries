using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using System.Linq;
using CommonBasicStandardLibraries.Exceptions;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Integers
    {
        public static string Join(this CustomBasicList<int> ThisList, string Delimiter)
        {
            StrCat cats = new StrCat();
            ThisList.ForEach(x => cats.AddToString(x.ToString(), Delimiter));
            return cats.GetInfo();
        }

        public static (int Batches, int LeftOver) GetRemainderInfo(this int ThisInt, int BatchSize) //if i need the class, can do later.  don't remember if i needed the real class or not.
        {
            int x = 0;
            int b = 0;
            for (int i = 1; i < ThisInt; i++)
            {
                x += 1;
                if (x == BatchSize)
                {
                    x = 0;
                    b++;
                }
            }
            return (b, x);
        }

        public static string ConvertToSpecificStrings(this int ThisInt, int DesiredDigits)
        {
            string Temps = ThisInt.ToString();
            if (Temps.Count() > DesiredDigits)
                throw new BasicBlankException($"The Integer Of {ThisInt} has more digits than the desired digits of {DesiredDigits}");

            if (Temps.Count() == DesiredDigits)
                return Temps;
            int Padding = DesiredDigits - Temps.Count();
            StrCat cats = new StrCat();
            for (int i = 0; i < Padding; i++)
            {
                cats.AddToString("0");
            }
            cats.AddToString(Temps);
            return cats.GetInfo();
        }
        public static string MusicProgressStringFromMillis(this int MilliSecondsUpTo, int DurationMilliseconds)
        {
            TimeSpan ProgressSpan = TimeSpan.FromMilliseconds(MilliSecondsUpTo);
            TimeSpan DurationSpan = TimeSpan.FromMilliseconds(DurationMilliseconds);
            return ProgressSpan.SongProgress(DurationSpan);
        }
        public static string MusicProgressStringFromSeconds(this int SecondsUpTo, int DurationSeconds)
        {
            TimeSpan ProgressSpan = TimeSpan.FromSeconds(SecondsUpTo);
            TimeSpan DurationSpan = TimeSpan.FromSeconds(DurationSeconds); //meant to use from seconds.
            return ProgressSpan.SongProgress(DurationSpan);
        }
        public static int MultiplyPercentage(this int Amount, int Percentage) => (int)Math.Ceiling(((decimal)Percentage / 100) * Amount); //decided this needs to be clear it multiplies

        public static T ToEnum<T>(this int param) //i may need to cast other times to enums and even generic enums.
        {
            var info = typeof(T);
            if (info.IsEnum)
            {
                T result = (T)Enum.Parse(typeof(T), param.ToString(), true);
                return result;
            }

            return default;
        }
        public static int FromEnum<T>(this T param) where T: Enum
        {
            object Firsts = Convert.ChangeType(param, param.GetTypeCode());
            return (int)Firsts;
        }

        public static void Times(this int @this, Action action)
        {
            for (var i = 0; i < @this; i++)
            {
                action?.Invoke();
            }
        }

        public static void Times(this int @this, Action<int> action)
        {
            for (var i = 0; i < @this; i++)
            {
                action?.Invoke(i + 1); //we want it one based.
            }
        }

        public async static Task TimesAsync(this int @this, Func<Task> action)
        {
            for (var i = 0; i < @this; i++)
            {
                await action?.Invoke();
            }
        }

        public async static Task TimesAsync(this int @this, Func<int, Task> action)
        {
            for (var i = 0; i < @this; i++)
            {
                await action?.Invoke(i + 1); //we want it one based.
            }
        }

    }
}
