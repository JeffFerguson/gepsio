using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the item of a loaded document.
    /// </summary>
    public class ItemValidationError : ValidationError
    {
        /// <summary>
        /// The item containing the error being reported.
        /// </summary>
        public Item Item { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="item">
        /// The item containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal ItemValidationError(Item item, string message)
            : base(message)
        {
            this.Item = item;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="item">
        /// The item containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal ItemValidationError(Item item, string message, Exception exception)
            : base(message, exception)
        {
            this.Item = item;
        }
    }
}
