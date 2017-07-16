using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the hyperlink reference of a loaded document.
    /// </summary>
    public class HyperlinkReferenceValidationError : ValidationError
    {
        /// <summary>
        /// The HyperlinkReference containing the error being reported.
        /// </summary>
        public HyperlinkReference HyperlinkReference { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="hyperlinkReference">
        /// The hyperlink reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal HyperlinkReferenceValidationError(HyperlinkReference hyperlinkReference, string message)
            : base(message)
        {
            this.HyperlinkReference = hyperlinkReference;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="hyperlinkReference">
        /// The hyperlink reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal HyperlinkReferenceValidationError(HyperlinkReference hyperlinkReference, string message, Exception exception)
            : base(message, exception)
        {
            this.HyperlinkReference = HyperlinkReference;
        }
    }
}
