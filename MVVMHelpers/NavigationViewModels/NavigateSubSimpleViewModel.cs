﻿using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMHelpers.NavigationViewModels
{
    public abstract class NavigateSubSimpleViewModel : NavigateMainSimpleViewModel, INavigateVM
    {
        public NavigateSubSimpleViewModel(ISimpleUI tempUI) : base(tempUI)
        {
            CreateBackButton(); //this way if you want to have another implementation to the error info, that would be possible.
            Visible = false; //has to start out false because its a sub one, not the main one.
        }

        protected virtual void CreateBackButton()
        {
            this.CreateBackCommand(this); //this simple.
        }

        public Command? BackCommand { get; set; }
        public Func<Task>? BackAction { get; set; }
    }
}
