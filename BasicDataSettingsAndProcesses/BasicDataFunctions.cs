using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Linq;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Reflection;
using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.ContainerClasses;
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public static class BasicDataFunctions
    {
        public static IResolver? cons; // at this point, we don't know who is going to implement this.
        //looks like we need this one after all.
        //because when creating a brand new custom list that has a random function, needs static random
        //did it the way we did to also allow for unit testing.
        //if there was no cons, one will be created using my implementation of the interface.

        public enum EnumOS
        {
            None,WindowsDT, WindowsRT,Android,Linux,Macintosh
        }
        public static EnumOS OS { get; set; } = EnumOS.WindowsDT; //most of the time, its windows desktop.  however, it can be set
        //the purpose of this is so things can run differently whether its windows, etc.
        //no more awaitblank because we have better ways of doing it now.

        public static T Resolve<T>()
        {
            if (cons == null)
                cons = new ContainerMain();
            return cons.Resolve<T>();
        }
        public static string GetSQLServerConnectionString(string databaseOrPath) //this is intended for sql server.  this means can have several implementations of this.
        {
            ISQLServer sqls = cons!.Resolve<ISQLServer>();
            return sqls.GetConnectionString(databaseOrPath);
        }
        public delegate Task ActionAsync<in T> (T obj); //this is used so if there is a looping, then it can await it if needed.
        public delegate object VisibleTranslation(bool endResults); 
        public delegate void TextEventData(string TthisStr); //in this case the argument being received is text.  this can be used anywhere the argument being passed back is a string
		public delegate void ErrorRaisedEventHandler(string message); //this is when i want to make it clear its error data.  anything can use this.
		public delegate void UpdateFunct<T>(T thisObj, int expectedItem); //the purpose of this is so i can update.  for this to work, the list has to determine the proper value
		public static  CustomBasicList<int>  GetIntegerList(int startAt, int howMany)
		{
			return Enumerable.Range(startAt, howMany).ToCustomBasicList();
		}
        //public static void AutoClearProperties(object thisObj)
        //{
        //    Type thisType = thisObj.GetType();
        //    CustomBasicList<PropertyInfo> thisList = thisType.GetPropertiesWithAttribute<AutoClearAttribute>().ToCustomBasicList();
        //    thisList.ForEach(items => items.SetValue(thisObj, default));
        //}
    }
}