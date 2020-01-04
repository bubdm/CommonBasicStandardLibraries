using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Threading.Tasks;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.MVVMHelpers
{
    /// <summary>

    /// Base view model.

    /// </summary>

    public class BaseViewModel : ObservableObject, IErrorHandler, IMappable

    {
        string _title = string.Empty;

        //since its now smart enough to know when its executing, this is no longer needed.
        //for game package, something else should be done.


        /// <summary>

        /// Gets or sets the title.

        /// </summary>

        /// <value>The title.</value>

        public string Title

        {

            get => _title;

            set => SetProperty(ref _title, value);

        }



        protected ISimpleUI ThisMessage { get; set; } //maybe i should set as protected.  but instead register itself.


        //decided that since i need to do more di, then go ahead and require this.
        //can always mock it anyways for unit tests
        //also console can mock it as well.  if there is a need, can put in global functions as well.


        //public BaseViewModel() { } //don't require it though so it can be called from console.   otherwise, you make it harder for testing.

        public BaseViewModel(ISimpleUI tempUI) { ThisMessage = tempUI; }

        async Task IErrorHandler.HandleErrorAsync(Exception ex)
        {
            //if we have something else we are using, do it.
            IErrorHandler? thisError = null;
            ICustomLogger? thisLog = null; //ignore if it can't resolve for this.  because its optional.
            try
            {
                thisError = Resolve<IErrorHandler>();
            }
            catch
            {

            }
            try
            {
                thisLog = Resolve<ICustomLogger>();
            }
            catch
            {

            }
            Exception firstException;
            Exception secondException;
            if (thisLog != null)
            {
                await thisLog.LogToFileAsync(ex.Message);
                await thisLog.LogToFileAsync(ex.StackTrace);
                firstException = ex.InnerException;
                do
                {
                    if (firstException == null)
                        break;
                    await thisLog.LogToFileAsync($"{firstException.Message} Inner Exception Message");
                    await thisLog.LogToFileAsync($"{firstException.StackTrace} Inner Exception Stack Trace");
                    secondException = firstException.InnerException;
                    firstException = secondException;
                } while (true);
            }
            if (thisError != null && thisError.Equals(this) == false)
            {
                await thisError.HandleErrorAsync(ex);
                return;
            }
            await CustomErrorHandler(ex);
        }
        protected virtual Task CustomErrorHandler(Exception ex) //default implementation is to just throw it.  you can do other things if needed.
        {
            Exception newEx = new Exception(ex.Message, ex);
            throw newEx; //hopefully can get more of a trace
        }
    }
}