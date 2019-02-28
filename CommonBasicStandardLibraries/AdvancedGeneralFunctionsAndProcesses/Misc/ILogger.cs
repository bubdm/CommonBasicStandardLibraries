using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    public interface ILogger
    {
        Task LogToFileAsync(string TextToLog);
    }
}
