using System;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMHelpers;
using static CommonBasicStandardLibraries.MVVMHelpers.Command; //this is used so if you want to know if its still executing, can be done.
using System.Linq; //sometimes i do use linq.
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.MVVMHelpers.NavigationViewModels
{
    public class NavigateMainSimpleViewModel : BaseViewModel, IToggleVM
    {
        public NavigateMainSimpleViewModel(ISimpleUI tempUI) : base(tempUI)
        {
            Visible = true; //the main one should start out with visible.
        }

        public bool Visible { get; set; }
    }
}
