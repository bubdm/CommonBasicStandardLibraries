using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    public static class UIPlatform
    {
        public static IUIThread CurrentThread { get; set; } = new DefaultThread(); //if you don't specify, you will get defaultthread.
        public static IViewLocator? ViewLocator { get; set; }
        public static ILoadScreen? ScreenLoader { get; set; } //can't have default implementations.
        public static Action ExitApp { get; set; } = () => { Console.WriteLine("Closing"); };
        public static Action<string> ShowError { get; set; } = (message) =>
        {
            Console.WriteLine($"There was an error.  The message was {message}");
            ExitApp();
        };
        public static Func<string, Task> ShowMessageAsync { get; set; } = (message) =>
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        };
    }
}