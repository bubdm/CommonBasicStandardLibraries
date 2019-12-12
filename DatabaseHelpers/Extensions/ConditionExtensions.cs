using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses;
using System.Linq;
using cs = CommonBasicStandardLibraries.DatabaseHelpers.ConditionClasses.ConditionOperators;
namespace DapperHelpersLibrary.Extensions
{
    public static class ConditionExtensions
    {
        public static CustomBasicList<ICondition> AppendCondition(this CustomBasicList<ICondition> tempList, string property, object value)
        {
            return tempList.AppendCondition(property, cs.Equals, value);
        }
        public static CustomBasicList<ICondition> AppendRangeCondition(this CustomBasicList<ICondition> tempList, string property,
            object lowRange, object highRange)
        {
            return tempList.AppendCondition(property, cs.GreaterOrEqual, lowRange).AppendCondition(property, cs.LessThanOrEqual, highRange);
        }
        public static CustomBasicList<ICondition> AppendCondition(this CustomBasicList<ICondition> tempList, string property, string toperator, object value)
        {
            AndCondition thisCon = new AndCondition();
            thisCon.Property = property;
            thisCon.Operator = toperator;
            thisCon.Value = value;
            tempList.Add(thisCon);
            return tempList;
        }
        public static CustomBasicList<ICondition> JoinedCondition(this CustomBasicList<ICondition> tempList, string tableCode)
        {
            AndCondition thisCon = (AndCondition)tempList.Last();
            thisCon.Code = tableCode;
            return tempList;
        }

        public static CustomBasicList<ICondition> AppendContains(this CustomBasicList<ICondition> tempList, CustomBasicList<int> containList)
        {
            SpecificListCondition thisCon = new SpecificListCondition();
            thisCon.ItemList = containList;
            tempList.Add(thisCon);
            return tempList;
        }
        public static CustomBasicList<ICondition> AppendsNot(this CustomBasicList<ICondition> tempList, CustomBasicList<int> notList)
        {
            NotListCondition thiscon = new NotListCondition();
            thiscon.ItemList = notList;
            tempList.Add(thiscon);
            return tempList;
        }
        public static OrCondition AppendOr(this OrCondition thisOr, string property, object value)
        {
            var thisCon = new AndCondition();
            thisCon.Property = property;
            thisCon.Value = value;
            thisOr.ConditionList.Add(thisCon);
            return thisOr;
        }
        public static OrCondition AppendOr(this OrCondition thisOr, string property, string toperator, object value)
        {
            var ThisCon = new AndCondition();
            ThisCon.Property = property;
            ThisCon.Operator = toperator;
            ThisCon.Value = value;
            thisOr.ConditionList.Add(ThisCon);
            return thisOr;
        }
    }
}