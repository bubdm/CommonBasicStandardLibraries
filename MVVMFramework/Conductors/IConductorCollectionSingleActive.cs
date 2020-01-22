using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;

namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    /// <summary>
    /// this is an interface for cases where you have a collection but only a single one is active at a time.
    /// </summary>
    public interface IConductorCollectionSingleActive<T> : IConductorSingleBegins<T>
    {
        //this is the screen that is currently active.  there can be several of them.
        //but only one at a time.

        IUIView? MainScreen { get; set; }
        bool CanCloseChild { get; }
    }
}