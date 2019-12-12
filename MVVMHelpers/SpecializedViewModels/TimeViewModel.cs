using CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Reflection;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class TimeViewModel : DataEntryViewModel
    {
        private bool _CanEnableTimeFeatures = true; // defaults to true.  however, may be cases where it can't be done.
        public bool CanEnableTimeFeatures
        {
            get
            {
                return _CanEnableTimeFeatures;
            }

            set
            {
                if (SetProperty(ref _CanEnableTimeFeatures, value) == true)
                {
                    ChangeToDayCommand!.ReportCanExecuteChange();
                    ChangeToHourCommand!.ReportCanExecuteChange();
                    ChangeToNoneCommand!.ReportCanExecuteChange();
                }
            }
        }
        private CustomTimeAttribute.EnumTimeFormat _TimeCategory = CustomTimeAttribute.EnumTimeFormat.None; // defaults to none.
        public CustomTimeAttribute.EnumTimeFormat TimeCategory
        {
            get
            {
                return _TimeCategory;
            }

            set
            {
                if (SetProperty(ref _TimeCategory, value) == true)
                    OnPropertyChanged(nameof(TimeString));// i think this is needed so it can reevaluate
            }
        }
        protected override bool OtherCustomAttributes(PropertyInfo s)
        {
            bool rets;
            rets = s.IsDefined(typeof(CustomTimeAttribute));
            return rets;
        }
        protected override void AddOtherPropertiesAttributes(PropertyInfo ThisProp)
        {
            var customSDate = ThisProp.GetCustomAttribute<CustomTimeAttribute>();
            if (customSDate != null)
            {
                try
                {
                    var firstValue = ThisProp.GetValue(this);
                    string currentValue;
                    if (firstValue == null)
                        currentValue = "";
                    else
                        currentValue = (string)firstValue!;
                    if (customSDate.IsValid(currentValue, TimeCategory) == false)
                        AddErrorMessage(ThisProp, customSDate.ErrorMessage);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private string _TimeString = ""; // this will be built in.  if i wanted to have other cases, can do so.  it would need at least one.  otherwise, would not even use this.

        public TimeViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
        {
            RunFirst();
        }

        [CustomTime]
        public string TimeString
        {
            get
            {
                return _TimeString;
            }

            set
            {
                if (SetProperty(ref _TimeString, value) == true)
                {
                }
            }
        }
        public Command? ChangeToHourCommand { get; set; }

        public Command? ChangeToDayCommand { get; set; }

        public Command? ChangeToNoneCommand { get; set; }

        // decided to add minutes for ease
        public Command? ChangeToMinutesCommand { get; set; }

        private bool DoEnable()
        {
            return CanEnableTimeFeatures;
        }

        private void RunFirst()
        {
            ChangeToHourCommand = new Command(x =>
            {
                TimeCategory = CustomTimeAttribute.EnumTimeFormat.Hours;
            }, x =>
            {
                return DoEnable();
            }, this);
            ChangeToDayCommand = new Command(x =>
            {
                TimeCategory = CustomTimeAttribute.EnumTimeFormat.Days;
            }, x =>
            {
                return CanEnableTimeFeatures;
            }, this);
            ChangeToNoneCommand = new Command(x =>
            {
                TimeCategory = CustomTimeAttribute.EnumTimeFormat.None;
            }, x =>
            {
                return CanEnableTimeFeatures;
            }, this);
            ChangeToMinutesCommand = new Command(x =>
            {
                TimeCategory = CustomTimeAttribute.EnumTimeFormat.Minutes;
            }, x =>
            {
                return CanEnableTimeFeatures;
            }, this);
        }
        //public TimeViewModel(IFocusOnFirst TempFocus) { FirstControl = TempFocus; ThisMessage = TempFocus; RunFirst(); }

        //public TimeViewModel()
        //{

        //    //since we don't need parameter, i guess this is the way to do it this time.
        //    RunFirst();

        //}
    }
}
