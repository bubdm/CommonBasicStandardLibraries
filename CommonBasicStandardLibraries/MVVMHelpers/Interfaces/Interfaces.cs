using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonBasicStandardLibraries.MVVMHelpers.Interfaces
{
    public interface ISimpleUI
    {
        Task ShowMessageBox(string Message);

        void ShowError(string Message);
        void CloseProgram();

    }

    public interface IFocusOnFirst : ISimpleUI
    {
        void FocusOnFirstControl();
    }

    public interface IBasicContactManagerUI : IFocusOnFirst
    {
        void SaveChanges();
        void ContactListChanged();
        void FinishedStartEdit();
        void FinishedEndEdit();
    }

	public interface IErrorHandler
	{
		Task HandleErrorAsync(Exception ex); //i think this is even better.
	}

	public interface ICustomCommand : ICommand
	{
		void ReportCanExecuteChange(); //looks like i am forced to do it this way.
	}

	//decided to attempt to not do the new interface for the iasync though.


}
