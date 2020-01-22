using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class InterfaceExtensions
    {
        public static void AutoClearProperties(this IClearable thisObj) //i do like having as extension now.
        {
            Type thisType = thisObj.GetType();
            CustomBasicList<PropertyInfo> thisList = thisType.GetPropertiesWithAttribute<AutoClearAttribute>().ToCustomBasicList();
            thisList.ForEach(items => items.SetValue(thisObj, default));
        }

        public static T AutoMap<T>(this IMappable payLoad) //this does the most simple option.  if i need something more complex, i will have more details to figure out what to do.
            where T : new()
        {
            //for now, i think this is fine.  i may have to redo some things.  however, i'm not sure what the best way to handle different scenarios currently.


            Type payType = payLoad.GetType();
            if (payType.IsSubclassOf(typeof(ViewModelBase)))
            {
                CustomBasicList<PropertyInfo> oldProperties = payType.GetMappableProperties();
                Type newType = typeof(T);
                CustomBasicList<PropertyInfo> newProperties = newType.GetMappableProperties(); //i think if new ones has some not mappable, will ignore.
                T output = new T();
                newProperties.ForEach(newP =>
                {
                    PropertyInfo old = oldProperties.SingleOrDefault(item => item.Name == newP.Name);
                    if (old != null)
                    {
                        newP.SetValue(output, old.GetValue(payLoad)); //hopefully this simple.
                    }
                });
                return output;
            }
            else
            {
                //throw new BasicBlankException("Json");
                string results = JsonConvert.SerializeObject(payLoad);
                return JsonConvert.DeserializeObject<T>(results);

            }
        } //saves the extra lines of code.  i like it even better as extensions.  if you map a list to a single item, will error out.  that is okay.
    }
}