using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using System;
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
                if (_changing)
                {
                    return; //because something else is being opened instead.
                }
                _canCloseChild = value;
                ChangeCloseChild();
                NotifyOfCanExecuteChange(nameof(CanCloseChild));
            }
        }

        protected bool IsOpened(Type type)
        {
            if (ActiveViewModel == null)
            {
                return false;
            }
            Type mains = ActiveViewModel.GetType();
            return mains == type;
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
        public Task CloseChildAsync()
        {
            return ConductorBehavior.CloseChildAsync(this);
        }

        Task IConductorScreenLoader<T>.LoadScreenAsync(T viewModel)
        {
            return LoadScreenAsync(viewModel);
        }
        private bool _changing;
        //decided to make it virtual so i can do other things too.
        protected virtual async Task LoadScreenAsync(T viewModel)
        {
            if (ActiveViewModel != null)
            {
                _changing = true;
                await CloseChildAsync(); //i think
            }
            if (await ConductorBehavior.TryActivateItemAsync(this, _mainScreen, viewModel!))
            {
                if (ActiveViewModel == null)
                    throw new BasicBlankException("Failed to set the active item.  I think this will cause problems.  Rethink");
                else
                {
                    CanCloseChild = true;
                    ActiveViewModel.Closing = () => { ActiveViewModel = null; CanCloseChild = false; };
                }
            }
            _changing = false;
        }
    }
}