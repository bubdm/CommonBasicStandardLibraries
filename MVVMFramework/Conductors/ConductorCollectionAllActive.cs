using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    public abstract class ConductorCollectionAllActive<T> : Screen, IConductorCollectionAllActive<T>
    {

        //can't be active view model because there can be more than one obviously.
        //so the inherited classes has to specify them
        //i do like the idea the sub classes can use the aggregator
        protected IEventAggregator Aggregator;

        //private readonly IEventAggregator _trackerEventAggregor;
        /// <summary>
        /// this is used to subscribe to messages from child view models.
        /// </summary>
        protected virtual void SetSubscribers()
        {
            Aggregator.Subscribe(this); //that is default.  could eventually do others.
        }
        public ConductorCollectionAllActive()
        {
            Aggregator = cons!.Resolve<IEventAggregator>();
            SetSubscribers();
        }

        public IUIView? MainScreen { get; set; }
        IUIView? IConductorCollectionAllActive<T>.MainScreen { get; set; }

        protected override Task ActivateAsync(IUIView view)
        {
            MainScreen = view;
            return base.ActivateAsync(view);
        }

        protected Task CloseSpecificChildAsync(IScreen childViewModel)
        {
            return childViewModel.TryCloseAsync();
        }

        Task IConductorCollectionAllActive<T>.CloseSpecificChildAsync(IScreen childviewModel)
        {
            return CloseSpecificChildAsync(childviewModel);
        }

        protected Task LoadScreenAsync(T viewModel)
        {
            return ConductorBehavior.TryActivateItemAsync(this, MainScreen, viewModel!);
        }
        
        Task IConductorScreenLoader<T>.LoadScreenAsync(T viewModel)
        {
            return LoadScreenAsync(viewModel);
        }
    }
}
