using System;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the fact of a loaded document.
    /// </summary>
    class FactValidationError : ValidationError
    {
        /// <summary>
        /// The fact containing the error being reported.
        /// </summary>
        public Fact Fact { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="fact">
        /// The fact containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal FactValidationError(Fact fact, string message)
            : base(message)
        {
            this.Fact = fact;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="fact">
        /// The fact containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal FactValidationError(Fact fact, string message, Exception exception)
            : base(message, exception)
        {
            this.Fact = fact;
        }
    }
}
