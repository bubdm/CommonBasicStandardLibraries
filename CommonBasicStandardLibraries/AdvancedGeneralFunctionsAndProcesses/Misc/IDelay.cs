using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    //this needed to be an interface so it can be mocked.  its possible that there is a delay for reals but not in testing.
    public interface IDelay
    {
        Task DelaySeconds(double HowLong);
    }
}
