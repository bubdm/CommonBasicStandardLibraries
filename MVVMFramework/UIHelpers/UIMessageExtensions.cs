using CommonBasicStandardLibraries.Messenging;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.MVVMFramework.UIHelpers
{
    /// <summary>
    /// this has the extensions for using event aggravation for sending ui specific messages
    /// </summary>
    public static class UIMessageExtensions
    {
        //if you are going to handle focus first, then you must handle 
        public static void FocusOnFirst(this IEventAggregator aggregator)
        {
            aggregator.Publish(new UIEventModel(), "focus");
            //we have to allow duplicates until we get to version 2.
            //its important to 
        }
        public static void HandleFocus(this IEventAggregator aggregator, object payLoad)
        {
            aggregator.Subscribe(payLoad, "focus");
        }

        public static Task DisplayDialogAsync(this IEventAggregator aggregator, string message)
        {
            return aggregator.PublishAsync(new UIEventModel
            {
                Message = message
            }, "display");
        }

        public static void HandleDisplayDialog(this IEventAggregator aggregator, object payLoad)
        {
            aggregator.Subscribe(payLoad, "display");
        }
    }
}