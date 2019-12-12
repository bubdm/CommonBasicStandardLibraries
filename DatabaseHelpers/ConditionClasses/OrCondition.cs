using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses
{
    public class OrCondition : ICondition
    {
        EnumConditionCategory ICondition.ConditionCategory { get => EnumConditionCategory.Or; }
        public CustomBasicList<AndCondition> ConditionList = new CustomBasicList<AndCondition>();
    } //this has to be a list of the ones for the and.
}