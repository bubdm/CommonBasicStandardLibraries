using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses
{
    public interface IValidateConditions
    {
        bool IsValid(CustomBasicList<ICondition> conditionList, out string message);
    }
}