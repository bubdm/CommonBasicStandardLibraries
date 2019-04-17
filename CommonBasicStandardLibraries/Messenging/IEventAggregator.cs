﻿using CommonBasicStandardLibraries.Messenging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonBasicStandardLibraries.Messenging
{
    /// <summary>
    /// Enables loosely-coupled publication of and subscription to events.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Searches the subscribed handlers to check if we have a handler for
        /// the message type supplied.
        /// This is for regular ones.
        /// </summary>
        /// <param name="messageType">The message type to check with</param>
        /// <param name="arguments">Tag to search for</param>
        /// <param name="Action">Can be regular or async.</param>
        /// <returns>True if any handler is found, false if not.</returns>
        bool HandlerExistsFor(Type messageType, string arguments = "", EnumActionCategory Action = EnumActionCategory.Regular); //i like this part.  i can specify that if none are found, raise custom error.



        /// <summary>
        /// Needs to implement IHandler for it.   I like that way even better.  More Organized.
        /// </summary>
        /// <param name="subscriber">The instance to subscribe</param>
        /// the subscriber must implement the interface he wants to handle.
        void Subscribe(object subscriber, string Tag = ""); //the arguments are specified in the interface.

        //if i just do subscribe, then will check to see which one is implemented.
        //they should implement one or the other.


        /// <summary>
        /// Unsubscribes the instance from all events.
        /// </summary>
        /// <param name = "subscriber">The instance to unsubscribe.</param>
        void Unsubscribe(object subscriber, string Tag = ""); //when you unsubscribe, its for all.

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
        Task PublishAsync(object message, string arguments = "", bool AllowDuplicates = true);
        /// <summary>
        /// Publishes a message but not async.  Only the ones who implement the non async version will get this.
        /// </summary>
        /// <param name="message">The message instance</param>
        /// <param name="arguments">Extra information so 2 subscribers can reuse the class but something else is needed.</param>
        /// <param name="AllowDuplicates">If Duplicates Are Not Allowed, then if more than one is subscribed, raise an error.
        /// This hints there is a problem.
        /// </param>
        void Publish(object message, string arguments = "", bool AllowDuplicates = true);

        //was going to have whether to allow duplicates. but that is all or nothing.

    }
}
