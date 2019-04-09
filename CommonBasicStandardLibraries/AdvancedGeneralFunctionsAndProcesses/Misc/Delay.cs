using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.Misc
{
    //decided to not even use static.  this could cause a bad habit.  does not allow swapping out easily unfortunately.
    public class Delay : IDelay
    {
        /// <summary>
        /// This delays number of seconds.  if you want 1 and a half, use 1.5.
        /// </summary>
        /// <param name="HowManySeconds"></param>
        /// <returns></returns>
        async Task IDelay.DelaySeconds(double HowManySeconds)
        {
            int TotalTime;
            TotalTime = HowManySeconds.Multiply(1000);
            await Task.Delay(TotalTime);
            //await DelaySeconds(HowLong);
        }
    }
}