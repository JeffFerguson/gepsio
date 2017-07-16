using System;

namespace JeffFerguson.Gepsio
{
    /// <summary>
    /// The class for all validation errors discovered in the role reference of a loaded document.
    /// </summary>
    public class RoleReferenceValidationError : ValidationError
    {
        /// <summary>
        /// The <see cref="RoleReference"/> object containing the error.
        /// </summary>
        public RoleReference RoleReference { get; private set; }

        /// <summary>
        /// Class constructor. This constructor will store a message and will automatically set the
        /// Exception property to a null value.
        /// </summary>
        /// <param name="roleReference">
        /// The role reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        internal RoleReferenceValidationError(RoleReference roleReference, string message)
            : base(message)
        {
            this.RoleReference = roleReference;
        }

        /// <summary>
        /// Class constructor. This constructor will store a message and an exception.
        /// </summary>
        /// <param name="roleReference">
        /// The role reference containing the error being reported.
        /// </param>
        /// <param name="message">
        /// The message to be stored in the validation error.
        /// </param>
        /// <param name="exception">
        /// The exception to be stored in the validation error.
        /// </param>
        internal RoleReferenceValidationError(RoleReference roleReference, string message, Exception exception)
            : base(message, exception)
        {
            this.RoleReference = roleReference;
        }
    }
}
