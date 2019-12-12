using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using static CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses.SortInfo;
using cs = CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses.ConditionOperators;

namespace CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses
{
    public static class StaticHelpers
    {
        public static CustomBasicList<ICondition> StartConditionWithID(int ID)
        {
            CustomBasicList<ICondition> ThisList = new CustomBasicList<ICondition>();
            AndCondition ThisCon = new AndCondition();
            ThisCon.Property = nameof(ISimpleDapperEntity.ID);
            ThisCon.Value = ID;
            ThisList.Add(ThisCon);
            return ThisList;
        }

        public static CustomBasicList<ICondition> StartWithOneCondition(string Property, object Value)
        {
            CustomBasicList<ICondition> ThisList = new CustomBasicList<ICondition>();
            AndCondition ThisCon = new AndCondition();
            ThisCon.Property = Property;
            ThisCon.Value = Value;
            ThisList.Add(ThisCon);
            return ThisList;
        }
        public static CustomBasicList<ICondition> StartWithNullCondition(string Property, string Operator)
        {
            CustomBasicList<ICondition> ThisList = new CustomBasicList<ICondition>();
            AndCondition ThisCon = new AndCondition();
            ThisCon.Property = Property;
            if (Operator != cs.IsNotNull && Operator != cs.IsNull)
                throw new BasicBlankException("Only null or is not null is allowed when starting with null conditions");
            //this was needed for the tv shows.
            ThisCon.Operator = Operator;
            ThisList.Add(ThisCon);
            return ThisList;
        }

        public static CustomBasicList<ICondition> StartWithOneCondition(string Property, string Operator, object Value)
        {
            CustomBasicList<ICondition> ThisList = new CustomBasicList<ICondition>();
            AndCondition ThisCon = new AndCondition();
            ThisCon.Property = Property;
            ThisCon.Value = Value;
            ThisCon.Operator = Operator;
            ThisList.Add(ThisCon);
            return ThisList;
        }

        public static CustomBasicList<SortInfo> StartSorting(string Property)
        {
            SortInfo ThisSort = new SortInfo();
            ThisSort.Property = Property;
            return new CustomBasicList<SortInfo> { ThisSort };
        }
        public static CustomBasicList<SortInfo> StartSorting(string Property, EnumOrderBy Order)
        {
            SortInfo ThisSort = new SortInfo();
            ThisSort.Property = Property;
            ThisSort.OrderBy = Order;
            return new CustomBasicList<SortInfo> { ThisSort };
        }
        public static CustomBasicList<UpdateEntity> StartUpdate(string Property, object value)
        {
            CustomBasicList<UpdateEntity> output = new CustomBasicList<UpdateEntity>
            {
                new UpdateEntity(Property, value)
            };
            return output;
        }
    }
}
