using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
            if (payType.IsSubclassOf(typeof(BaseViewModel)))
            {
                //throw new BasicBlankException("Trying New");
                CustomBasicList<PropertyInfo> oldProperties = payType.GetMappableProperties();

                //CustomBasicList<PropertyInfo> oldList = oldProperties.Where(item => item.IsEnumerable()).ToCustomBasicList();
                //CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat.Stop();
                //CustomBasicList<PropertyInfo> simpleOld = oldProperties.Where(item => item.IsEnumerable() == false).ToCustomBasicList();
                Type newType = typeof(T);

                CustomBasicList<PropertyInfo> newProperties = newType.GetMappableProperties(); //i think if new ones has some not mappable, will ignore.

                //CustomBasicList<PropertyInfo> newList = newProperties.Where(item => item.IsEnumerable()).ToCustomBasicList();
                //CustomBasicList<PropertyInfo> simpleNew = newProperties.Where(item => item.IsEnumerable() == false).ToCustomBasicList();


                T output = new T();
                //start without the list.
                //CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat.Stop();
                //throw new BasicBlankException(simpleNew.Count().ToString());
                newProperties.ForEach(newP =>
                {
                    //CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat.Stop();
                    //throw new BasicBlankException(newP.Name);
                    PropertyInfo old = oldProperties.SingleOrDefault(item => item.Name == newP.Name);
                    if (old != null)
                    {
                        //has to set the old to the new.
                        //throw new BasicBlankException($"Trying to get old value.  Received {old.GetValue(null)} value");
                        newP.SetValue(output, old.GetValue(payLoad)); //hopefully this simple.
                    }
                });

                //maybe it does not matter on the other part (?)

                //newList.ForEach(newP =>
                //{

                //    PropertyInfo old = oldList.SingleOrDefault(item => item.Name == newP.Name);
                //    //CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat.Stop();
                //    if (old != null)
                //    {

                //        //throw new BasicBlankException("Help 2");
                //        //CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.VBCompat.Stop();
                //        //has to set the old to the new.
                //        newP.SetValue(output, old.GetValue(payLoad)); //hopefully this simple.
                //    } //hopefully this simple.  will assume that part at least matches.
                //});


                //return Activator.CreateInstance(thisType, args);
                //to make it simple, only work if it can do without parameters in constructor.



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
