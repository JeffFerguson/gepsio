using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the unit of a loaded document.
    /// </summary>
    public class UnitValidationError : ValidationError
    {
        /// <summary>
        /// The unit containing the error being reported.
        /// </summary>
        public Unit Unit { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="unit">
        /// The unit containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal UnitValidationError(Unit unit, string message)
            : base(message)
        {
            this.Unit = unit;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="unit">
        /// The unit containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal UnitValidationError(Unit unit, string message, Exception exception)
            : base(message, exception)
        {
            this.Unit = Unit;
        }
    }
}
