using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the decimal type of a loaded document.
    /// </summary>
    public class DecimalValidationError : ValidationError
    {
        /// <summary>
        /// The decimal containing the error being reported.
        /// </summary>
        public Decimal Decimal { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="dec">
        /// The decimal containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal DecimalValidationError(Decimal dec, string message)
            : base(message)
        {
            this.Decimal = dec;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="dec">
        /// The decimal containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal DecimalValidationError(Decimal dec, string message, Exception exception)
            : base(message, exception)
        {
            this.Decimal = dec;
        }
    }
}
