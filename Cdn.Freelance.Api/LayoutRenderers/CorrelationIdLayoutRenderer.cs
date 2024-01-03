using NLog.LayoutRenderers;
using NLog;
using System.Text;
using Cdn.Freelance.Api.Middlewares;
using NLog.Web.LayoutRenderers;

namespace Cdn.Freelance.Api.LayoutRenderers
{
    /// <summary>
    /// Represents a renderer that appends the CorrelationId in NLog logs if it exists.
    /// </summary>
    [LayoutRenderer("correlation-id")]
    public class CorrelationIdLayoutRenderer : AspNetLayoutRendererBase
    {
        /// <summary>
        /// Appends the CorrelationId in NLog logs if it exists.
        /// </summary>
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var correlationId = HttpContextAccessor.HttpContext?.Request.Headers[CorrelationIdMiddleware.CorrelationIdHeader];
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = HttpContextAccessor?.HttpContext?.Response.Headers[CorrelationIdMiddleware.CorrelationIdHeader];
            }

            builder.Append(correlationId);
        }
    }
}
