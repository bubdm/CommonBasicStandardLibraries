using CommonBasicStandardLibraries.Messenging;
using System;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.MVVMFramework.Blazor.ViewModels
{
    public interface IBlazorScreen
    {
        public Action Closing { get; set; }
        public bool CanClose() => true;
        Task ActivateAsync(); //i don't think we need anything to exit app for the game package anyways.
        IEventAggregator Aggregator { get; }
        Task TryCloseAsync();
    }
}