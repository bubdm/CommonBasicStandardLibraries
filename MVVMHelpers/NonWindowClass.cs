using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
    /// <summary>
    /// this class is intended for cases where you don't do anything but want to use the basic view models
    /// </summary>
    public class NonWindowClass : ISimpleUI, IFocusOnFirst
    {
        void IClose.CloseProgram()
        {

        }

        void IFocusOnFirst.FocusOnFirstControl()
        {

        }

        void IError.ShowError(string message)
        {
            throw new BasicBlankException(message);
        }

        Task IMessage.ShowMessageBox(string message)
        {
            return Task.CompletedTask;
        }
    }
}
