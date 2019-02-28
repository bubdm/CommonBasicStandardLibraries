using System;
using System.Collections.Generic;
using System.Text;

namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
    public static class ApplicationPath
    {
         
        public static string GetApplicationPath() // this is the path the application is in
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static bool IsDebug()
        {
            return System.Diagnostics.Debugger.IsAttached;
        }

        public static void RunTest()
        {
            //System.IO.StreamWriter exps = new System.IO.StreamWriter("",  );
            //exps.WriteLineAsync("Hello" ,)

        
            //since async is now supported on console.main, then its probably best to start doing more of it now.
            //this means when creating an object, if there is async stuff, since that can't be awaited, then another method is needed to init.

        }


    }
}
