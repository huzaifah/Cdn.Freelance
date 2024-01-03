using Microsoft.Extensions.Primitives;

namespace Cdn.Freelance.Api.Middlewares
{
    /// <summary>
    /// The correlationId middleware. Propagates the current correlationId or generates a new one if it doesn't exist onto request and response.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        public const string CorrelationIdHeader = "X-Request-ID";

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next RequestDelegate to execute after this one.</param>
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the CorrelationIdMiddleware to add or propagate the correlationId onto request and response headers.
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            bool correlationIdExists = context.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationIds);
            var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

            if (!correlationIdExists)
                context.Response.Headers.Append(CorrelationIdHeader,correlationId);

            await _next(context);
        }
    }
}
