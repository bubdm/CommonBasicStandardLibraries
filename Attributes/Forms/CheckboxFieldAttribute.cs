using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes.Forms
{
    public class CheckboxFieldAttribute : Attribute
    {
        public string HeaderLabel { get; set; } = "";
        public int FToggleKey { get; set; }
        public CheckboxFieldAttribute(string header, int key = 0)
        {
            HeaderLabel = header;
            FToggleKey = key;
        }
    }
}
