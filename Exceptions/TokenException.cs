using System.Runtime.Serialization;
using TestWebApplication.Models;

namespace TestWebApplication.Exceptions
{
    /// <summary>
    /// Exception thrown when a token is invalid
    /// </summary>
    public class TokenException : Exception
    {
        /// <summary>
        /// Error codes that map to handled error use-cases inside API
        /// </summary>
        public ErrorCode ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenException"/> class with error code
        /// </summary>
        /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
        public TokenException(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="TokenException"/> class with error code and message
        /// </summary>
        /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
        /// <param name="message">The massage that describe the error</param>
        public TokenException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="TokenException"/> class with error code, message and the inner exception
        /// </summary>
        /// <param name="errorCode">Error codes that map to handled error use-cases inside API</param>
        /// <param name="message">The massage that describe the error</param>
        /// <param name="innerException">The inner exception</param>
        public TokenException(ErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <inheritdoc />
        public TokenException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public TokenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected TokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }  
    }
}
