namespace CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses
{
    public class SpecificListCondition : BaseListCondition, ICondition
    {
        EnumConditionCategory ICondition.ConditionCategory => EnumConditionCategory.ListInclude;
    }
}