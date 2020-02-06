using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.Conductors;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMFramework.ViewModels
{
    /// <summary>
    /// this is used in cases where you want the view model to be able to close out of a view.
    /// if you are using single view model, then since boot straper would connect the 2
    /// then you don't have to do anything else.
    /// you can even inherit from viewmodelbase and it would technically work.
    /// this is not intended to open another screen.
    /// conductor is intended for that purpose.
    /// 
    /// </summary>
    public abstract class Screen : ViewModelBase, IScreen
    {

        //protected IUIView? ActiveView { get; set; } //i don't think its necessary to notify property change.  if i am wrong, can change it.
        /// <summary>
        /// this is the main container that hosts the view.
        /// an example is for the person screen, the parent is the control holding the screen.
        /// </summary>
        public IContentControl? ParentContainer { get; set; }
        Action IScreen.Closing { get; set; } = () => { };

        private IUIView? _view;
        protected void FocusOnFirst()
        {
            IEventAggregator aggregator = cons!.Resolve<IEventAggregator>();
            aggregator.FocusOnFirst();
        }

        /// <summary>
        /// Allows the developer to add custom handling of named elements which were not matched by any default conventions.
        /// </summary>


        async Task IScreen.ActivateAsync(IUIView view)
        {
            _view = view;
            await ActivateAsync(view);
            //ActiveView = view;
        }

        protected virtual async Task ActivateAsync(IUIView view)
        {
            await view.TryActivateAsync(); //hopefully no need to do on ui thread (?)

            //await Execute.OnUIThreadAsync(() => view.TryActivateAsync()); //i think the view model should initiate it.
            //this is the final steps the ui needs to run.
            await ActivateAsync();

        }

        protected virtual Task ActivateAsync()
        {
            return Task.CompletedTask;
        }
        //this needed to be virtual so in cases where you have to close, you can clear out of the resources as well.
        protected virtual async Task TryCloseAsync()
        {
            
            //think about how its going to close.
            if (ParentContainer == null)
            {
                return;
            }
            ParentContainer.Close();
            IScreen screen = this;
            screen.Closing();
            if (_view != null)
            {
                await CloseViewAsync();
            }
            //hopefully its this simple.
        }

        protected async Task CloseViewAsync()
        {
            if (_view == null)
            {
                throw new BasicBlankException("There was no view.  If needs to ignore, then take out error");
            }
            await _view.TryCloseAsync();
        }

        public virtual async Task CancelAsync()
        {
            //this will always cancel.
            //i do like the idea of being able to send message automatically.  will send itself.
            //because most of the time, if the screen is being closed out, it needs to send message so parent can do something.
            //that will only happen when its done this way.  does not need to be implemented to be iscreen.
            await TryCloseAsync();
            IEventAggregator aggregator = cons!.Resolve<IEventAggregator>();
            //may require rethinking.
            await aggregator.PublishAsync(this);
        }

        Task IScreen.TryCloseAsync()
        {
            return TryCloseAsync();
        }

        public void ExitApp()
        {
            UIPlatform.ExitApp();
        }
    }
}
