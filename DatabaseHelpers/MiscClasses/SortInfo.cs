//i think this is the most common things i like to do
using CommonBasicStandardLibraries.DatabaseHelpers.MiscInterfaces;
namespace CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses
{
    public class SortInfo : IProperty
    {
        public enum EnumOrderBy
        {
            Ascending, Descending
        }
        public string Property { get; set; } = "";
        public EnumOrderBy OrderBy { get; set; } = EnumOrderBy.Ascending;
        //this time just a list of sortinfo.  no interfaces needed this time.
    }
}