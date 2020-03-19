using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
namespace CommonBasicStandardLibraries.NuGetHelpers
{


    public class NugetUIClass
    {
        public async Task UpdateAsync()
        {
            if (ThisSetting == null)
            {
                ThisSetting = Resolve<INugetSettings>();
            }
            if (ThisSetting == null)
            {
                Console.WriteLine("You did not use dependency injection in order to be able to create and upload the nuget packages");
                return;
            }
            await ThisBus!.CreateNugetPackage();
        }

        public NugetUIClass()
        {
            ThisBus = new NugetBusiness();
            ThisUI = this;
        }
    }
}
