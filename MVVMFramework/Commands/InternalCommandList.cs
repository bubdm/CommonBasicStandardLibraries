using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.MVVMFramework.Commands
{
	internal static class InternalCommandList
	{
		private static readonly CustomBasicList<ICustomCommand> _commandList = new CustomBasicList<ICustomCommand>();

		private static bool _executing;
		public static bool IsExecuting
		{
			get
			{
				return _executing;
			}
			set
			{
				if (value == _executing)
					return;
				_executing = value;
				ReportAll();
			}
		}
		internal static void ReportAll() //when changing, will report to all no matter what.  decided it can be good to notify all that something has changed.
		{

			_commandList.ForEach(items =>
			{
				items.ReportCanExecuteChange();
			});
		}
		public static void AddCommand(ICustomCommand thisCommand)
		{
			_commandList.Add(thisCommand);
		}
	}
}
