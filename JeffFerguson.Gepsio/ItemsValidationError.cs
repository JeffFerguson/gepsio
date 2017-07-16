using System;
using System.Collections.Generic;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in a set of items in a loaded document.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="ItemsValidationError"/>, which reports on a validation error for a single
    /// item, this class reports on validation errors that are reported against two or more items.
    /// </remarks>
    public class ItemsValidationError : ValidationError
    {
        /// <summary>
        /// A collection of the items containing the error being reported.
        /// </summary>
        public List<Item> Items { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal ItemsValidationError(string message)
            : base(message)
        {
            this.Items = new List<Item>();
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
        internal ItemsValidationError(string message, Exception exception)
            : base(message, exception)
        {
            this.Items = new List<Item>();
        }

        internal void AddItem(Item ItemToAdd)
        {
            this.Items.Add(ItemToAdd);
        }
    }
}
