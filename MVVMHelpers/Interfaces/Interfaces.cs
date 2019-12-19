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
    public interface IAsyncCommand
    {
        //for now, just certain things.  can add more as needed anyways.
        bool CanExecute(); //the non generics one should accept no parameters
        Task ExecuteAsync();
    }
    public interface IAsyncCommand<T>
    {
        bool CanExecute(T args);
        Task ExecuteAsync(T args);
    }
    public interface IToggleVM
    {
        bool Visible { get; set; }
    }
    public interface INavigateVM : IToggleVM
    {
        Command? BackCommand { get; set; }
        Func<Task>? BackAction { get; set; }
    }
    //public interface INavigateVM<T> : IToggleVM
    //{
    //    Command<T>? BackCommand { get; set; }
    //    Func<T, Task>? BackAction { get; set; }
    //}
}