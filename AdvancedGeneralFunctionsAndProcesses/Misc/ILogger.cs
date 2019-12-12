using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public interface ICustomLogger //decided to use customlogger since there is now a logger in .net core.
    {
        Task LogToFileAsync(string textToLog);
    }
}
