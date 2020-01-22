using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    public class ConductorSingle<T> : Screen, IConductorSingleScreen<T>
    {
        private bool _canCloseChild = false;
        public bool CanCloseChild
        {
            get
            {
                return _canCloseChild;
            }
            private set
            {
                _canCloseChild = value;
                NotifyOfCanExecuteChange(nameof(CanCloseChild));
                NotifyOfCanExecuteChange(nameof(CanOpenChild));
            }
        }
        private IUIView? _mainScreen;
        public bool CanOpenChild { get => !CanCloseChild; }
        IUIView? IConductorCollectionSingleActive<T>.MainScreen { get => _mainScreen; set => _mainScreen = value; }
        bool IConductorCollectionSingleActive<T>.CanCloseChild { get; }
        bool IConductorSingleScreen<T>.CanOpenChild { get; }
        public IScreen? ActiveViewModel { get; set; }

        public Task CloseChildAsync()
        {
            return ConductorBehavior.CloseChildAsync(this);
        }

        protected override Task ActivateAsync(IUIView view)
        {
            _mainScreen = view;
            return base.ActivateAsync(view);
        }
        protected async Task LoadScreenAsync(T viewModel)
        {
            if (ActiveViewModel != null)
                throw new BasicBlankException("Failed to clear out the active view model before loading another screen.");
            if (await ConductorBehavior.TryActivateItemAsync(this, _mainScreen, viewModel!))
                if (ActiveViewModel == null)
                    throw new BasicBlankException("Failed to set the active item.  I think this will cause problems.  Rethink");
                else
                {
                    CanCloseChild = true;
                    ActiveViewModel.Closing = () => { ActiveViewModel = null; CanCloseChild = false; };
                }
        }

        Task IConductorScreenLoader<T>.LoadScreenAsync(T viewModel)
        {
            return LoadScreenAsync(viewModel);
        }
    }
}
