using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] //will make this only on property
    public abstract class DataTypeAttribute : ValidationAttribute
    {
        public DataTypeAttribute (string ErrorMessage, EnumDataType DataType) : base(ErrorMessage)
        {
            this.DataType = DataType;
        }

        public DataTypeAttribute(EnumDataType DataType)
        {
            this.DataType = DataType;
        }

        public EnumDataType DataType { get; set; }

    }
}
