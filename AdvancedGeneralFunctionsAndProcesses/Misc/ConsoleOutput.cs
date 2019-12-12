using System;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public class ConsoleOutput : IConsole
    {
        bool IConsole.ExtraSpaces => false;

        void IConsole.WriteLine(object thisObject)
        {
            Console.WriteLine(thisObject.ToString());
        }
    }
    //can't do the test one because that would require a dependency to the xunit.  this can't require xunit dependencies.
}
