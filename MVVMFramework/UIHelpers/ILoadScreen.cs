using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public interface ILoadScreen
    {
        Task LoadScreenAsync(object parentViewModel, IUIView parentViewScreen, object childViewModel, IUIView childViewScreen);
    }
}