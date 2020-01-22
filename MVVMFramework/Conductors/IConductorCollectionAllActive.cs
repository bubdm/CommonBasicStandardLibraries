using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    /// <summary>
    /// this are conductors that has several screens and can be opened at the same time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConductorCollectionAllActive<T> : IConductorScreenLoader<T>
    {
        IUIView? MainScreen { get; set; }
        Task CloseSpecificChildAsync(IScreen childviewModel); //this is the view model being closed.
    }
}