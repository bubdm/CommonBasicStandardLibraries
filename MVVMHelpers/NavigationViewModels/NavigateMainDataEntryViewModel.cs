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
using CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels;

namespace CommonBasicStandardLibraries.MVVMHelpers.NavigationViewModels
{
    public class NavigateMainDataEntryViewModel : DataEntryViewModel, IToggleVM
    {
        public NavigateMainDataEntryViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
        {
        }

        public bool Visible { get; set; }
    }
}
