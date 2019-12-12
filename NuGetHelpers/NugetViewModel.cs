using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using static CommonBasicStandardLibraries.NuGetHelpers.NuGetGlobals;
namespace CommonBasicStandardLibraries.NuGetHelpers
{
    public class NugetViewModel : BaseViewModel
    {

        private string _Progress = "";
        public NugetViewModel(ISimpleUI tempUI) : base(tempUI) //a breaking change in the nuget manager.
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

            UploadOnlyCommand = new Command(async x =>
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
                await ThisBus.UploadAloneAsync(); //this means it will not even version it.
                }, x =>
                {
                    return true;
                }, this);
        }

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
        //public async Task RunTestTraditional()
        //{
        //    //no need for dependency injection while i am running tests
        //    await ThisBus.TestTraditionalNet();
        //}

        /// <summary>
        /// You can do either from ui or from console.  Since you can invoke commands from view models.
        /// If doing from console. you can directly use the executeasync though.
        /// Also, this assumes you never uploaded a nuget package.  If you did upload, then you need to the first time manually update the version.
        /// After the first time, it will automatically increment the version as needed by nuget.
        /// </summary>
        public Command UpdateCommand { get; set; }
        /// <summary>
        /// could be a case where you need to reupload the packages.  in this case, it will just upload them alone.  however in this case, may need to show as its happening.
        /// </summary>
        public Command UploadOnlyCommand { get; set; }

        //public NugetViewModel()
        //{
        //    ThisMod = this;
        //    ThisBus = new NugetBusiness();
        //    UpdateCommand = new Command(async x =>
        //    {
        //        if (ThisSetting == null)
        //            ThisSetting = Resolve<INugetSettings>();
        //        if (ThisSetting == null)
        //        {
        //            if (ThisMessage == null)
        //            {
        //                Console.WriteLine("You did not use dependency injection in order to get the setting in order to create and upload nuget packages");
        //                return;
        //            }
        //            else
        //            {
        //                ThisMessage.ShowError("You did not use dependency injection in order to be able to create and upload the nuget packages");
        //                return;
        //            }
        //        }
        //        await ThisBus.CreateNugetPackage();
        //    }, x =>
        //    {
        //        return true; //just return true for now.
        //    }, this);

        //    UploadOnlyCommand = new Command(async x =>
        //    {
        //        if (ThisSetting == null)
        //            ThisSetting = Resolve<INugetSettings>();
        //        if (ThisSetting == null)
        //        {
        //            if (ThisMessage == null)
        //            {
        //                Console.WriteLine("You did not use dependency injection in order to get the setting in order to create and upload nuget packages");
        //                return;
        //            }
        //            else
        //            {
        //                ThisMessage.ShowError("You did not use dependency injection in order to be able to create and upload the nuget packages");
        //                return;
        //            }
        //        }
        //        await ThisBus.UploadAloneAsync(); //this means it will not even version it.
        //    }, x =>
        //    {
        //        return true;
        //    }, this);
        //}
    }
}
