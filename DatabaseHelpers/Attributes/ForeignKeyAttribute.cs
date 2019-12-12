using System;
namespace CommonBasicStandardLibraries.DatabaseHelpers.Attributes
{
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string className)
        {
            ClassName = className;
        }
        public string ClassName { get; private set; }
    }
}