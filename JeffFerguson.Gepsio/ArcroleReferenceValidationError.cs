using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the arcrole reference of a loaded document.
    /// </summary>
    public class ArcroleReferenceValidationError : ValidationError
    {
        /// <summary>
        /// The <see cref="ArcroleReference"/> object containing the error.
        /// </summary>
        public ArcroleReference ArcroleReference { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="arcroleReference">
        /// The arcrole reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal ArcroleReferenceValidationError(ArcroleReference arcroleReference, string message)
            : base(message)
        {
            this.ArcroleReference = arcroleReference;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="arcroleReference">
        /// The arcrole reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal ArcroleReferenceValidationError(ArcroleReference arcroleReference, string message, Exception exception)
            : base(message, exception)
        {
            this.ArcroleReference = arcroleReference;
        }
    }
}
