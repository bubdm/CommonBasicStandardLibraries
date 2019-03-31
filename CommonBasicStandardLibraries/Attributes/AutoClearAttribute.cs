using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoClearAttribute : Attribute
    {
    }
}
