using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the definition arc of a loaded document.
    /// </summary>
    public class DefinitionArcValidationError : ValidationError
    {
        /// <summary>
        /// The definition arc containing the error being reported.
        /// </summary>
        public DefinitionArc Arc { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="arc">
        /// The definition arc containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal DefinitionArcValidationError(DefinitionArc arc, string message)
            : base(message)
        {
            this.Arc = arc;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="arc">
        /// The definition arc containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal DefinitionArcValidationError(DefinitionArc arc, string message, Exception exception)
            : base(message, exception)
        {
            this.Arc = arc;
        }
    }
}
