using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    //i do like the idea that if somebody else needs to implement, they still can have shared code.
    public static class ConductorBehavior
    {
        public static Task CloseChildAsync(IHaveActiveViewModel conductor)
        {
            if (conductor.ActiveViewModel == null)
                throw new BasicBlankException("Cannot close the child because there was none to close");
            return conductor.ActiveViewModel.TryCloseAsync();
        }
        public static async Task<bool> TryActivateItemAsync(object parentViewModel, IUIView? parentViewScreen, object childViewModel)
        {
            if (UIPlatform.ViewLocator is null)
                return false;
            if (UIPlatform.ScreenLoader is null)
                return false;


            IUIView childview = await UIPlatform.ViewLocator.LocateViewAsync(childViewModel)!;
            if (childview == null)
                throw new BasicBlankException("No view was found when trying to active an item.  Rethink");
            if (parentViewScreen == null)
                throw new BasicBlankException("Has to have an active view in order to activate another screen");
            //we need a method that will do the rest of what is needed.  probably another interface.
            await UIPlatform.ScreenLoader.LoadScreenAsync(parentViewModel, parentViewScreen, childViewModel, childview);
            await childview.TryActivateAsync(); //to do extra things whatever is needed that is actually async.
            if (childViewModel is IScreen screens)
            {
                if (parentViewModel is IHaveActiveViewModel active)
                    active.ActiveViewModel = screens;
                await screens.ActivateAsync(childview);
            }


            //try risk not running on ui thread here too.
            //await Execute.OnUIThreadAsync(async () =>
            //{
                
            //});
            return true;
        }
    }
}