using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.MVVMHelpers.InternalCommandList;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
    public class Command<T> : ICustomCommand
    {


        private readonly Func<T, Task> ExecuteMethod;
        private readonly Func<T, bool> CanExecuteMethod;
        private readonly IErrorHandler _errorHandler;
        private readonly Action<T> oldcommand;

        //you can't invoke another action if one is not processed i think.

        //private static CustomBasicList<Command<object>> SavedList = new CustomBasicList<Command<object>>();


        //private static bool _isExecuting; //will try to do shared (static).  if that needs to change, rethink.


        //was going to try for asyn ones.
        //however, its probably not worth the risk.  i can always put async in there anyways.

        //public Command(Action execute) : this(o =>
        //{
        //    execute();
        //})
        //{
        //    if (execute == null)
        //        throw new ArgumentNullException(nameof(execute));
        //}


        //public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        //{
        //    if (execute == null)
        //        throw new ArgumentNullException(nameof(execute));
        //    if (canExecute == null)
        //        throw new ArgumentNullException(nameof(canExecute));
        //}

        public Command(Func<T, Task> action, Func<T, bool> _Cans, IErrorHandler errors) //decided to make it required for all. i think this is best to force good practices
        {
            ExecuteMethod = action;
            CanExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);
        }

        public Command(Action<T> action, Func<T, bool> _Cans, IErrorHandler errors) //i think its best to do it this way because i really don't want to have to do several waitblanks when its not really needed
        {
            oldcommand = action;
            CanExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);
        }

        //

        //public Command(Action<T> action)
        //{
        //    ExecuteMethod = action;
        //}



        // Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged


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

            T NewObj = (T)parameter;

            if (CanExecuteMethod == null == true)
            {
                return true;// if nothing is sent, implies it can be executed.  otherwise, needs logic to decide whether it can be executed. never makes sense to have a command that can never be executed
            }

            return CanExecuteMethod(NewObj); // i think this is how that part is done.
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
            if (CanExecute(parameter))
            {
                try
                {
                    IsExecuting = true;
                    oldcommand.Invoke(parameter);
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
            if (oldcommand != null)
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
            if (CanExecute(parameter))
            {
                try
                {
                    IsExecuting = true;
                    await ExecuteMethod(parameter);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
            //if several share one class, the old would work.
            //however, if there are other processes, then they should not work no matter what while this is going on.
            //this means if you are not allowed to do something because of other reasons, you have to handle it.   no longer using IsBusy.
            //should now while processes are running don't allow any to run.
        }


        // this is what c# had.

        // Public Sub ChangeCanExecute()
        // Dim changed As EventHandler = PrivateCanExecuteChanged
        // changed?.Invoke(Me, EventArgs.Empty)
        // End Sub

        public event EventHandler CanExecuteChanged; //i am forced to make this public.

    }


    public class Command : ICustomCommand
    {
        private readonly Func<object, Task> ExecuteMethod;
        private readonly Func<object, bool> CanExecuteMethod;
        private readonly IErrorHandler _errorHandler;
        private readonly Action<object> oldcommand;

        public static bool CurrentlyExecuting()
        {
            return IsExecuting;
        }

        public static void NotifyAllCommands()
        {
            ReportAll();
        }

        //we managed without the weakreferences.   we should be able to manage again.

        public Command(Func<object, Task> action, Func<object, bool> _Cans, IErrorHandler errors)
        {
            ExecuteMethod = action;
            CanExecuteMethod = _Cans;
            _errorHandler = errors;
            AddCommand(this);

        }

        public Command(Action<object> action, Func<object, bool> _Cans, IErrorHandler errors) //i think its best to do it this way because i really don't want to have to do several waitblanks when its not really needed
        {
            oldcommand = action;
            CanExecuteMethod = _Cans;
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
                    oldcommand.Invoke(parameter);
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
                    await ExecuteMethod(parameter);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
            //if several share one class, the old would work.
            //however, if there are other processes, then they should not work no matter what while this is going on.
            //this means if you are not allowed to do something because of other reasons, you have to handle it.   no longer using IsBusy.
            //should now while processes are running don't allow any to run.
        }



        public bool CanExecute(object parameter)
        {
            if (IsExecuting == true)
            {
                return false;
            }

            if (CanExecuteMethod == null == true)
            {
                return true;// if nothing is sent, implies it can be executed.  otherwise, needs logic to decide whether it can be executed. never makes sense to have a command that can never be executed
            }

            return CanExecuteMethod(parameter); // i think this is how that part is done.
        }


        // c# shows as event handler.  in vb.net, its just event



        public void Execute(object parameter)
        {
            if (oldcommand != null)
            {
                OldExecute(parameter);
                return;
            }

            ExecuteAsync(parameter).FireAndForgetSafeAsync(_errorHandler);

            // not sure about this part
            // _action(Of Object)()
            //ExecuteMethod(parameter);
        }

        // Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged


        public void ReportCanExecuteChange()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs()); // try this
        }

        // this is what c# had.

        // Public Sub ChangeCanExecute()
        // Dim changed As EventHandler = PrivateCanExecuteChanged
        // changed?.Invoke(Me, EventArgs.Empty)
        // End Sub

        public event EventHandler CanExecuteChanged; //i am forced to make this public.
    }


}
