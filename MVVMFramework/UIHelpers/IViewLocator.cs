using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    /// <summary>
    /// the view locator needs to not only locate the view; but needs to run methods to bind whatever needs to be bound.
    /// finally return the view.  i think something else is responsible for figuring out what to do with the new view.
    /// most likely the view would already be created by the time the function ends.
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel">this is the view model that is requesting the view that goes along with it.</param>
        /// <returns></returns>
        Task<IUIView>? LocateViewAsync(object viewModel); //can return null if nothing is found.
    }
}
