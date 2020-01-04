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
using Newtonsoft.Json;

namespace CommonBasicStandardLibraries.MVVMHelpers.NavigationViewModels
{
    public class NavigateMainSimpleViewModel : BaseViewModel, IToggleVM
    {
        public NavigateMainSimpleViewModel(ISimpleUI tempUI) : base(tempUI)
        {
            Visible = true; //the main one should start out with visible.
        }

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

    }
}
