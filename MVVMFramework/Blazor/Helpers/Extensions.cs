using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.Blazor.EventModels;
using CommonBasicStandardLibraries.MVVMFramework.Blazor.ViewModels;
using System;
namespace CommonBasicStandardLibraries.MVVMFramework.Blazor.Helpers
{
    public static class Extensions
    {

        public static void Ask(this IEventAggregator aggregator, Type type)
        {
            aggregator.Publish(new AskEventModel(), type.Name);
        }

        public static void OpenView(this IBlazorScreen screen)
        {
            OpenEventModel open = new OpenEventModel(screen);
            screen.Aggregator.Publish(open, screen.GetType().Name); //i think.
        }
        public static void CloseView(this IBlazorScreen screen)
        {
            screen.Aggregator.Publish(new CloseEventModel(), screen.GetType().Name);
        }

    }
}
