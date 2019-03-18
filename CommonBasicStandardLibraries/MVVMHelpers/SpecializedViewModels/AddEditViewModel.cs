using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class AddEditViewModel : DataEntryViewModel
    {
        private bool PreviousValidated;

        private bool _AddVisible;
        public bool AddVisible
        {
            get
            {
                return _AddVisible;
            }

            set
            {
                if (SetProperty(ref _AddVisible, value) == true)
                {
                    // code to run
                    if (value == false)
                    {
                        PreviousValidated = AttemptedToSubmitForm;
                        AttemptedToSubmitForm = false; // has to be put to false
                    }
                    else
                        AttemptedToSubmitForm = PreviousValidated;
                    ChangeAddVisible();
                }
            }
        }

        protected virtual void ChangeAddVisible()
        {
        }

        protected virtual async Task ProcessAdd(object ThisObj) { await WaitBlank(); }

        public Command AddCommand { get; set; }
        public Command EnterCommand { get; set; }

        protected virtual bool CanAdd(object ThisObj)
        {
            return base.CanSave(ThisObj);
        }

        protected override bool CanSave(object ThisObj)
        {
            //if (IsBusy == true)
            //    return false;
			//for now, will defaut to it can.  however,  something else can implement and decide whether it really can or not (?)
            return true; // because save is different from 
        }

        protected virtual async Task ProcessEnter(object ThisObj)
        {
			await WaitBlank();
            AddVisible = !AddVisible;
            if (AddVisible == true)
                RaiseFinish();// to focus on first control
        }

        protected override void ValidationCompleted()
        {
            if (AddCommand == null == true)
                return;
            AddCommand.ReportCanExecuteChange(); // because this can change as a result.
        }

        protected virtual bool CanEnter(object ThisObj)
        {
            //if (IsBusy == true)
            //    return false;// most of the time.
            return true; // can usually do it.  however, sometimes, it won't do it.
        }

        private void RunFirst()
        {
            EnterCommand = new Command(async a =>
            {
                await ProcessEnter(a);
            }, x =>
            {
                return CanEnter(x);
            }, this);

            AddCommand = new Command(ProcessAdd, s =>
            {
                return CanAdd(s);
            }, this);
        }
        public AddEditViewModel(IFocusOnFirst TempFocus) { FirstControl = TempFocus; ThisMessage = TempFocus; RunFirst(); }
        public AddEditViewModel()
        {
            RunFirst();
        }

    }
}
