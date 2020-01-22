using System.Windows.Input;
namespace CommonBasicStandardLibraries.MVVMFramework.Commands
{
    public interface ICustomCommand : ICommand
    {
        void ReportCanExecuteChange();
    }
}