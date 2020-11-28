using System;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    /// <summary>
    /// this is a temporary class.  will eventually be part of another library for .net 6.  has here to prevent breaking changes until .net 6 comes out.
    /// </summary>
    public static class ToastPlatform
    {
        public static Action<string> ShowWarning { get; set; } = (message) =>
        {
            Console.WriteLine($"Warning { message}");
        };

        public static Action<string> ShowSuccess { get; set; } = (message) =>
        {
            Console.WriteLine($"Success { message}");
        };

        public static Action<string> ShowInfo { get; set; } = (message) =>
        {
            Console.WriteLine($"Info { message}");
        };

        //decided to go ahead and use the same name.  however, this means would have to have a using statement with abbrev to prevent compile errors.
        public static Action<string> ShowError { get; set; } = (message) =>
        {
            Console.WriteLine($"Error { message}");
        };

    }
}
