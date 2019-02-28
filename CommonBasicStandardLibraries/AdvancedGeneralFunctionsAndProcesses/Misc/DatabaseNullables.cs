using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public static class DatabaseNullables
    {
        public static bool WhatBoolean(Nullable<bool> ThisBoolean)
        {
            if (ThisBoolean.HasValue == false)
                return false;
            return ThisBoolean.Value;
        }

        public static bool WhatBoolean(bool ThisBoolean)
        {
            return ThisBoolean;
        }

        public static bool WhatBoolean(string ThisString)
        {
            if (ThisString == null == true)
                return false;
            if (ThisString.Trim().Length == 0)
                return false;
            ThisString = WhatString(ThisString);
            if (ThisString.ToLower() == "false")
                return false;
            if (ThisString.ToLower() == "true")
                return true;
            if (ThisString.Length > 1)
                throw new Exception("Boolean strings has to be 1 character long; not " + ThisString.Length + " long");
            if (ThisString.ToLower() == "y")
                return true;
            if (ThisString.ToLower() == "n")
                return false;
            if (ThisString == "1")
                return true;
            if (ThisString == "0")
                return false;
            if (ThisString == "-1")
                return true;
            bool.TryParse(ThisString, out bool NewValue);
            return NewValue;
        }

        public static bool WhatBoolean(int ThisInt)
        {
            if (ThisInt == 0)
                return false;
            if (ThisInt == 1)
                return true;
            if (ThisInt == -1)
                return true;// i think -1 should be true as well
            throw new Exception("The Integer has to be 0 or 1 or -1 for booleans; not " + ThisInt);
        }

        public static bool WhatBoolean(Nullable<int> ThisInt)
        {
            if (ThisInt.HasValue == false)
                return false;
            return WhatBoolean(ThisInt.Value);
        }

        public static bool WhatBoolean(Nullable<char> ThisChar)
        {
            if (ThisChar.HasValue == false)
                return false;
            return WhatBoolean(ThisChar.Value);
        }

        public static bool WhatBoolean(char ThisChar)
        {
            if (ThisChar.ToString().ToLower() == "y")
                return true;
            if (ThisChar.ToString().ToLower() == "n")
                return false;
            if (ThisChar.ToString() == "0")
                return false;
            if (ThisChar.ToString() == "1")
                return true;
            if (ThisChar.ToString() == "-1")
                return true;
            throw new Exception("Cannot find out whether true or false for " + ThisChar);
        }

        public static string WhatString(int? ThisInt)
        {
            if (ThisInt.HasValue == false)
                return "";
            return ThisInt.ToString();
        }

        public static string WhatString(int ThisInt)
        {
            return ThisInt.ToString();
        }

        public static string WhatString(decimal? ThisDec)
        {
            if (ThisDec.HasValue == false)
                return "";
            return ThisDec.ToString();
        }

        public static string WhatString(decimal ThisDec)
        {
            return ThisDec.ToString();
        }

        public static string WhatString(string ThisString)
        {
            if (ThisString == null == true)
                return "";
            return ThisString.Trim();
        }

        public static string WhatString(char ThisString)
        {
            return ThisString.ToString();
        }

        public static string WhatString(Nullable<char> ThisString)
        {
            if (ThisString.HasValue == false)
                return "";
            return ThisString.ToString();
        }

        public static decimal WhatDecimal(Nullable<decimal> ThisDec)
        {
            if (ThisDec.HasValue == false)
                // Dim ThisStr As String
                // 'ThisStr = ThisItem.ToString
                // ThisStr = String.Format("$0.00", 0)
                // Stop
                return decimal.Parse(string.Format("$0.00", 0));
            return Math.Round(ThisDec.Value, 2);
        }

        public static decimal WhatDecimal(decimal ThisDec)
        {
            return Math.Round(ThisDec, 2);
        }

        public static decimal WhatDecimal(string ThisDec)
        {
            ThisDec = WhatString(ThisDec);
            decimal.TryParse(ThisDec, out decimal NewValue);
            return Math.Round(NewValue, 2);
        }

        public static int WhatInteger(Nullable<int> ThisInt)
        {
            if (ThisInt.HasValue == false)
                return 0;
            return ThisInt.Value;
        }

        public static int WhatInteger(int ThisInt)
        {
            return ThisInt;
        }

        public static int WhatInteger(string ThisInt)
        {
            ThisInt = WhatString(ThisInt); // to take out the spaces
            try
            {
                int.TryParse(ThisInt, out int NewValue);
                return NewValue;
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}
