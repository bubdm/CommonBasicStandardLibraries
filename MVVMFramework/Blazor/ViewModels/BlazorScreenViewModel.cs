using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.Blazor.EventModels;
using CommonBasicStandardLibraries.MVVMFramework.Blazor.Helpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMFramework.Blazor.ViewModels
{
    public abstract class BlazorScreenViewModel : ViewModelBase, IBlazorScreen, IHandle<AskEventModel>
    {
        //this is for blazor.  however, good news is does not have to reference any blazor specific stuff though.
        //since you can't actually open the screen since the way navigation works.
        Action IBlazorScreen.Closing { get; set; } = () => { };
        IEventAggregator IBlazorScreen.Aggregator => Aggregator;

        protected IEventAggregator Aggregator; //i think this needs it.

        public BlazorScreenViewModel()
        {
            Aggregator = cons!.Resolve<IEventAggregator>();
            string name = GetType().Name;
            Aggregator.Subscribe(this, name);
            Aggregator.Subscribe(this); //just in case no names are needed either.  so its done automatically now.
        }

        Task IBlazorScreen.ActivateAsync()
        {
            this.OpenView();
            return ActivateAsync();
        }

        protected virtual Task ActivateAsync()
        {
            return Task.CompletedTask;
        }

        Task IBlazorScreen.TryCloseAsync()
        {
            return TryCloseAsync();
        }
        protected virtual Task TryCloseAsync()
        {
            this.CloseView();
            Aggregator.Unsubscribe(this, GetType().Name); //can't be subscribed to this anymore.
            Aggregator.Unsubscribe(this);
            return Task.CompletedTask;
        }

        void IHandle<AskEventModel>.Handle(AskEventModel message)
        {
            this.OpenView();
        }
    }
}