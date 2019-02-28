using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
	internal static class TaskUtilities //decided to make this internal
	{

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
		public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
		{
			try
			{
				await task;
			}
			catch (Exception ex)
			{
				await handler?.HandleErrorAsync(ex);
			}
		}
	}
}

