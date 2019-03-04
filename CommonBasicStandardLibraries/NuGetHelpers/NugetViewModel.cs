using System;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMHelpers;
using static CommonBasicStandardLibraries.MVVMHelpers.Command; //this is used so if you want to know if its still executing, can be done.
using System.Linq; //sometimes i do use linq.
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    public class NugetViewModel : BaseViewModel
    {

        private string _Progress;

        public string Progress
        {
            get { return _Progress; }
            internal set
            {
                if (SetProperty(ref _Progress, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        /// <summary>
        /// You can do either from ui or from console.  Since you can invoke commands from view models.
        /// If doing from console. you can directly use the executeasync though.
        /// Also, this assumes you never uploaded a nuget package.  If you did upload, then you need to the first time manually update the version.
        /// After the first time, it will automatically increment the version as needed by nuget.
        /// </summary>
        public Command UpdateCommand { get; set; }

        public NugetViewModel()
        {
            ThisMod = this;
            ThisBus = new NugetBusiness();
            UpdateCommand = new Command(async x =>
            {
                if (ThisSetting == null)
                    ThisSetting = Resolve<INugetSettings>();
                if (ThisSetting == null)
                {
                    if (ThisMessage == null)
                    {
                        Console.WriteLine("You did not use dependency injection in order to get the setting in order to create and upload nuget packages");
                        return;
                    }
                    else
                    {
                        ThisMessage.ShowError("You did not use dependency injection in order to be able to create and upload the nuget packages");
                        return;
                    }
                }
                await ThisBus.CreateNugetPackage();
            }, x =>
            {
                return true; //just return true for now.
            }, this);
        }
    }
}
