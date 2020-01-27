using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public interface ICalenderWindow
    {
        void LoadCalender(DateTime date);
        //for this first version, is needed.

        event Func<DateTime, Task>? DateSelectedAsync;

    }
}
