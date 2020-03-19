using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using CommonBasicStandardLibraries.Exceptions;
using static CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses.SortInfo;
using cs = CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses.ConditionOperators;

namespace CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses
{
    public static class StaticHelpers
    {
        public static CustomBasicList<ICondition> StartConditionWithID(int id)
        {
            CustomBasicList<ICondition> list = new CustomBasicList<ICondition>();
            AndCondition condition = new AndCondition();
            condition.Property = nameof(ISimpleDapperEntity.ID);
            condition.Value = id;
            list.Add(condition);
            return list;
        }

        public static CustomBasicList<ICondition> StartWithOneCondition(string property, object value)
        {
            CustomBasicList<ICondition> list = new CustomBasicList<ICondition>();
            AndCondition condition = new AndCondition();
            condition.Property = property;
            condition.Value = value;
            list.Add(condition);
            return list;
        }
        public static CustomBasicList<ICondition> StartWithNullCondition(string property, string operation)
        {
            CustomBasicList<ICondition> list = new CustomBasicList<ICondition>();
            AndCondition condition = new AndCondition();
            condition.Property = property;
            if (operation != cs.IsNotNull && operation != cs.IsNull)
                throw new BasicBlankException("Only null or is not null is allowed when starting with null conditions");
            //this was needed for the tv shows.
            condition.Operator = operation;
            list.Add(condition);
            return list;
        }

        public static CustomBasicList<ICondition> StartWithOneCondition(string property, string operation, object value)
        {
            CustomBasicList<ICondition> list = new CustomBasicList<ICondition>();
            AndCondition condition = new AndCondition();
            condition.Property = property;
            condition.Value = value;
            condition.Operator = operation;
            list.Add(condition);
            return list;
        }

        public static CustomBasicList<SortInfo> StartSorting(string property)
        {
            SortInfo sort = new SortInfo();
            sort.Property = property;
            return new CustomBasicList<SortInfo> { sort };
        }
        public static CustomBasicList<SortInfo> StartSorting(string property, EnumOrderBy order)
        {
            SortInfo sort = new SortInfo();
            sort.Property = property;
            sort.OrderBy = order;
            return new CustomBasicList<SortInfo> { sort };
        }
        public static CustomBasicList<UpdateEntity> StartUpdate(string property, object value)
        {
            CustomBasicList<UpdateEntity> output = new CustomBasicList<UpdateEntity>
            {
                new UpdateEntity(property, value)
            };
            return output;
        }
    }
}
