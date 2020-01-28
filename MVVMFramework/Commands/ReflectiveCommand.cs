using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.MVVMFramework.EventArgClasses;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.MVVMFramework.Commands.InternalCommandList;
namespace CommonBasicStandardLibraries.MVVMFramework.Commands
{
	public class ReflectiveCommand : ICustomCommand
	{
		private readonly PropertyInfo? _canExecutep;
		private readonly MethodInfo _execute;
		private readonly object _model;
		private readonly MethodInfo? _canExecuteM;

		private bool _isAsync;
		private readonly string _functionName = "";
		//this is needed so the data entry forms will know when it can focus on control.
		public static bool CurrentlyExecuting()
		{
			return IsExecuting;
		}

		public ReflectiveCommand(object model, MethodInfo execute, MethodInfo canExecuteM)
		{
			_model = model;
			_execute = execute;
			_canExecuteM = canExecuteM;
			if (_canExecuteM != null)
				_functionName = canExecuteM.Name;
			HookUpNotifiers();
		}
		public ReflectiveCommand(object model, MethodInfo execute, PropertyInfo? canExecute)
		{
			_model = model;
			_execute = execute;
			_canExecutep = canExecute;
			if (_canExecutep != null)
				_functionName = _canExecutep.Name;
			HookUpNotifiers();
		}
		private void HookUpNotifiers()
		{
			_isAsync = _execute.ReturnType.Name == "Task";

			if (_canExecuteM == null && _canExecutep == null)
				return; //no need to notify because the resulting part is not even there.
			if (_model is INotifyCanExecuteChanged notifier)
				notifier.CanExecuteChanged += Notifier_CanExecuteChanged;
			AddCommand(this);
		}

		private void Notifier_CanExecuteChanged(object sender, CanExecuteChangedEventArgs e)
		{
			if (_functionName == "")
				throw new BasicBlankException("No canexecute function was found.  Should not have raised this.  Rethink");

			if (e.Name == _functionName)
				ReportCanExecuteChange();
		}



		//private bool _executing = false;

		private void Notifier_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_canExecutep == null)
				throw new BasicBlankException("Only properties should have shown property change to let listenrs know canexecute changed");
			if (e.PropertyName == _canExecutep.Name || e.PropertyName == "") //because refresh needs to refresh all period.
				ReportCanExecuteChange();
		}

		//well have to see how that implements icommand.
		//you still have icommand.
		//its just that more is done for me.


		public event EventHandler CanExecuteChanged = delegate { };
		public bool CanExecute(object parameter)
		{
			//can have extra things that runj before even running this (but not yet).
			if (IsExecuting)
				return false;
			if (_canExecutep != null)
				return (bool)_canExecutep.GetValue(_model); //properties cannot have parameters obviously.

			if (_canExecuteM != null)
			{
				if (parameter == null)
					return (bool)_canExecuteM.Invoke(_model, null);
				return (bool)_canExecuteM.Invoke(_model, new object[] { parameter });
			}

			return true;
		}

		//private void PrivateChangeCanExecute()
		//{

		//	//CanExecuteChanged(this, EventArgs.Empty);
		//}

		public async void Execute(object parameter)
		{


			if (CanExecute(parameter) == false)
				return;
			IsExecuting = true;
			//PrivateChangeCanExecute();
			if (_isAsync == false)
				if (parameter == null)
					_execute.Invoke(_model, null);
				else
					_execute.Invoke(_model, new object[] { parameter });
			else

				await UIHelpers.Execute.OnUIThreadAsync(async () =>
				{
					Task task;
					if (parameter == null)
						task = (Task)_execute.Invoke(_model, null);
					else
						task = (Task)_execute.Invoke(_model, new object[] { parameter });
					await task;
				});
			//await Task.Delay(2000); //another experiment.
			IsExecuting = false;
			//PrivateChangeCanExecute();
			//if (returnValue != null)
			//{
			//	HandleReturnValue(returnValue);
			//}
		}

		public void ReportCanExecuteChange()
		{
			CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
