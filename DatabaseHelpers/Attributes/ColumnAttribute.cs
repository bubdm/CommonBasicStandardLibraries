/// <summary>
/// 
/// </summary>
namespace CommonBasicStandardLibraries.DatabaseHelpers.Attributes
{
    using System;
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnAttribute(string columnName)
        {
            //TableName = TSQLTableName;
            ColumnName = columnName;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColumnName { get; private set; }

    }
}