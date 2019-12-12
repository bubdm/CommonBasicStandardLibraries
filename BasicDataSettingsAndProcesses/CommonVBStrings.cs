using System;
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public static class Constants //so far, will have this part.  if we need anymore, could have other classes here.
    {
        public const string vbCr = "\r"; //keep the names to be consistent.
        public const string vbLf = "\n";
        public const string vbCrLf = "\r\n";
        public const string vbTab = "\t";
        public const string DoubleQuote = "\""; //hopefully this simple.
        public const string qq = "\""; //decided to do it this way so the typing is much simplier for this purpose.
        public const string SingleQuote = "\'";
        public const string VerticalTab = "\v";
    }
    public static class VBCompat
    {
        public static int AscW(string ThisStr)
        {
            return Convert.ToInt32(ThisStr[0]);
        }
        public static int AscW(char ThisChar)
        {
            return Convert.ToInt32(ThisChar);
        }
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