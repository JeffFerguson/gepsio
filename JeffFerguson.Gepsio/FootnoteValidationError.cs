using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the footnote of a loaded document.
    /// </summary>
    public class FootnoteValidationError : ValidationError
    {
        /// <summary>
        /// The footnote containing the error being reported.
        /// </summary>
        public Footnote Footnote { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="footnote">
        /// The footnote containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal FootnoteValidationError(Footnote footnote, string message)
            : base(message)
        {
            this.Footnote = footnote;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="footnote">
        /// The footnote containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal FootnoteValidationError(Footnote footnote, string message, Exception exception)
            : base(message, exception)
        {
            this.Footnote = footnote;
        }
    }
}
