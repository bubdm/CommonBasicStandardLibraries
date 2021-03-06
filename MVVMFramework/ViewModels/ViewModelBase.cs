using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.CustomValidationClasses;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    //decided that this one would handle either data entry or regular now.
    //means that anything with attributes that has to do with validations will be validated
    //attempt to do what i did for the data entry one.

    public abstract class ViewModelBase : ObservableObject, IHaveDisplayName, IDataErrorInfo, IViewModelBase
    {
        //one possible solution is to have something set it.
        //that way it can be used if necessary for data entry, etc.

        public static ViewModelBase? CurrentViewModel { get; set; }

        public ViewModelBase()
        {
            DisplayName = ToString();
            CurrentViewModel = this; //hopefully this way works.
        } //i think the base runs first so if nothing is specified, then will be tostring().

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
                    return Errors[PropertyName];
                return string.Empty;
            }
        }

        private string _displayName = "";
        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public virtual string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                NotifyOfPropertyChange();
            }
        }
        protected bool HasErrors()
        {
            return ErrorLists.Any(); //try this one.
        }
        protected bool IsOK()
        {
            return !HasErrors(); // try this way
        }

        protected Dictionary<string, string> Errors = new Dictionary<string, string>(); // for hints
        public CustomBasicCollection<string> ErrorLists { get; } = new CustomBasicCollection<string>(); //hopefully this is fine. don't know though.

        protected virtual void ValidationCompleted() { }

        private bool _attemptedToSubmitForm;
        public bool AttemptedToSubmitForm
        {
            get => _attemptedToSubmitForm;
            set => SetProperty(ref _attemptedToSubmitForm, value);
        }
        protected bool UseBlankString;
        private CustomBasicList<PropertyInfo> _propList = new CustomBasicList<PropertyInfo>();

        protected void NotifyAllChanges()
        {
            var thisList = PropList;
            foreach (var ThisItem in thisList)
                OnPropertyChanged(ThisItem.Name);// i think
        }
        //hopefully i am able to manually collect errors.
        protected void CollectErrors()
        {
            Errors.Clear();
            ErrorLists.Clear();
            foreach (var thisProp in PropList)
                GetPropErrors(thisProp);
            //OnPropertyChanged(nameof(ErrorLists)); // for this one, i have to do differently unfortunately because of how data entry processes work.
            CheckMiscErrors();
            
            ValidationCompleted();
        }

        protected virtual void CheckMiscErrors() { }

        public virtual bool IsValid
        {
            get { return IsOK(); }
        }
        protected override void ExtraStepInChangingProperty([CallerMemberName] string? propertyName = null)
        {
            CollectErrors();
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
                             if (s.IsDefined(typeof(CustomTimeAttribute)) == true)
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

        private void AddErrorMessage(PropertyInfo thisProp, string errorMessage)
        {
            Errors.Add(thisProp.Name, errorMessage);
            AddMiscError(errorMessage);
        }
        //protected void AddErrorMessage(string thisProp, string errorMessage)
        //{
        //    Errors.Add(thisProp, errorMessage);
        //    AddMiscError(errorMessage);
        //}
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
                if (stAt!.IsValid(currentValue) == false)
                    AddErrorMessage(thisProp, stAt.ErrorMessage);
            if (requiredAtt == null == false)
                if (requiredAtt!.IsValid(currentValue) == false)
                    AddErrorMessage(thisProp, requiredAtt.ErrorMessage); // see if it can be smart enough
            var newValue = currentValue?.ToString();
            if (string.IsNullOrWhiteSpace(newValue) == true)
                newValue = "";// this is one way to do it.
            if (maxLengthAtt != null)
                if (newValue!.Length > maxLengthAtt.Length)
                    AddErrorMessage(thisProp, maxLengthAtt.ErrorMessage);
            RangeAttribute rangeA;
            rangeA = thisProp.GetCustomAttribute<RangeAttribute>();
            if (rangeA != null)
                if (rangeA.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, rangeA.ErrorMessage);// can try to use isvalid now.
            if (minAtt != null)
                if (newValue!.Length < minAtt.Length)
                    AddErrorMessage(thisProp, minAtt.ErrorMessage);
            if (phAt != null)
                if (phAt.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, phAt.ErrorMessage);
            if (emAt != null)
                if (emAt.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, emAt.ErrorMessage);
            var customSDate = thisProp.GetCustomAttribute<CustomValidDateAttribute>();
            if (customSDate != null)
                if (customSDate.IsValid(newValue!) == false)
                    AddErrorMessage(thisProp, customSDate.ErrorMessage);
            var customRDate = thisProp.GetCustomAttribute<CustomDateRangeAttribute>();
            if (customRDate != null)
                if (customRDate.IsValid(newValue!, this) == false)
                    AddErrorMessage(thisProp, customRDate.ErrorMessage);

            var customTDate = thisProp.GetCustomAttribute<CustomTimeAttribute>();
            if (customTDate != null && customTDate.IsValid(newValue!) == false)
            {
                AddErrorMessage(thisProp, customTDate.ErrorMessage);
            }

            AddOtherPropertiesAttributes(thisProp);
        }
        protected void FinishSaved()
        {
            AttemptedToSubmitForm = false;
            this.AutoClearProperties();
            IEventAggregator aggregator = cons!.Resolve<IEventAggregator>();
            aggregator.FocusOnFirst();
        }
    }
}
