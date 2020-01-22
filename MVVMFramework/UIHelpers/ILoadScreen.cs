using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public interface ILoadScreen
    {
        Task LoadScreenAsync(object parentViewModel, IUIView parentViewScreen, object childViewModel, IUIView childViewScreen);
        //that way the platform can do what is required.  may even be the boot strapper class.
        //if so, then bootstrapper will implement this.
    }
}