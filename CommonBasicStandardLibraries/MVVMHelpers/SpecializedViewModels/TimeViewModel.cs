using CommonBasicStandardLibraries.MVVMHelpers.CustomValidationClasses;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
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
                    // code to run
                    ChangeToDayCommand.ReportCanExecuteChange();
                    ChangeToHourCommand.ReportCanExecuteChange();
                    ChangeToNoneCommand.ReportCanExecuteChange();
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
                    // code to run
                    // not sure if this needs to trigger anything.
                    OnPropertyChanged(nameof(TimeString));// i think this is needed so it can reevaluate
            }
        }

        protected override bool OtherCustomAttributes(PropertyInfo s)
        {
            // Dim CustomSDate = s.GetCustomAttribute(Of CustomTimeAttribute)
            // Return Not CustomSDate Is Nothing
            // Console.WriteLine("Others")
            bool rets;
            rets = s.IsDefined(typeof(CustomTimeAttribute));
            // Console.WriteLine(rets & " for property " & s.Name)
            return rets;
        }

        protected override void AddOtherPropertiesAttributes(PropertyInfo ThisProp)
        {
            // Throw New Exception("Test")
            // Console.WriteLine("Trying Custom")

            // Exit Sub 'try this way.
            var CustomSDate = ThisProp.GetCustomAttribute<CustomTimeAttribute>();
            if (CustomSDate == null == false)
            {
                Console.WriteLine(ThisProp.Name);
                // Stop
                // Exit Sub
                // Console.WriteLine("Had Custom")
                // Throw New Exception("Added other properties attributes")
                // Stop
                try
                {
                    // Stop
                    // Dim CurrentValue = ThisProp.GetValue(Me).ToString
                    var FirstValue = ThisProp.GetValue(this);
                    string CurrentValue;
                    if (FirstValue == null == true)
                        CurrentValue = "";
                    else
                        CurrentValue = (string)FirstValue;
                    if (CustomSDate.IsValid(CurrentValue, TimeCategory) == false)
                        // Console.WriteLine(CustomSDate.ErrorMessage)
                        // Stop
                        AddErrorMessage(ThisProp, CustomSDate.ErrorMessage);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debugger.Break();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private string _TimeString = ""; // this will be built in.  if i wanted to have other cases, can do so.  it would need at least one.  otherwise, would not even use this.

        // 
        // 
        // <Required(AllowEmptyStrings:=False, ErrorMessage:="Must Enter Time String")>
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

        // no need for tuplie function here because it should be an extension so it can be used anywhere, not just where you have the view/model
        // then again, maybe its best to have here too.  however, this can use the extension as well.



        // Public Event ShowTimeError(Message As String)

        // Protected Sub OnShowTimeError(Message As String)
        // RaiseEvent ShowTimeError(Message)
        // End Sub




        public Command ChangeToHourCommand { get; set; }

        public Command ChangeToDayCommand { get; set; }

        public Command ChangeToNoneCommand { get; set; }

        // decided to add minutes for ease
        public Command ChangeToMinutesCommand { get; set; }

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

        public TimeViewModel(IFocusOnFirst TempFocus) { FirstControl = TempFocus; ThisMessage = TempFocus; RunFirst(); }

        public TimeViewModel()
        {

            //since we don't need parameter, i guess this is the way to do it this time.
            RunFirst();
            
        }
    }
}
