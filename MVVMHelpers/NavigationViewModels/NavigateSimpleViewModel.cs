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
    public abstract class NavigateSimpleViewModel : BaseViewModel, INavigateVM
    {


        public NavigateSimpleViewModel(ISimpleUI tempUI) : base(tempUI)
        {
            CreateBackButton(); //this way if you want to have another implementation to the error info, that would be possible.

        }

        protected virtual void CreateBackButton()
        {
            this.CreateBackCommand(this); //this simple.
        }

        public Command? BackCommand { get; set; }
        public Func<Task>? BackAction { get; set; }
        public bool Visible { get; set; }
    }
}
