using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    internal static class NuGetGlobals
    {
        //this stores all global variables
        public static  NugetViewModel ThisMod;
        public static INugetSettings ThisSetting;
        public static NugetBusiness ThisBus;
    }
}
