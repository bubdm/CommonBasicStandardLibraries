using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.DatabaseHelpers.Attributes;
using CommonBasicStandardLibraries.DatabaseHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace CommonBasicStandardLibraries.DatabaseHelpers.MiscClasses
{
    public class ChangeTracker
    {
        private Dictionary<string, object> _originalValues = new Dictionary<string, object>();
        public void PopulateOriginalDictionary(Dictionary<string, object> savedOriginal) //the server has to put in the original dictionary
        {
            _originalValues = savedOriginal;
        }
        public Dictionary<string, object> GetOriginalValues()
        {
            return new Dictionary<string, object>(_originalValues);
        }
        public void Initialize() //from api if updating, needs to call the populateoriginaldictionary
        {
            //you are on your own if you mess up unfortunately.
            _originalValues.Clear();
            var tempList = _thisType.GetProperties().Where(Items => Items.CanMapToDatabase() == true && Items.Name != "ID"); //id can never be tracked
            tempList = tempList.Where(Items => Items.HasAttribute<ExcludeUpdateListenerAttribute>() == false);
            tempList = tempList.Where(Items => Items.HasAttribute<ForeignKeyAttribute>() == false); //because the foreigns would never be updated obviously.
            foreach (PropertyInfo property in tempList)
            {
                _originalValues.Add(property.Name, property.GetValue(_thisObject, null));
            }
        }
        private readonly object _thisObject;
        private readonly Type _thisType;
        public ChangeTracker(object thisObject)
        {
            _thisObject = thisObject;
            _thisType = _thisObject.GetType();
        }
        public CustomBasicList<string> GetChanges()
        {
            CustomBasicList<string> output = new CustomBasicList<string>();
            foreach (var thisValue in _originalValues)
            {
                PropertyInfo property = _thisType.GetProperties().Where(Items => Items.Name == thisValue.Key).Single();
                object newValue = property.GetValue(_thisObject, null);
                if (IsUpdate(thisValue.Value, newValue) == true)
                    output.Add(property.Name);
            }
            return output;
        }
        private bool IsUpdate(object thisValue, object newValue)
        {
            if (thisValue == null && newValue == null)
                return false;
            if (thisValue == null)
                return true;
            if (thisValue.Equals(newValue) == false)
                return true;
            return false;
        }
    }
}