using System;
namespace CommonBasicStandardLibraries.Attributes //may include some from dapperhelpers (?)
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AutoClearAttribute : Attribute { }
}