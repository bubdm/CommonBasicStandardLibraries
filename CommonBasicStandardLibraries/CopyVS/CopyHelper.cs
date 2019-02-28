using System;
using System.Collections.Generic;
using System.Text;
using vb = CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.Constants;
namespace CommonBasicStandardLibraries.CopyVS
{
    internal static class CopyHelper
    {
        public static string GetCleanText(string ThisStr)
        {
            string FirstText;
            FirstText = ThisStr.Substring(0, 1);

            if (FirstText == vb.vbCr)
                FirstText = ThisStr.Substring(1);
            if (FirstText == vb.vbLf)
                FirstText = ThisStr.Substring(1);
            return FirstText;
        }




    }
}
