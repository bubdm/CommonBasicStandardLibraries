using System;
using System.Collections.Generic;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class Characters
    {
        public static bool IsAlpha(this char ThisChar, bool AllowDots = false)
        {
            if (AscW(ThisChar) >= 97 && AscW(ThisChar) <= 122)
                return true;// because lower case
            if (AscW(ThisChar) >= 65 && AscW(ThisChar) <= 90)
                return true;
            if (AllowDots == true)
            {
                if (AscW(ThisChar) == 46)
                    return true;
            }
            return false;
        }

        public static bool IsInteger(this char ThisChar)
        {
            if (AscW(ThisChar) >= 48 && AscW(ThisChar) <= 57)
                return true;
            else
                return false;
        }
    }
}
