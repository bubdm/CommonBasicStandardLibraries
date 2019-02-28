using CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class DataEntryViewModel : BaseViewModel, IDataErrorInfo
    {
        string IDataErrorInfo.Error //this implements the Error property
        {
            get
            {
                return null;
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

        

        protected override void ExtraStepInChangingProperty([CallerMemberName] string propertyName = null)
        {
            // validate for errors for just one property
            // OnGenericMessageToClient(propertyName & " has changed.  Attempting to check")
            CollectErrors(); // may try this way.
        }

        protected Dictionary<string, string> Errors = new Dictionary<string, string>(); // for hints


        public IFocusOnFirst FirstControl { get; set; }

        public CustomBasicCollection<string> ErrorLists { get; } = new CustomBasicCollection<string>(); //hopefully this is fine. don't know though.




        //public ObservableRangeCollection<string> ErrorLists
        //{
        //    get
        //    {
        //        return PrivateList;
        //    }
        //}

        protected void RaiseFinish()
        {
            AttemptedToSubmitForm = false; // because are preparing for next time.
            //IsBusy = false; // needs this as well here
            FirstControl.FocusOnFirstControl();
        }

        // Protected Sub OnFocusOnFirstControl()
        // RaiseEvent FocusOnFirstControl()
        // End Sub



        // Public Sub ValidateEntireForm()
        // CollectErrors() 'try this way.
        // End Sub

        private List<PropertyInfo> _PropList = new List<PropertyInfo>(); // looks like i could not make it shared.
		//otherwise, if there is more than one view model, it will get hosed.

        // Protected DidRunValidation As Boolean
        private bool _AttemptedToSubmitForm;
        public bool AttemptedToSubmitForm
        {
            //get => title;

            get => _AttemptedToSubmitForm;


            set => SetProperty(ref _AttemptedToSubmitForm, value);


        }

        protected bool UseBlankString;
        // well see what happens when it comes to integers.

        protected void ClearPropertiesWithAttributes()
        {
            var ThisList = PropList;
            foreach (var ThisItem in ThisList)
            {
                if (UseBlankString == false)
                    ThisItem.SetValue(this, null); // even better.  well see how that works.
                else if (ThisItem.PropertyType == typeof(string))
                    ThisItem.SetValue(this, "");
                else
                    ThisItem.SetValue(this, null);
            }
        }

        protected void NotifyAllChanges()
        {
            var ThisList = PropList;
            foreach (var ThisItem in ThisList)
                OnPropertyChanged(ThisItem.Name);// i think
        }

        private List<PropertyInfo> PropList
        {
            get
            {
                if (_PropList.Count == 0)
                {

                    var Properties = GetType()
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
                        ).ToList();
					_PropList = Properties;
                }
                return _PropList;
            }
        }

        protected virtual bool OtherCustomAttributes(PropertyInfo s)
        {
            return false; // can put other things into it  if there is a case where i don't have a custom attribute but want to add to the list for automation, i can.
        }

        protected virtual void AddOtherPropertiesAttributes(PropertyInfo ThisProp)
        {
        }

        protected void AddErrorMessage(PropertyInfo ThisProp, string ErrorMessage)
        {
            Errors.Add(ThisProp.Name, ErrorMessage);
            AddMiscError(ErrorMessage);
        }

        protected void AddMiscError(string ErrorMessage)
        {
            ErrorLists.Add(ErrorMessage);
        }

        private void GetPropErrors(PropertyInfo ThisProp)
        {
            var CurrentValue = ThisProp.GetValue(this);
            var RequiredAtt = ThisProp.GetCustomAttribute<RequiredAttribute>();
            var MaxLengthAtt = ThisProp.GetCustomAttribute<MaxLengthAttribute>();
            var MinAtt = ThisProp.GetCustomAttribute<MinLengthAttribute>();
            var PhAt = ThisProp.GetCustomAttribute<PhoneAttribute>();
            var EmAt = ThisProp.GetCustomAttribute<EmailAddressAttribute>();
            var StAt = ThisProp.GetCustomAttribute<StringLengthAttribute>();
            if (StAt == null == false)
            {
                if (StAt.IsValid(CurrentValue) == false)
                    AddErrorMessage(ThisProp, StAt.ErrorMessage);
            }
            if (RequiredAtt == null == false)
            {
                if (RequiredAtt.IsValid(CurrentValue) == false)
                    AddErrorMessage(ThisProp, RequiredAtt.ErrorMessage); // see if it can be smart enough
            }

            var NewValue = CurrentValue?.ToString();
            if (string.IsNullOrWhiteSpace(NewValue) == true)
                NewValue = "";// this is one way to do it.
            if (MaxLengthAtt == null == false)
            {
                if (NewValue.Length > MaxLengthAtt.Length)
                    AddErrorMessage(ThisProp, MaxLengthAtt.ErrorMessage);
            }
            RangeAttribute RangeA;
            RangeA = ThisProp.GetCustomAttribute<RangeAttribute>();
            if (RangeA == null == false)
            {
                if (RangeA.IsValid(NewValue) == false)
                    AddErrorMessage(ThisProp, RangeA.ErrorMessage);// can try to use isvalid now.
            }
            if (MinAtt == null == false)
            {
                if (NewValue.Length < MinAtt.Length)
                    AddErrorMessage(ThisProp, MinAtt.ErrorMessage);
            }
            if (PhAt == null == false)
            {
                if (PhAt.IsValid(NewValue) == false)
                    AddErrorMessage(ThisProp, PhAt.ErrorMessage);
            }
            if (EmAt == null == false)
            {
                if (EmAt.IsValid(NewValue) == false)
                    AddErrorMessage(ThisProp, EmAt.ErrorMessage);
            }
            var CustomSDate = ThisProp.GetCustomAttribute<CustomValidDateAttribute>();
            if (CustomSDate == null == false)
            {
                if (CustomSDate.IsValid(NewValue) == false)
                    AddErrorMessage(ThisProp, CustomSDate.ErrorMessage);
            }
            var CustomRDate = ThisProp.GetCustomAttribute<CustomDateRangeAttribute>();
            if (CustomRDate == null == false)
            {
                if (CustomRDate.IsValid(NewValue, this) == false)
                    AddErrorMessage(ThisProp, CustomRDate.ErrorMessage);
            }
            AddOtherPropertiesAttributes(ThisProp);
        }

		public bool StepThrough = false;

        private void CollectErrors()
        {
            
            Errors.Clear();
            ErrorLists.Clear();
			
            foreach (var ThisProp in PropList)
                GetPropErrors(ThisProp);
			
            OnPropertyChanged(nameof(ErrorLists)); // this changed now.
            if (SaveCommand == null == false)
                SaveCommand.ReportCanExecuteChange();


            ValidationCompleted();
        }

        protected virtual void ValidationCompleted()
        {
        }

        protected virtual async Task ProcessSave(object ThisObj) { await WaitBlank(); }

        protected virtual bool CanSave(object ThisObj)
        {
            //if (IsBusy == true)
            //    return false;
            // Stop
            // because it does have to run all the time afterall
            // if i don't then can't focus on the correct fields

            // If DidRunValidation = False Then
            // Return False 'must run my custom validation first.
            // End If
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

        public Command SaveCommand { get; set; }



        public DataEntryViewModel()
        {
            SaveCommand = new Command(ProcessSave, s =>
            {
                return CanSave(s);
            }, this);
            CollectErrors(); // looks like has to collect errors to begin with now.
        }
    }
}
