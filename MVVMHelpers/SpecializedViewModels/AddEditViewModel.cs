using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class AddEditViewModel : DataEntryViewModel
    {
        private bool _previousValidated;
        public AddEditViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
        {
            RunFirst();
        }
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
                        _previousValidated = AttemptedToSubmitForm;
                        AttemptedToSubmitForm = false; // has to be put to false
                    }
                    else
                    {
                        AttemptedToSubmitForm = _previousValidated;
                    }

                    ChangeAddVisible();
                }
            }
        }
        protected virtual void ChangeAddVisible() { }
        protected virtual async Task ProcessAdd(object thisObj) { await Task.CompletedTask; }
        public Command? AddCommand { get; set; }
        public Command? EnterCommand { get; set; }
        protected virtual bool CanAdd(object thisObj)
        {
            return base.CanSave(thisObj);
        }

        protected override bool CanSave(object thisObj)
        {
            return true; // because save is different from 
        }
        protected virtual async Task ProcessEnter(object thisObj)
        {
            await Task.CompletedTask;
            AddVisible = !AddVisible;
            if (AddVisible == true)
                RaiseFinish();// to focus on first control
        }
        protected override void ValidationCompleted()
        {
            AddCommand?.ReportCanExecuteChange();
        }
        protected virtual bool CanEnter(object thisObj)
        {
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
    }
}