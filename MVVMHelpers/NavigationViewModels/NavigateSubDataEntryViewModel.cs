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
    public class NavigateSubDataEntryViewModel : DataEntryViewModel, INavigateVM
    {
        public NavigateSubDataEntryViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
        {
        }

        protected virtual void CreateBackButton()
        {
            this.CreateBackCommand(this); //this simple.
        }
        public Func<Task>? SaveAction { get; set; } //this is standard just like a normal data entry has a save command
        public Command? BackCommand { get; set; }
        public Func<Task>? BackAction { get; set; }
        public bool Visible { get; set; }
        protected override Task ProcessSave(object thisObj)
        {
            if (SaveAction == null)
            {
                return Task.CompletedTask;
            } //for data entry, would never use generics to pass parameters
            //because you are entering something and the parent knows about the children.
            return SaveAction!.Invoke();
        }
    }
}
