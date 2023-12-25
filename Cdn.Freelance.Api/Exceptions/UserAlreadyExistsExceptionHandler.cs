using System.Net;

namespace Cdn.Freelance.Api.Exceptions
{
    /// <summary>
    /// Handle user already exist exception. 
    /// </summary>
    public class UserAlreadyExistsExceptionHandler : BaseExceptionHandler<UserAlreadyExistsException>
    {
        /// <inheritdoc />
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        /// <inheritdoc />
        public override string ErrorMessage => "User name or email address already exists.";

        /// <inheritdoc />
        public UserAlreadyExistsExceptionHandler(ILogger<BaseExceptionHandler<UserAlreadyExistsException>> logger) : base(logger)
        {
        }
    }

    /// <inheritdoc />
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        /// <summary>
        /// Creates and instance of <see cref="UserAlreadyExistsException"/>
        /// </summary>
        public UserAlreadyExistsException()
        {
        }

        /// <inheritdoc />
        public UserAlreadyExistsException(string? message) : base(message)
        {
        }

        /// <inheritdoc />
        public UserAlreadyExistsException(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }
    }
}
