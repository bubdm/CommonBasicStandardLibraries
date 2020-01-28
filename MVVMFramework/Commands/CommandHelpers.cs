using CommonBasicStandardLibraries.CollectionClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.Exceptions;

namespace CommonBasicStandardLibraries.MVVMFramework.Commands
{
    public static class CommandHelpers
    {

        //public ReflectiveCommand GetReflectiveCommand<T>(string name, )

        public static ReflectiveCommand GetReflectiveCommand(object payLoad, string name)
        {
            Type type = payLoad.GetType();

            CustomBasicList<MethodInfo> methods = type.GetMethods().ToCustomBasicList();
            methods.KeepConditionalItems(x => (x.ReturnType.Name == "Task" ||
            x.ReturnType.Name == "Void" ||
            x.ReturnType.Name == "Boolean")
            && x.Name.StartsWith("get") == false
            && x.Name.StartsWith("set") == false
            && x.Name.StartsWith("add") == false
            && x.Name.StartsWith("remove") == false
            && x.Name != "NotifyOfPropertyChange"
            && x.Name != "FocusOnFirstControl"
            && x.Name != "Refresh"
            );
            //for now, don't worry about some repeating.

            //the first part must be command period.
            //for this part, name has to be exact


            var tempList = methods.Where(x => x.Name == name).ToCustomBasicList();
            if (tempList.Count > 1)
            {
                throw new BasicBlankException($"More than one with the name of {name} was found");
            }
            if (tempList.Count == 0)
            {
                throw new BasicBlankException($"No method with the name of {name} was found when creating reflective command");
            }

            MethodInfo method = tempList.Single();

            //has to see if we now have a function to go along with it.
            string searchName = GetSearchName(method);
            MethodInfo? functionM = null;
            functionM = methods.FirstOrDefault(x => x.Name == "Can" + searchName);
            //if a function is found, just use it and not both with property.  try that way.
            //i don't think this should be to open child.  if i am wrong, rethink.
            PropertyInfo? functionP = null;
            if (functionM == null)
            {
                CustomBasicList<PropertyInfo> properties = type.GetProperties().ToCustomBasicList();
                functionP = properties.FirstOrDefault(x => x.Name == "Can" + searchName);
            }
            ReflectiveCommand output;
            if (functionP == null && functionM != null)
            {
                output = new ReflectiveCommand(payLoad, method, functionM);
            }
            else
            {
                output = new ReflectiveCommand(payLoad, method, functionP);
            }
            return output;
        }

        public static string GetSearchName(MethodInfo method)
        {
            if (method.Name.ToLower().EndsWith("async"))
                return method.Name.Substring(0, method.Name.Count() - 5);
            return method.Name;
        }

    }
}
