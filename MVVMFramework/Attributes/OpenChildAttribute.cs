using System;
namespace CommonBasicStandardLibraries.MVVMFramework.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false)]
    public class OpenChildAttribute : Attribute
    {
    }
}
