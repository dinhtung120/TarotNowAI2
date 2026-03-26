using System.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace TarotNow.Api.Middlewares;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);

        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;
        Activity.Current?.SetTag("correlation.id", correlationId);

        using (_logger.BeginScope(new Dictionary<string, object>
               {
                   ["CorrelationId"] = correlationId
               }))
        {
            await _next(context);
        }
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out StringValues incoming)
            && !StringValues.IsNullOrEmpty(incoming))
        {
            var existing = incoming.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(existing))
            {
                return existing;
            }
        }

        var activityId = Activity.Current?.TraceId.ToString();
        if (!string.IsNullOrWhiteSpace(activityId))
        {
            return activityId;
        }

        return Guid.NewGuid().ToString("N");
    }
}
