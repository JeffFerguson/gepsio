using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the footnote arc of a loaded document.
    /// </summary>
    public class FootnoteArcValidationError : ValidationError
    {
        /// <summary>
        /// The footnote arc containing the error being reported.
        /// </summary>
        public FootnoteArc Arc { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="arc">
        /// The footnote arc containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal FootnoteArcValidationError(FootnoteArc arc, string message)
            : base(message)
        {
            this.Arc = arc;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="arc">
        /// The footnote arc containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal FootnoteArcValidationError(FootnoteArc arc, string message, Exception exception)
            : base(message, exception)
        {
            this.Arc = arc;
        }
    }
}
