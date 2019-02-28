using System;


namespace SimpleJson.Serialization
{
    /// <summary>
    /// Provides data for the Error event.
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current object the error event is being raised against.
        /// </summary>
        /// <value>The current object the error event is being raised against.</value>
        public object CurrentObject { get; }

        /// <summary>
        /// Gets the error context.
        /// </summary>
        /// <value>The error context.</value>
        internal ErrorContext ErrorContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="currentObject">The current object.</param>
        /// <param name="errorContext">The error context.</param>
        internal ErrorEventArgs(object currentObject, ErrorContext errorContext)
        {
            CurrentObject = currentObject;
            ErrorContext = errorContext;
        }
    }
}