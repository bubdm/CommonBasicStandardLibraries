using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class StrCat
    {
        private StringBuilder Atoms = new StringBuilder();

        public void ClearString()
        {
            Atoms = new StringBuilder();
        }

        public void AddToString(string Info, string Delimiter = "")
        {
            if (Atoms.Length > 0)
                Atoms.Append(Delimiter);
            Atoms.Append(Info);
        }

        public string GetInfo()
        {
            return Atoms.ToString();
        }
    }
}
