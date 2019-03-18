using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels
{
    public abstract class BaseContactManagerViewModel<C, P> : AddEditViewModel// because we have a phone and a desktop version of it.
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





        private EnumEditStatus _EditStatus = EnumEditStatus.None;
        public EnumEditStatus EditStatus
        {
            get
            {
                return _EditStatus;
            }

            set
            {
                if (SetProperty(ref _EditStatus, value) == true)
                    // code to run
                    ChangeScreens();
            }
        }




        private EnumAddEditCategory _AddEditCategory = EnumAddEditCategory.None;
        public EnumAddEditCategory AddEditCategory
        {
            get
            {
                return _AddEditCategory;
            }

            set
            {
                if (SetProperty(ref _AddEditCategory, value) == true)
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

        private C _SelectedItem;
        public C SelectedItem //we could decide on interfaces but not sure now though.
        {
            get
            {
                return _SelectedItem;
            }

            set
            {
                if (SetProperty(ref _SelectedItem, value) == true)
                {
                    EnterCommand.ReportCanExecuteChange();
                    StartEditContactCommand.ReportCanExecuteChange(); // not sure why it worked on desktop.
                    OnSelectItemChange();
                }
            }
        }

        protected virtual void OnSelectItemChange()
        {
        }

        protected P PhoneChosen;




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

        protected List<C> PrivateContactList;

        public List<C> GetContactList()
        {
            return PrivateContactList;
        }

        protected List<P> PrivatePhoneList;

        public List<P> GetPhoneList()
        {
            return PrivatePhoneList;
        }


        public abstract void Init(); // this should be called after all the ui has been laid out.


        public List<EnumPhoneCategory> GetPhoneCategories()
        {
            return new List<EnumPhoneCategory>() { EnumPhoneCategory.HomeMainPhone, EnumPhoneCategory.RegularMobilePhone, EnumPhoneCategory.AlternatePhone, EnumPhoneCategory.HusbandMobilePhone, EnumPhoneCategory.WifeMobilePhone };
        }

        public List<EnumRelationship> GetRelationshipCategories()
        {
            return new List<EnumRelationship>() { EnumRelationship.Family, EnumRelationship.Business, EnumRelationship.CurrentChurch, EnumRelationship.OldCollege, EnumRelationship.Other, EnumRelationship.PreviousChurch };
        }


        // #End Region

        // i think will be here for the enums

        private string _DisplayName;
        [Required(ErrorMessage = "Must Have A Display Name")]
        [StringLength(50)]
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }

            set
            {
                if (SetProperty(ref _DisplayName, value) == true)
                {
                }
            }
        }


        private string _Email;
        [StringLength(50)]
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                if (SetProperty(ref _Email, value) == true)
                {
                }
            }
        }

        private string _StreetAddress;
        [StringLength(100)]
        public string StreetAddress
        {
            get
            {
                return _StreetAddress;
            }

            set
            {
                if (SetProperty(ref _StreetAddress, value) == true)
                {
                }
            }
        }


        private string _City;
        [StringLength(100)]
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                if (SetProperty(ref _City, value) == true)
                {
                }
            }
        }

        private string _State;
        [StringLength(50)]
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                if (SetProperty(ref _State, value) == true)
                {
                }
            }
        }

        private string _ZipCode;
        [StringLength(50)]
        public string ZipCode
        {
            get
            {
                return _ZipCode;
            }

            set
            {
                if (SetProperty(ref _ZipCode, value) == true)
                {
                }
            }
        }


        private string _Notes;
        public string Notes
        {
            get
            {
                return _Notes;
            }

            set
            {
                if (SetProperty(ref _Notes, value) == true)
                {
                }
            }
        }


        private string _DrivingInstructions;
        public string DrivingInstructions
        {
            get
            {
                return _DrivingInstructions;
            }

            set
            {
                if (SetProperty(ref _DrivingInstructions, value) == true)
                {
                }
            }
        }

        private EnumRelationship? _Relationship = default;
        public EnumRelationship? Relationship
        {
            get
            {
                return _Relationship;
            }

            set
            {
                if (SetProperty(ref _Relationship, value) == true)
                    // code to run
                    OnRelationShipChange();
            }
        }

        protected virtual void OnRelationShipChange()
        {
        }

        private string _PhoneNumber;
        [Required(ErrorMessage = "Must Have A Phone Number")]
        [StringLength(50)]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }

            set
            {
                if (SetProperty(ref _PhoneNumber, value) == true)
                {
                }
            }
        }

        private EnumPhoneCategory? _PhoneCategory;
        public EnumPhoneCategory? PhoneCategory
        {
            get
            {
                return _PhoneCategory;
            }

            set
            {
                if (SetProperty(ref _PhoneCategory, value) == true)
                {
                }
            }
        }

        // normal save is for saving the normal contact information.

        protected override async Task ProcessEnter(object ThisObj)
        {
			await WaitBlank();
            EditStatus = EnumEditStatus.Add; // because you are adding an item
            AddEditCategory = (EnumAddEditCategory)ThisObj; // i think

            if (AddEditCategory == EnumAddEditCategory.None)
                throw new Exception("Can't have none as a category");
            if (AddEditCategory == EnumAddEditCategory.Contact)
            {
                SelectedItem = default; // i think
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
            // Stop
            PhoneCategory = EnumPhoneCategory.HomeMainPhone; // default to home number
            if (AddEditCategory == EnumAddEditCategory.Phone)
            {
                PhoneNumber = "";
                return;
            }
            UseBlankString = true;
            ClearPropertiesWithAttributes();
            Notes = "";
            DrivingInstructions = ""; // since no attributes, i have to do manually.
            Relationship = default;
            PhoneCategory = EnumPhoneCategory.HomeMainPhone;
        }


        protected override void AddOtherPropertiesAttributes(PropertyInfo ThisProp)
        {
            if (ThisProp.Name == nameof(Relationship))
            {
                if (Relationship.HasValue == false)
                    // Stop 'another problem here
                    AddErrorMessage(ThisProp, "Must Choose A Relationship Status");
            }
            if (ThisProp.Name == nameof(PhoneCategory))
            {
                if (PhoneCategory.HasValue == false)
                    AddErrorMessage(ThisProp, "Must Choose A Phone Category");
                if (AddEditCategory == EnumAddEditCategory.Phone && EditStatus == EnumEditStatus.Add)
                {
                    if (PhoneCategoryValidForAdding() == false)
                        AddErrorMessage(ThisProp, "Cannot Choose A Phone Category You Already Have");
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


        protected override bool CanAdd(object ThisObj)
        {
			//if its busy, then it won't make it this far anyways.
            //if (IsBusy == true)
            //    return false;// just make sure you can't do if its busy at this moment.
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
            return base.CanAdd(ThisObj);
        }

        protected override bool CanSave(object ThisObj)
        {
            if (EditStatus != EnumEditStatus.Edit)
                return false;// because must be actually editing something.
            if (SelectedItem == null == true)
                return false;// because you don't even have an item selected
            return base.CanSave(ThisObj);
        }

        protected override bool CanEnter(object ThisObj)
        {

            var ThisItem = (EnumAddEditCategory)ThisObj;
            if (ThisItem == EnumAddEditCategory.Contact)
                return true;
            if (SelectedItem == null == true)
                return false;// because there is no contact to link up to.
            return IsAbleToAddPhone();
        }

        protected abstract void AddContact();
        protected abstract void AddPhone();


        protected override async Task ProcessAdd(object ThisObj)
        {
			await WaitBlank();
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

        protected override async Task ProcessSave(object ThisObj)
        {
			await WaitBlank();
            //IsBusy = true;
			//will no longer set it because its done on the commands.  all commands for now will be marked busy.
            if (AddEditCategory == EnumAddEditCategory.Contact)
                UpdateContact();
            else if (AddEditCategory == EnumAddEditCategory.Phone)
                UpdatePhone();
            else
                throw new Exception("Needed to know what type to save");
        }

        public Command StartEditContactCommand { get; set; }

        public Command<P> StartEditPhoneCommand { get; set; } //if the parameter is different, then needs to use generics here too.

        protected abstract void PrivateStartEditContact(); // will probably will need to populate

        protected abstract void PrivateStartEditPhone(); // you have to send the parameter of the entire phone object to show which is being edited.

        public Command ShowListCommand { get; set; }

        public Command UndoAddEditPhoneCommand { get; set; }
        //public event BackToMainEventHandler BackToMain;

        //public delegate void BackToMainEventHandler();

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
                if (EqualityComparer<P>.Default.Equals(x, default))
                    return false;

                if (SelectedItem == null)
                    return false;
                return true;

            }, this);

            ShowListCommand = new Command(x =>
            {
                SelectedItem = default;
                PhoneChosen = default;
                ClearItems(); // i think
                EditStatus = EnumEditStatus.None;
                AddEditCategory = EnumAddEditCategory.None; // you are basically starting over in this case
                EnterCommand.ReportCanExecuteChange();
                BasicContactManagerUI.BackToMain(); //i don't think i need async.  if i do, rethink.
                //BackToMain?.Invoke(); // its up to each ui to decide what to do about this.
            }, x => true, this);

            UndoAddEditPhoneCommand = new Command(x =>
            {
                EditStatus = EnumEditStatus.Edit;
                AddEditCategory = EnumAddEditCategory.Contact; // the only way you can add a number to a contact is if you are editing a contact
            }, x => true, this);
        }
        public BaseContactManagerViewModel(IBasicContactManagerUI ThisTemp) { BasicContactManagerUI = ThisTemp; ThisMessage = ThisTemp; FirstControl = ThisTemp; RunFirst(); }
        public BaseContactManagerViewModel()
        {
            RunFirst();
        }
    }
}
