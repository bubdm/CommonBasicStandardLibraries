using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.Blazor.ViewModels
{
    public abstract class BlazorConductorViewModel : BlazorScreenViewModel
    {
        //looks like can't inherit from any screen.
        //because you can't even open a screen.
        protected Task LoadScreenAsync(IBlazorScreen viewModel)
        {
            return viewModel.ActivateAsync(); //hopefully this simple.
        }

        virtual protected Task CloseSpecificChildAsync(IBlazorScreen viewModel) //looke like this has to be virtual afterall.
        {
            return viewModel.TryCloseAsync();
        }
    }
}