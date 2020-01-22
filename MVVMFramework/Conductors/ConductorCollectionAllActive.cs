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

        private readonly IEventAggregator _trackerEventAggregor;
        /// <summary>
        /// this is used to subscribe to messages from child view models.
        /// </summary>
        /// <param name="aggregator"></param>
        protected virtual void SetSubscribers(IEventAggregator aggregator)
        {
            aggregator.Subscribe(this); //that is default.  could eventually do others.
        }
        public ConductorCollectionAllActive()
        {
            _trackerEventAggregor = cons!.Resolve<IEventAggregator>();
            SetSubscribers(_trackerEventAggregor);
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
