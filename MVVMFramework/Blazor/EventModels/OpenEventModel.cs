using CommonBasicStandardLibraries.MVVMFramework.Blazor.ViewModels;
namespace CommonBasicStandardLibraries.MVVMFramework.Blazor.EventModels
{
    public class OpenEventModel
    {
        public IBlazorScreen ViewModelUsed { get; set; }
        public OpenEventModel(IBlazorScreen viewModelUsed)
        {
            ViewModelUsed = viewModelUsed;
        }
    }
}