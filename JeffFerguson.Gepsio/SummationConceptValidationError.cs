using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the summation concept of a loaded document.
    /// </summary>
    public class SummationConceptValidationError : ValidationError
    {
        /// <summary>
        /// The summation concept containing the error being reported.
        /// </summary>
        public SummationConcept SummationConcept { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="summationConcept">
        /// The SummationConcept containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal SummationConceptValidationError(SummationConcept summationConcept, string message)
            : base(message)
        {
            this.SummationConcept = summationConcept;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="summationConcept">
        /// The SummationConcept containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal SummationConceptValidationError(SummationConcept summationConcept, string message, Exception exception)
            : base(message, exception)
        {
            this.SummationConcept = summationConcept;
        }
    }
}
