using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.Attributes;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class BaseContactManagerViewModel<C, P> : AddEditViewModel// because we have a phone and a desktop version of it.
        where C : class
        where P : class
    {
        public enum EnumPhoneCategory
        {
            None = 0, // try adding this one.
            HomeMainPhone = 1, // defaults to 1
            BusinessPhone = 2,
            HusbandMobilePhone = 3,
            WifeMobilePhone = 4,
            AlternatePhone = 5,
            RegularMobilePhone = 6
        }
        // can't be none.  otherwise does not work with listbox
        public enum EnumRelationship
        {
            // None = 0 'defaults to 0
            Business = 1,
            Family = 2,
            CurrentChurch = 3,
            PreviousChurch = 4,
            OldCollege = 5,
            Other = 6
        }
        public enum EnumEditStatus
        {
            None = 0,
            Edit = 1,
            Add = 2
        }
        public enum EnumAddEditCategory
        {
            None = 0,
            Contact = 1,
            Phone = 2
        }
        private EnumEditStatus _editStatus = EnumEditStatus.None;
        public EnumEditStatus EditStatus
        {
            get
            {
                return _editStatus;
            }

            set
            {
                if (SetProperty(ref _editStatus, value) == true)
                    ChangeScreens();
            }
        }
        private EnumAddEditCategory _addEditCategory = EnumAddEditCategory.None;
        public EnumAddEditCategory AddEditCategory
        {
            get
            {
                return _addEditCategory;
            }

            set
            {
                if (SetProperty(ref _addEditCategory, value) == true)
                    // code to run
                    ChangeScreens();
            }
        }
        private void ChangeScreens()
        {
            OnPropertyChanged(nameof(ContactAddScreenVisible)); // must use nameof.  otherwise, does not work.  not sure why.  that is the way it is.
            OnPropertyChanged(nameof(ContactEditScreenVisible));
            OnPropertyChanged(nameof(PhoneAddScreenVisible));
            OnPropertyChanged(nameof(PhoneEditScreenVisible));
            OnPropertyChanged(nameof(MainListVisible));
        }
        protected abstract bool IsPhone { get; } // can decide what to do whether its phone or not.
        private C? _selectedItem;
        public C? SelectedItem //we could decide on interfaces but not sure now though.
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                if (SetProperty(ref _selectedItem, value) == true)
                {
                    EnterCommand!.ReportCanExecuteChange();
                    StartEditContactCommand!.ReportCanExecuteChange(); // not sure why it worked on desktop.
                    OnSelectItemChange();
                }
            }
        }
        protected virtual void OnSelectItemChange() { }
        protected P? PhoneChosen;
        public bool MainListVisible
        {
            get
            {
                if (EditStatus == EnumEditStatus.None)
                    return true;
                return false;
            }
        }
        public bool ContactAddScreenVisible
        {
            get
            {
                if (EditStatus != EnumEditStatus.Add)
                    return false;
                if (AddEditCategory == EnumAddEditCategory.Contact)
                    return true;
                return false;
            }
        }
        public bool PhoneAddScreenVisible
        {
            get
            {
                if (EditStatus != EnumEditStatus.Add)
                    return false;
                if (AddEditCategory == EnumAddEditCategory.Phone)
                    return true;
                return false;
            }
        }
        public bool PhoneEditScreenVisible
        {
            get
            {
                if (EditStatus != EnumEditStatus.Edit)
                    return false;
                if (AddEditCategory == EnumAddEditCategory.Phone)
                    return true;
                return false;
            }
        }
        public bool ContactEditScreenVisible
        {
            get
            {
                if (EditStatus != EnumEditStatus.Edit)
                    return false;
                if (AddEditCategory == EnumAddEditCategory.Contact)
                    return true;
                return false;
            }
        }
        protected IBasicContactManagerUI BasicContactManagerUI { get; set; }
        protected CustomBasicList<C>? PrivateContactList;
        public CustomBasicList<C> GetContactList()
        {
            return PrivateContactList!;
        }
        protected CustomBasicList<P>? PrivatePhoneList;
        public CustomBasicList<P> GetPhoneList()
        {
            return PrivatePhoneList!;
        }
        public abstract void Init(); // this should be called after all the ui has been laid out.
        public CustomBasicList<EnumPhoneCategory> GetPhoneCategories()
        {
            return new CustomBasicList<EnumPhoneCategory>() { EnumPhoneCategory.HomeMainPhone, EnumPhoneCategory.RegularMobilePhone, EnumPhoneCategory.AlternatePhone, EnumPhoneCategory.HusbandMobilePhone, EnumPhoneCategory.WifeMobilePhone };
        }
        public CustomBasicList<EnumRelationship> GetRelationshipCategories()
        {
            return new CustomBasicList<EnumRelationship>() { EnumRelationship.Family, EnumRelationship.Business, EnumRelationship.CurrentChurch, EnumRelationship.OldCollege, EnumRelationship.Other, EnumRelationship.PreviousChurch };
        }
        private string _displayName = "";
        [Required(ErrorMessage = "Must Have A Display Name")]
        [StringLength(50)]
        [AutoClear]
        public string DisplayName
        {
            get
            {
                return _displayName;
            }

            set
            {
                if (SetProperty(ref _displayName, value) == true)
                {
                }
            }
        }
        private string _email = "";
        [StringLength(50)]
        [AutoClear]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (SetProperty(ref _email, value) == true)
                {
                }
            }
        }
        private string _streetAddress = "";
        [StringLength(100)]
        [AutoClear]
        public string StreetAddress
        {
            get
            {
                return _streetAddress;
            }

            set
            {
                if (SetProperty(ref _streetAddress, value) == true)
                {
                }
            }
        }
        private string _city = "";
        [StringLength(100)]
        [AutoClear]
        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                if (SetProperty(ref _city, value) == true)
                {
                }
            }
        }
        private string _state = "";
        [StringLength(50)]
        [AutoClear]
        public string State
        {
            get
            {
                return _state;
            }

            set
            {
                if (SetProperty(ref _state, value) == true)
                {
                }
            }
        }
        private string _zipCode = "";
        [StringLength(50)]
        [AutoClear]
        public string ZipCode
        {
            get
            {
                return _zipCode;
            }

            set
            {
                if (SetProperty(ref _zipCode, value) == true)
                {
                }
            }
        }
        private string _notes = "";
        [AutoClear]
        public string Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                if (SetProperty(ref _notes, value) == true)
                {
                }
            }
        }
        private string _drivingInstructions = "";
        [AutoClear]
        public string DrivingInstructions
        {
            get
            {
                return _drivingInstructions;
            }

            set
            {
                if (SetProperty(ref _drivingInstructions, value) == true)
                {
                }
            }
        }
        private EnumRelationship? _relationship = default;
        [AutoClear]
        public EnumRelationship? Relationship
        {
            get
            {
                return _relationship;
            }

            set
            {
                if (SetProperty(ref _relationship, value) == true)
                    // code to run
                    OnRelationShipChange();
            }
        }
        protected virtual void OnRelationShipChange() { }
        private string _phoneNumber = "";
        [Required(ErrorMessage = "Must Have A Phone Number")]
        [StringLength(50)]
        [AutoClear]
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }

            set
            {
                if (SetProperty(ref _phoneNumber, value) == true)
                {
                }
            }
        }
        private EnumPhoneCategory? _phoneCategory;

        public BaseContactManagerViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI, IBasicContactManagerUI customerui) : base(tempFocus, tempUI)
        {
            BasicContactManagerUI = customerui;
            RunFirst();
        }

        public EnumPhoneCategory? PhoneCategory
        {
            get
            {
                return _phoneCategory;
            }

            set
            {
                if (SetProperty(ref _phoneCategory, value) == true)
                {
                }
            }
        }
        protected override async Task ProcessEnter(object thisObj)
        {
            await Task.CompletedTask;
            EditStatus = EnumEditStatus.Add; // because you are adding an item
            AddEditCategory = (EnumAddEditCategory)thisObj; // i think

            if (AddEditCategory == EnumAddEditCategory.None)
                throw new Exception("Can't have none as a category");
            if (AddEditCategory == EnumAddEditCategory.Contact)
            {
                SelectedItem = default!; // i think
                ClearItems();
            }
            else
            {
                PhoneNumber = "";
                PhoneCategory = default; // i think
            }
            BasicContactManagerUI.FinishedStartEdit(); // i think this is fine.
        }
        protected virtual void ClearItems()
        {
            PhoneCategory = EnumPhoneCategory.HomeMainPhone; // default to home number
            if (AddEditCategory == EnumAddEditCategory.Phone)
            {
                PhoneNumber = "";
                return;
            }
            UseBlankString = true;
            this.AutoClearProperties(); //could be iffy.
            PhoneCategory = EnumPhoneCategory.HomeMainPhone;
        }
        protected override void AddOtherPropertiesAttributes(PropertyInfo thisProp)
        {
            if (thisProp.Name == nameof(Relationship))
            {
                if (Relationship.HasValue == false)
                    AddErrorMessage(thisProp, "Must Choose A Relationship Status");
            }
            if (thisProp.Name == nameof(PhoneCategory))
            {
                if (PhoneCategory.HasValue == false)
                    AddErrorMessage(thisProp, "Must Choose A Phone Category");
                if (AddEditCategory == EnumAddEditCategory.Phone && EditStatus == EnumEditStatus.Add)
                {
                    if (PhoneCategoryValidForAdding() == false)
                        AddErrorMessage(thisProp, "Cannot Choose A Phone Category You Already Have");
                }
            }
        }
        protected abstract bool PhoneCategoryValidForAdding();
        protected override bool OtherCustomAttributes(PropertyInfo s)
        {
            if (s.Name == nameof(Relationship) || s.Name == nameof(PhoneCategory))
                return true;// add this to the list afterall.
            return base.OtherCustomAttributes(s);
        }
        protected virtual bool IsAbleToAddPhone()
        {
            return true;
        }
        protected override bool CanAdd(object thisObj)
        {
            if (EditStatus != EnumEditStatus.Add)
                return false;// because the status is not even add
            if (AddEditCategory == EnumAddEditCategory.None)
                return false;// because we don't know which one.
            if (AddEditCategory == EnumAddEditCategory.Phone)
            {
                if (SelectedItem == null == true)
                    return false;// because if its a phone number, needs to have a selected item to associate with.
                if (IsAbleToAddPhone() == false)
                    return false;
            }
            return base.CanAdd(thisObj);
        }
        protected override bool CanSave(object thisObj)
        {
            if (EditStatus != EnumEditStatus.Edit)
                return false;// because must be actually editing something.
            if (SelectedItem == null == true)
                return false;// because you don't even have an item selected
            return base.CanSave(thisObj);
        }
        protected override bool CanEnter(object thisObj)
        {

            var thisItem = (EnumAddEditCategory)thisObj;
            if (thisItem == EnumAddEditCategory.Contact)
                return true;
            if (SelectedItem == null == true)
                return false;// because there is no contact to link up to.
            return IsAbleToAddPhone();
        }
        protected abstract void AddContact();
        protected abstract void AddPhone();
        protected override async Task ProcessAdd(object thisObj)
        {
            await Task.CompletedTask;
            if (AddEditCategory == EnumAddEditCategory.Contact)
            {
                AddContact();
                return;
            }
            if (AddEditCategory == EnumAddEditCategory.Phone)
            {
                if (SelectedItem == null == true)
                    throw new Exception("Should not have allowed the phone number to be added because no phone number associated with it");
                AddPhone();
                return;
            }
            throw new Exception("Needed to be phone or contact");
        }
        protected abstract void UpdateContact();
        protected abstract void UpdatePhone();
        protected override async Task ProcessSave(object thisObj)
        {
            await Task.CompletedTask;
            if (AddEditCategory == EnumAddEditCategory.Contact)
                UpdateContact();
            else if (AddEditCategory == EnumAddEditCategory.Phone)
                UpdatePhone();
            else
                throw new Exception("Needed to know what type to save");
        }
        public Command? StartEditContactCommand { get; set; }
        public Command<P>? StartEditPhoneCommand { get; set; } //if the parameter is different, then needs to use generics here too.
        protected abstract void PrivateStartEditContact(); // will probably will need to populate
        protected abstract void PrivateStartEditPhone(); // you have to send the parameter of the entire phone object to show which is being edited.
        public Command? ShowListCommand { get; set; }
        public Command? UndoAddEditPhoneCommand { get; set; }
        private void RunFirst()
        {
            StartEditContactCommand = new Command(s =>
            {
                AddEditCategory = EnumAddEditCategory.Contact;
                EditStatus = EnumEditStatus.Edit;
                PrivateStartEditContact();
            }, s =>
            {
                return !(SelectedItem == null);
            }, this);

            StartEditPhoneCommand = new Command<P>(x =>
            {
                AddEditCategory = EnumAddEditCategory.Phone;
                EditStatus = EnumEditStatus.Edit;
                PhoneChosen = x;
                PrivateStartEditPhone();

            }, x =>
            {
                if (EqualityComparer<P>.Default.Equals(x, default!))
                    return false;
                if (SelectedItem == null)
                    return false;
                return true;

            }, this);

            ShowListCommand = new Command(x =>
            {
                SelectedItem = default!;
                PhoneChosen = default!;
                ClearItems(); // i think
                EditStatus = EnumEditStatus.None;
                AddEditCategory = EnumAddEditCategory.None; // you are basically starting over in this case
                EnterCommand!.ReportCanExecuteChange();
                BasicContactManagerUI.BackToMain(); //i don't think i need async.  if i do, rethink.
            }, x => true, this);

            UndoAddEditPhoneCommand = new Command(x =>
            {
                EditStatus = EnumEditStatus.Edit;
                AddEditCategory = EnumAddEditCategory.Contact; // the only way you can add a number to a contact is if you are editing a contact
            }, x => true, this);
        }
        //public BaseContactManagerViewModel(IBasicContactManagerUI ThisTemp) { BasicContactManagerUI = ThisTemp; ThisMessage = ThisTemp; FirstControl = ThisTemp; RunFirst(); }
        //public BaseContactManagerViewModel()
        //{
        //    RunFirst();
        //}
    }
}
