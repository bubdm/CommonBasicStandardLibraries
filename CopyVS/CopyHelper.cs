using System;
using System.Collections.Generic;
using System.Text;
using vb = CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.Constants;
namespace CommonBasicStandardLibraries.CopyVS
{
    internal static class CopyHelper
    {
        public static string GetCleanText(string thisStr)
        {
            string firstText;
            firstText = thisStr.Substring(0, 1);

            if (firstText == vb.vbCr)
                firstText = thisStr.Substring(1);
            if (firstText == vb.vbLf)
                firstText = thisStr.Substring(1);
            return firstText;
        }
    }
}