using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.MVVMHelpers.InternalCommandList;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
    public class Command<T> : ICustomCommand, IAsyncCommand<T>
    {


        private readonly Func<T, Task>? _executeMethod;
        private readonly Func<T, bool> _canExecuteMethod;
        private readonly IErrorHandler _errorHandler;
        private readonly Action<T>? _oldcommand;

        public Command(Func<T, Task> action, Func<T, bool> _Cans, IErrorHandler errors) //decided to make it required for all. i think this is best to force good practices
        {
            _executeMethod = action;
            _canExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);
        }

        public Command(Action<T> action, Func<T, bool> _Cans, IErrorHandler errors) //i think its best to do it this way because i really don't want to have to do several waitblanks when its not really needed
        {
            _oldcommand = action;
            _canExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);
        }
        public void ReportCanExecuteChange() //i think still
        {
            CanExecuteChanged?.Invoke(this, new EventArgs()); // try this
        }
        public bool CanExecute(object parameter)
        {
            if (IsExecuting == true)
            {
                return false; //no matter what because there is a process already going on.
            }
            if (IsValidParameter(parameter) == false)
            {
                return false;
            }
            T newObj = (T)parameter;
            if (_canExecuteMethod == null)
            {
                return true;// if nothing is sent, implies it can be executed.  otherwise, needs logic to decide whether it can be executed. never makes sense to have a command that can never be executed
            }
            return _canExecuteMethod(newObj); // i think this is how that part is done.
        }
        static bool IsValidParameter(object o)
        {
            if (o != null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                return o is T;
            }
            var t = typeof(T);
            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }
            // Not a Nullable, if it's a value type then null is not valid
            return !t.GetTypeInfo().IsValueType;
        }
        private async void OldExecute(T parameter)
        {
            if (CanExecute(parameter!))
            {
                try
                {
                    IsExecuting = true;
                    _oldcommand!.Invoke(parameter);
                }
                catch (Exception ex)
                {
                    await _errorHandler.HandleErrorAsync(ex);
                }
                finally
                {
                    IsExecuting = false;
                }

            }
        }
        public void Execute(object parameter) //we decided to do it this way so for data entry, the same command can be used regardless of generics (?)
        {
            if (IsValidParameter(parameter) == false)
            {
                return; //can't execute because the parameter is not valid this time
            }
            if (_oldcommand != null)
            {
                {
                    OldExecute((T)parameter);
                    return;
                }
            }
            ExecuteAsync((T)parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter!))
            {
                try
                {
                    IsExecuting = true;
                    await _executeMethod!(parameter);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }

        public bool CanExecute(T args) //hopefully this won't cause any problems (?)
        {
            return CanExecute((object) args!);
        }

        public event EventHandler? CanExecuteChanged; //i am forced to make this public.
    }


    public class Command : ICustomCommand, IAsyncCommand
    {
        private readonly Func<object, Task>? _executeMethod;
        private readonly Func<object, bool> _canExecuteMethod;
        private readonly IErrorHandler _errorHandler;
        private readonly Action<object>? _oldcommand;
        public static bool CurrentlyExecuting()
        {
            return IsExecuting;
        }
        public static void NotifyAllCommands()
        {
            ReportAll();
        }
        public Command(Func<object, Task> action, Func<object, bool> cans, IErrorHandler errors)
        {
            _executeMethod = action;
            _canExecuteMethod = cans;
            _errorHandler = errors;
            AddCommand(this);
        }
        public Command(Action<object> action, Func<object, bool> _Cans, IErrorHandler errors) //i think its best to do it this way because i really don't want to have to do several waitblanks when its not really needed
        {
            _oldcommand = action;
            _canExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);
        }
        private async void OldExecute(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    IsExecuting = true;
                    _oldcommand!.Invoke(parameter);
                }
                catch (Exception ex)
                {
                    await _errorHandler.HandleErrorAsync(ex);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }
        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    IsExecuting = true;
                    await _executeMethod!(parameter);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            if (IsExecuting == true)
            {
                return false;
            }

            if (_canExecuteMethod == null == true)
            {
                return true;// if nothing is sent, implies it can be executed.  otherwise, needs logic to decide whether it can be executed. never makes sense to have a command that can never be executed
            }

            return _canExecuteMethod!(parameter); // i think this is how that part is done.
        }
        public void Execute(object parameter)
        {
            if (_oldcommand != null)
            {
                OldExecute(parameter);
                return;
            }
            ExecuteAsync(parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        public void ReportCanExecuteChange()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs()); // try this
        }

        bool IAsyncCommand.CanExecute()
        {
            return CanExecute(null!);
        }

        Task IAsyncCommand.ExecuteAsync()
        {
            return ExecuteAsync(null!);
        }

        public event EventHandler? CanExecuteChanged; //i am forced to make this public.
    }
}