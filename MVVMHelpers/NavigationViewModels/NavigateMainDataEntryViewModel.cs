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
using CommonBasicStandardLibraries.MVVMHelpers.SpecializedViewModels;
using Newtonsoft.Json;

namespace CommonBasicStandardLibraries.MVVMHelpers.NavigationViewModels
{
    public class NavigateMainDataEntryViewModel : DataEntryViewModel, IToggleVM
    {
        public NavigateMainDataEntryViewModel(IFocusOnFirst tempFocus, ISimpleUI tempUI) : base(tempFocus, tempUI)
        {
            Visible = true;
        }
        [JsonIgnore]
        public Func<Task>? SaveAction { get; set; } //this is standard just like a normal data entry has a save command

        private bool _visible;
        [JsonIgnore]
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (SetProperty(ref _visible, value))
                {
                    //can decide what to do when property changes
                }

            }
        }

        protected override Task ProcessSave(object thisObj)
        {

            return this.ProcessCommandAsync(SaveAction);

            //if (SaveAction == null)
            //{
            //    return Task.CompletedTask;
            //} //for data entry, would never use generics to pass parameters
            ////because you are entering something and the parent knows about the children.
            //return SaveAction!.Invoke();
        }
    }
}
