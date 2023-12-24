using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cdn.Freelance.Api.Exceptions
{
    /// <inheritdoc />
    public class DefaultExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DefaultExceptionHandler> _logger;
        private const string ErrorMessage = "An unexpected error occurred.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, ErrorMessage);

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = ErrorMessage,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }, cancellationToken: cancellationToken);

            return true;
        }
    }
}
