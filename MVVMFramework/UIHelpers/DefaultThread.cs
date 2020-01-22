using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public class DefaultThread : IUIThread
    {
        void IUIThread.BeginOnUIThread(Action action)
        {
            action();
        }

        //void IExit.ExitApp()
        //{
        //    Console.WriteLine("Exiting");
        //}

        void IUIThread.OnUIThread(Action action)
        {
            action();
        }

        Task IUIThread.OnUIThreadAsync(Func<Task> action)
        {
            return Task.Factory.StartNew(action);
        }
    }
}
