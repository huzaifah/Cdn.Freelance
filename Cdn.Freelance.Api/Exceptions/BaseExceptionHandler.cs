using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cdn.Freelance.Api.Exceptions
{
    /// <summary>
    /// Handle specific exception. 
    /// </summary>
    public abstract class BaseExceptionHandler<T> : IExceptionHandler where T : Exception
    {
        private readonly ILogger<BaseExceptionHandler<T>> _logger;

        /// <summary>
        /// Http status code.
        /// </summary>
        public abstract HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Error message.
        /// </summary>
        public abstract string ErrorMessage { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        protected BaseExceptionHandler(ILogger<BaseExceptionHandler<T>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not T)
                return false;

            _logger.LogError(exception, ErrorMessage);

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)StatusCode,
                Type = exception.GetType().Name,
                Title = ErrorMessage,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }, cancellationToken: cancellationToken);

            return true;
        }
    }
}
