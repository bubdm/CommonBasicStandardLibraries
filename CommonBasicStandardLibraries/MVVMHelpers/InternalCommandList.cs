using System;
using System.Collections.Generic;
using System.Text;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using CommonBasicStandardLibraries.CollectionClasses;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
	internal static class InternalCommandList
	{
		private static readonly CustomBasicList<ICustomCommand> CommandList = new CustomBasicList<ICustomCommand>();

		private static bool _Executing;

		public static  bool IsExecuting
		{
			get
			{
				return _Executing;
			}
			set
			{
				if (value == _Executing)
					return;
				_Executing = value;
				ReportAll();
			}
		}

		internal static void ReportAll() //when changing, will report to all no matter what.  decided it can be good to notify all that something has changed.
		{

            CommandList.ForEach(Items =>
            {
                Items.ReportCanExecuteChange();
            });
            
		}

		public static void AddCommand(ICustomCommand ThisCommand)
		{
			CommandList.Add(ThisCommand);
		}

	}
}
