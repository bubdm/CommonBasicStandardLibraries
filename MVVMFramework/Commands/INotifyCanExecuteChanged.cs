using CommonBasicStandardLibraries.MVVMFramework.EventArgClasses;
namespace CommonBasicStandardLibraries.MVVMFramework.Commands
{
    public interface INotifyCanExecuteChanged
    {
        event CanExecuteChangedEventHandler CanExecuteChanged;
    }
}