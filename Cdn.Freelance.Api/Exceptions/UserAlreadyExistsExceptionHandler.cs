using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.Serialization;

namespace Cdn.Freelance.Api.Exceptions
{
    /// <summary>
    /// Handle user already exist exception. 
    /// </summary>
    public class UserAlreadyExistsExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DefaultExceptionHandler> _logger;
        private const string ErrorMessage = "User name or email address already exists.";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public UserAlreadyExistsExceptionHandler(ILogger<DefaultExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not UserAlreadyExistsException)
                return true;

            _logger.LogError(exception, ErrorMessage);

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = exception.GetType().Name,
                Title = ErrorMessage,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }, cancellationToken: cancellationToken);

            return true;
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
