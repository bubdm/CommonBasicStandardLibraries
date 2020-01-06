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

namespace CommonBasicStandardLibraries.MVVMHelpers
{
    public abstract class ParentBaseViewModel : BaseViewModel
    {

        protected IToggleVM? FirstScreen { get; set; }
        protected string MainTitle { get; set; } = ""; //this is so it can easily set to main title if necessary.

        public ParentBaseViewModel(ISimpleUI tempUI) : base(tempUI)
        {
        }
        public Action? ShowChangedState { get; set; } //needs to be public so it can work from the web.
        //because the parent may or may not have the web portion of it.
        //protected Task StartOverAsync(IToggleVM visibleVM, string title)
        //{
        //    visibleVM.Visible = true;
        //    Title = title;
        //    ChangeState();
        //    return Task.CompletedTask;
        //}

        protected virtual Task StartOverAsync()
        {
            Title = MainTitle;
            ResetToMainScreen();
            return Task.CompletedTask;
        }
        protected virtual void ResetToMainScreen()
        {
            if (FirstScreen == null)
            {
                throw new BasicBlankException("You must have a main screen to set visible to");
            }
            FirstScreen.Visible = true;
            ChangeState();
        }
        protected void ChangeState()
        {
            ShowChangedState?.Invoke(); //try an experiment.
        }
        protected virtual void ChangeScreen(IToggleVM vm, string newTitle) //this will clear out the properties from previous time.  if that is not enough, then rethink.
        {
            //i think this should be allowed to change if necessary.
            Title = newTitle;
            vm.AutoClearProperties();
            vm.Visible = true;
            ChangeState(); //i think changing screen means change state as well.
        }

    }
}
