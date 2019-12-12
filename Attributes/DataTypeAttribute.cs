using System;
namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] //will make this only on property
    public abstract class DataTypeAttribute : ValidationAttribute
    {
        public DataTypeAttribute(string errorMessage, EnumDataType dataType) : base(errorMessage)
        {
            DataType = dataType;
        }

        public DataTypeAttribute(EnumDataType dataType)
        {
            DataType = dataType;
        }

        public EnumDataType DataType { get; set; }

    }
}
