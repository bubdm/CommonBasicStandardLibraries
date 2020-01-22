using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
namespace CommonBasicStandardLibraries.NuGetHelpers
{

    //not sure if we need the nuget view model anymore.
    //if i am wrong, then will rethink.

    public class NugetUIClass
    {
        //no need for progress since this is running on console.
        //no need for an actual view model this time.
        //if i change my mind, just can override from observableobject or view model base.

        public async Task UpdateAsync()
        {
            //we only need update alone now.
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
