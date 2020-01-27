using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.Conductors
{
    public class ConductorCollectionSingleActive<T> : Screen, IConductorCollectionSingleActive<T>
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
                ChangeCloseChild();
                NotifyOfCanExecuteChange(nameof(CanCloseChild));
            }
        }

        protected virtual void ChangeCloseChild() { }

        private IUIView? _mainScreen;
        IUIView? IConductorCollectionSingleActive<T>.MainScreen { get => _mainScreen; set => _mainScreen = value; }
        //this needs to be public so it can be hooked for data binding.
        public IScreen? ActiveViewModel { get; set; }
        protected override Task ActivateAsync(IUIView view)
        {
            _mainScreen = view;
            return base.ActivateAsync(view);
        }
        //may need to do other things like on reminders.
        public virtual Task CloseChildAsync()
        {
            return ConductorBehavior.CloseChildAsync(this);
        }

        Task IConductorScreenLoader<T>.LoadScreenAsync(T viewModel)
        {
            return LoadScreenAsync(viewModel);
        }
        protected async Task LoadScreenAsync(T viewModel)
        {
            if (ActiveViewModel != null)
                await CloseChildAsync(); //i think
            if (await ConductorBehavior.TryActivateItemAsync(this, _mainScreen, viewModel!))
                if (ActiveViewModel == null)
                    throw new BasicBlankException("Failed to set the active item.  I think this will cause problems.  Rethink");
                else
                {
                    CanCloseChild = true;
                    ActiveViewModel.Closing = () => { ActiveViewModel = null; CanCloseChild = false; };
                }
        }
    }
}