using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the context of a loaded document.
    /// </summary>
    public class ContextValidationError : ValidationError
    {
        /// <summary>
        /// The context containing the error being reported.
        /// </summary>
        public Context Context { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="context">
        /// The context containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal ContextValidationError(Context context, string message)
            : base(message)
        {
            this.Context = context;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="context">
        /// The context containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal ContextValidationError(Context context, string message, Exception exception)
            : base(message, exception)
        {
            this.Context = context;
        }
    }
}
