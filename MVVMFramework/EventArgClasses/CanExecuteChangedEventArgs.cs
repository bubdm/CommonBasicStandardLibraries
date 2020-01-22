using System;
namespace CommonBasicStandardLibraries.MVVMFramework.EventArgClasses
{
    public delegate void CanExecuteChangedEventHandler(object sender, CanExecuteChangedEventArgs e);
    public class CanExecuteChangedEventArgs : EventArgs
    {

        public string Name { get; set; } //i prefer the name alone.

        public CanExecuteChangedEventArgs(string name)
        {
            Name = name;
        }
    }
}