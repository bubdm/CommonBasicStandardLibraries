using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public static class Constants //so far, will have this part.  if we need anymore, could have other classes here.
    {
        public const string vbCr = "\r";

        public const string vbLf = "\n";

        public const string vbCrLf = "\r\n";

        public const string vbTab = "\t";

        //these are actually escape sequences.  however, i did not know that. will try an experiment.

        public const string DoubleQuote = "\""; //hopefully this simple.
        public const string qq = "\""; //decided to do it this way so the typing is much simplier for this purpose.

        public const string SingleQuote = "\'";

        public const string VerticalTab = "\v";

        //i think this is enough for now. will try an experiment to see how it works.

    }

    public static class VBCompat
    {
        public static int AscW(string ThisStr)
        {
            return System.Convert.ToInt32(ThisStr[0]);
        }

        public static int AscW(char ThisChar)
        {
            return Convert.ToInt32(ThisChar);
        }

        //var MyKeyChr = char.ConvertFromUtf32((int) e.KeyCode)

        public static string ChrW(int ThisKey)
        {
            return Convert.ToChar(ThisKey).ToString();
        }


        public static void Stop()
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
