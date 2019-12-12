using System;
using System.Threading.Tasks;
using System.Windows.Input;
namespace CommonBasicStandardLibraries.MVVMHelpers.Interfaces
{
    //this is used so if i use in unit testing, then it can at least show a messagebox.
    public interface IMessage //decided to do this so something can just display messages but do nothing else
    {
        Task ShowMessageBox(string message);
    }
    public interface IError
    {
        void ShowError(string message);
    }
    public interface IClose
    {
        void CloseProgram();
    }
    public interface ISimpleUI : IMessage, IError, IClose { }
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
}