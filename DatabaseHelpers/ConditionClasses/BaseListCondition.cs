using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses
{
    public abstract class BaseListCondition
    {
        public CustomBasicList<int> ItemList { get; set; } = new CustomBasicList<int>(); //so i can send in a list and things will be done to specific items.
    }
}