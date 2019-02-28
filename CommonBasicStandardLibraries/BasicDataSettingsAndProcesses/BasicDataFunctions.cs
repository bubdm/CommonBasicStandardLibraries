using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Linq;
using static CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.ListsExtensions;
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public static class BasicDataFunctions
    {
        public static IResolver cons; // at this point, we don't know who is going to implement this.


    //    Public Enum EnumOS
    //    None = 0
    //    WindowsDT = 1
    //    Android = 2
    //    Linux = 3
    //    Macintosh = 4
    //End Enum

    //Public Property OS As EnumOS


            public enum EnumOS
        {
            None,WindowsDT, WindowsRT,Android,Linux,Macintosh
        }

        public static EnumOS OS { get; set; } = EnumOS.WindowsDT; //most of the time, its windows desktop.  however, it can be set
        //the purpose of this is so things can run differently whether its windows, etc.

        public async static Task WaitBlank()
        {
            bool rets;
            rets = true;
            if (rets == false)
                await Task.Delay(1);
        }

        public static T Resolve<T>()
        {
            return cons.Resolve<T>();
        }

        public static string GetSQLServerConnectionString(string DatabaseOrPath) //this is intended for sql server.  this means can have several implementations of this.
        {
            ISQLServer sqls = cons.Resolve<ISQLServer>();
            return sqls.GetConnectionString(DatabaseOrPath);
        }

		

        public delegate Task ActionAsync<in T> (T obj); //this is used so if there is a looping, then it can await it if needed.

        //public delegate Object ConvertData(object Results);

        public delegate object ConvertTranslation(object EndResults);


        public delegate void TextEventData(string ThisStr); //in this case the argument being received is text.  this can be used anywhere the argument being passed back is a string

		public delegate void ErrorRaisedEventHandler(string Message); //this is when i want to make it clear its error data.  anything can use this.

		public delegate void UpdateFunct<T>(T ThisObj, int ExpectedItem); //the purpose of this is so i can update.  for this to work, the list has to determine the proper value
		//then this will actually update it.
		//its intended to run on all.  otherwise, could mess up a query (?)
		//if its conditional, then need additional extensions as well.

		//if i have a logger, maybe can use it.


		public static  CustomBasicList<int>  GetIntegerList(int StartAt, int HowMany)
		{
			return Enumerable.Range(StartAt, HowMany).ToCustomBasicList();
		}

    }
}
