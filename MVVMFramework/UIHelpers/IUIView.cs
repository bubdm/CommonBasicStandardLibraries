using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public interface IUIView
    {
        /// <summary>
        /// this is needed so if they need to unsubscribe, they can do so.
        /// plus they can do any clean up work required
        /// </summary>
        /// <returns></returns>
        Task TryCloseAsync();

        Task TryActivateAsync();
        object DataContext { set; get; } //decided to be both so that would already be implemented.  hopefully that works out.
    }
}
