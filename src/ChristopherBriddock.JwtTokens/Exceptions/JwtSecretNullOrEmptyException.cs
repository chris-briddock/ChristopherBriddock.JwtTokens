using System;
using System.Runtime.Serialization;

namespace ChristopherBriddock.JwtTokens.Exceptions
{
    internal class JwtSecretNullOrEmptyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSecretNullOrEmptyException"/> class.
        /// </summary>
        public JwtSecretNullOrEmptyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSecretNullOrEmptyException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that describes the exception.</param>
        public JwtSecretNullOrEmptyException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSecretNullOrEmptyException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that describes the exception.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public JwtSecretNullOrEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSecretNullOrEmptyException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected JwtSecretNullOrEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}