using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class DataEntryViewModel : BaseViewModel, IDataErrorInfo
    {
        string IDataErrorInfo.Error //this implements the Error property
        {
            get
            {
                return null!;
            }
        }
        public string this[string PropertyName] //this implements the item property of idataerrorinfo
        {
            get
            {
                CollectErrors();
                if (Errors.ContainsKey(PropertyName) == true)
                {
                    return Errors[PropertyName];
                }
                return string.Empty;
            }
        }
        protected override void ExtraStepInChangingProperty([CallerMemberName] string? propertyName = null)
        {
            CollectErrors(); // may try this way.
        }
        protected Dictionary<string, string> Errors = new Dictionary<string, string>(); // for hints
        protected IFocusOnFirst FirstControl { get; set; }
        public CustomBasicCollection<string> ErrorLists { get; } = new CustomBasicCollection<string>(); //hopefully this is fine. don't know though.
        protected void RaiseFinish()
        {
            AttemptedToSubmitForm = false; // because are preparing for next time.
            FirstControl.FocusOnFirstControl();
        }
        public void FocusOnFirstControl()
        {
            FirstControl.FocusOnFirstControl(); //i think for now, it will work this way.
            //may require rethinking (not sure yet).
        }
        private CustomBasicList<PropertyInfo> _propList = new CustomBasicList<PropertyInfo>();
        private bool _attemptedToSubmitForm;
        public bool AttemptedToSubmitForm
        {
            get => _attemptedToSubmitForm;
            set => SetProperty(ref _attemptedToSubmitForm, value);
        }
        protected bool UseBlankString;
        protected void NotifyAllChanges()
        {
            var thisList = PropList;
            foreach (var ThisItem in thisList)
                OnPropertyChanged(ThisItem.Name);// i think
        }
        private CustomBasicList<PropertyInfo> PropList
        {
            get
            {
                if (_propList.Count == 0)
                {

                    var properties = GetType()
                         .GetProperties()
                         .Where(s =>
                         {
                             if (s.IsDefined(typeof(RequiredAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(MaxLengthAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(MinLengthAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(PhoneAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(EmailAddressAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(CustomValidDateAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(CustomDateRangeAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(RangeAttribute)) == true)
                                 return true;
                             if (s.IsDefined(typeof(StringLengthAttribute)) == true)
                                 return true;
                             if (OtherCustomAttributes(s) == true)
                                 return true;
                             return false;
                         }
                        ).ToCustomBasicList();
                    _propList = properties;
                }
                return _propList;
            }
        }
        protected virtual bool OtherCustomAttributes(PropertyInfo s)
        {
            return false; // can put other things into it  if there is a case where i don't have a custom attribute but want to add to the list for automation, i can.
        }
        protected virtual void AddOtherPropertiesAttributes(PropertyInfo thisProp) { }

        protected void AddErrorMessage(PropertyInfo thisProp, string errorMessage)
        {
            Errors.Add(thisProp.Name, errorMessage);
            AddMiscError(errorMessage);
        }
        protected void AddMiscError(string errorMessage)
        {
            ErrorLists.Add(errorMessage);
        }
        private void GetPropErrors(PropertyInfo thisProp)
        {
            var currentValue = thisProp.GetValue(this);
            var requiredAtt = thisProp.GetCustomAttribute<RequiredAttribute>();
            var maxLengthAtt = thisProp.GetCustomAttribute<MaxLengthAttribute>();
            var minAtt = thisProp.GetCustomAttribute<MinLengthAttribute>();
            var phAt = thisProp.GetCustomAttribute<PhoneAttribute>();
            var emAt = thisProp.GetCustomAttribute<EmailAddressAttribute>();
            var stAt = thisProp.GetCustomAttribute<StringLengthAttribute>();
            if (stAt == null == false)
            {
                if (stAt!.IsValid(currentValue) == false)
                    AddErrorMessage(thisProp, stAt.ErrorMessage);
            }
            if (requiredAtt == null == false)
            {
                if (requiredAtt!.IsValid(currentValue) == false)
                    AddErrorMessage(thisProp, requiredAtt.ErrorMessage); // see if it can be smart enough
            }
            var newValue = currentValue?.ToString();
            if (string.IsNullOrWhiteSpace(newValue) == true)
                newValue = "";// this is one way to do it.
            if (maxLengthAtt != null)
            {
                if (newValue!.Length > maxLengthAtt.Length)
                    AddErrorMessage(thisProp, maxLengthAtt.ErrorMessage);
            }
            RangeAttribute rangeA;
            rangeA = thisProp.GetCustomAttribute<RangeAttribute>();
            if (rangeA != null)
            {
                if (rangeA.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, rangeA.ErrorMessage);// can try to use isvalid now.
            }
            if (minAtt != null)
            {
                if (newValue!.Length < minAtt.Length)
                    AddErrorMessage(thisProp, minAtt.ErrorMessage);
            }
            if (phAt != null)
            {
                if (phAt.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, phAt.ErrorMessage);
            }
            if (emAt != null)
            {
                if (emAt.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, emAt.ErrorMessage);
            }
            var CustomSDate = thisProp.GetCustomAttribute<CustomValidDateAttribute>();
            if (CustomSDate != null)
            {
                if (CustomSDate.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, CustomSDate.ErrorMessage);
            }
            var CustomRDate = thisProp.GetCustomAttribute<CustomDateRangeAttribute>();
            if (CustomRDate != null)
            {
                if (CustomRDate.IsValid(newValue!, this) == false)
                    AddErrorMessage(thisProp, CustomRDate.ErrorMessage);
            }
            AddOtherPropertiesAttributes(thisProp);
        }
        public bool StepThrough = false;

        public DataEntryViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempUI)
        {
            FirstControl = tempFocus;
            RunFirst();
        }

        private void CollectErrors()
        {
            Errors.Clear();
            ErrorLists.Clear();
            foreach (var thisProp in PropList)
                GetPropErrors(thisProp);
            OnPropertyChanged(nameof(ErrorLists)); // this changed now.
            SaveCommand?.ReportCanExecuteChange(); //i think this is better
            ValidationCompleted();
        }
        protected virtual void ValidationCompleted() { }
        protected virtual async Task ProcessSave(object thisObj) { await Task.CompletedTask; } //i think
        protected virtual bool CanSave(object thisObj)
        {
            return IsOK(); // defaults to if it passes validation.  however, i can change it and add more advanced parts to it.
        }
        protected bool HasErrors()
        {
            return Errors.Any();
        }
        protected bool IsOK()
        {
            return !HasErrors(); // try this way
        }
        public Command? SaveCommand { get; set; }

        //public DataEntryViewModel(IFocusOnFirst TempFocus) { FirstControl = TempFocus; ThisMessage = TempFocus; RunFirst(); }

        private void RunFirst()
        {
            SaveCommand = new Command(ProcessSave, s =>
            {
                return CanSave(s);
            }, this);
            CollectErrors(); // looks like has to collect errors to begin with now.
        }

        //public DataEntryViewModel()
        //{
        //    RunFirst();
        //}
    }
}
