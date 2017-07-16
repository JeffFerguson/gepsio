using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The base class for all validation errors discovered in a loaded document.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// A message describing the error.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// If this validation error was caused by an exception thrown by the runtime, then a reference
        /// to the exception will be available here. This property will be null if the validation
        /// error was not caused by an exception thrown by the runtime.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal ValidationError(string message)
        {
            Init(message, null);
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal ValidationError(string message, Exception exception)
        {
            Init(message, exception);
        }

        private void Init(string message, Exception exception)
        {
            this.Message = message;
            this.Exception = exception;
        }
    }
}
