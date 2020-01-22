using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    public interface IScreen : IParentContainer, IClearable, IMappable //i think that anything that implements should be clearable
    {
        /// <summary>
        /// this will take the view that was created and then will set the object so it can use to later close
        /// also, can run any other process needed after it gets the view
        /// probably can't unbox since you would not have what is needed though.
        /// something can set other things to the view models before it activates it.
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        Task ActivateAsync(IUIView view);
        //i think it needs to close as well.
        /// <summary>
        /// this is the control that can remove and add.  so when closing out of the screen, its able to close out.
        /// </summary>
        Task TryCloseAsync();
        public Action Closing { get; set; }
        public bool CanClose() => true;
        public void ExitApp(); //decided against default.  since i would have no help to override.

    }
}