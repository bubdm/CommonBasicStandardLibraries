using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
    /// <summary>

    /// Base view model.

    /// </summary>

    public class BaseViewModel : ObservableObject, IErrorHandler

    {

        string title = string.Empty;

		//since its now smart enough to know when its executing, this is no longer needed.
		//for game package, something else should be done.


        /// <summary>

        /// Gets or sets the title.

        /// </summary>

        /// <value>The title.</value>

        public string Title

        {

            get => title;

            set => SetProperty(ref title, value);

        }



        public ISimpleUI ThisMessage { get; set; }






        //bool isBusy;



        /// <summary>

        /// Gets or sets a value indicating whether this instance is busy.

        /// </summary>

        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>

        //public bool IsBusy

        //{

        //    get => isBusy;

        //    set

        //    {

        //        if (SetProperty(ref isBusy, value))
        //        {
        //            IsNotBusy = !isBusy;
        //            BusyChanged();
        //        }
                    
        //    }

        //}

        //protected virtual void BusyChanged() // so if i can have code to notify the changes.
        //{
        //}

		async Task IErrorHandler.HandleErrorAsync(Exception ex)
		{
			//if we have something else we are using, do it.
			IErrorHandler ThisError = Resolve<IErrorHandler>();
			ILogger ThisLog = Resolve<ILogger>();
			Exception FirstException;
			Exception SecondException;
			if (ThisLog != null)
			{
				await ThisLog.LogToFileAsync(ex.Message);
				await ThisLog.LogToFileAsync(ex.StackTrace);

				FirstException = ex.InnerException;

				do
				{
					if (FirstException == null)
						break;
					await ThisLog.LogToFileAsync($"{FirstException.Message} Inner Exception Message");
					await ThisLog.LogToFileAsync($"{FirstException.StackTrace} Inner Exception Stack Trace");
					SecondException = FirstException.InnerException;
					FirstException = SecondException;
				} while (true);
			}

			if (ThisError != null)
			{
				await ThisError.HandleErrorAsync(ex);
				return;
			}
            await CustomErrorHandler(ex);
		}

		protected virtual Task CustomErrorHandler(Exception ex) //default implementation is to just throw it.  you can do other things if needed.
		{
			throw ex;
		}

		

		//bool isNotBusy = true;



        /// <summary>

        /// Gets or sets a value indicating whether this instance is not busy.

        /// </summary>

        /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>

        //public bool IsNotBusy

        //{

        //    get => isNotBusy;

        //    set

        //    {

        //        if (SetProperty(ref isNotBusy, value))

        //            IsBusy = !isNotBusy;

        //    }

        //}

    }
}
