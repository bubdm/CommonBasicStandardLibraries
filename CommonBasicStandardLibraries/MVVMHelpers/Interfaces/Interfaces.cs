using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonBasicStandardLibraries.MVVMHelpers.Interfaces
{
    //this is used so if i use in unit testing, then it can at least show a messagebox.
    public interface IMessage //decided to do this so something can just display messages but do nothing else
    {
        Task ShowMessageBox(string Message);
    }
    public interface IError
    {
        void ShowError(string Message);
    }
    public interface IClose
    {
        void CloseProgram();
    }
    public interface ISimpleUI : IMessage, IError, IClose
    {
        //Task ShowMessageBox(string Message);

        //void ShowError(string Message);
        //void CloseProgram();
        //did it this way, so a person can choose just one or all 3.
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
        void BackToMain(); //instead of events.
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
