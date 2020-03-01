using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.Messenging
{
    /// <summary>
    /// Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// this needs to clear all of the subscriptions.  useful when its a new game and need to make sure its all cleared out no matter what.
        /// </summary>
        /// <param name="parent"></param>
        void ClearSubscriptions(object parent);
        /// <summary>
        /// Searches the subscribed handlers to check if we have a handler for
        /// the message type supplied.
        /// This is for regular ones.
        /// </summary>
        /// <param name="messageType">The message type to check with</param>
        /// <param name="arguments">Tag to search for</param>
        /// <param name="Action">Can be regular or async.</param>
        /// <returns>True if any handler is found, false if not.</returns>
        bool HandlerExistsFor(Type messageType, string arguments = "", EnumActionCategory action = EnumActionCategory.Regular); //i like this part.  i can specify that if none are found, raise custom error.
        /// <summary>
        /// Needs to implement IHandler for it.   I like that way even better.  More Organized.
        /// </summary>
        /// <param name="subscriber">The instance to subscribe</param>
        /// the subscriber must implement the interface he wants to handle.
        void Subscribe(object subscriber, string tag = ""); //the arguments are specified in the interface.

        //if i just do subscribe, then will check to see which one is implemented.
        //they should implement one or the other.


        /// <summary>
        /// Unsubscribes the instance from all events.
        /// </summary>
        /// <param name = "subscriber">The instance to unsubscribe.</param>
        void Unsubscribe(object subscriber, string tag = ""); //when you unsubscribe, its for all.

        /// <summary>
        /// Publishes a message.
        /// Only the ones who implement the async version will get this.
        /// </summary>
        /// <param name = "message">The message instance.</param>
        /// <param name="arguments">Extra information so 2 subscribers can reuse the class but something else is needed.</param>
        /// <param name="AllowDuplicates">If Duplicates Are Not Allowed, then if more than one is subscribed, raise an error.
        /// This hints there is a problem.
        /// </param>

        /// <returns>A task that represents the asynchronous operation.</returns>
        Task PublishAsync(object message, string arguments = "", bool allowDuplicates = true);
        /// <summary>
        /// Publishes a message but not async.  Only the ones who implement the non async version will get this.
        /// </summary>
        /// <param name="message">The message instance</param>
        /// <param name="arguments">Extra information so 2 subscribers can reuse the class but something else is needed.</param>
        /// <param name="AllowDuplicates">If Duplicates Are Not Allowed, then if more than one is subscribed, raise an error.
        /// This hints there is a problem.
        /// </param>
        void Publish(object message, string arguments = "", bool allowDuplicates = true);
    }
}