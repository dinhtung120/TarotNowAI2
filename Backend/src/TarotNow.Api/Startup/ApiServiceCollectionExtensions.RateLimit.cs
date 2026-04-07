using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    private static void AddRateLimitPolicies(IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = WriteRateLimitProblemAsync;
            ConfigureRateLimitPolicies(options);
        });
    }

    private static void ConfigureRateLimitPolicies(RateLimiterOptions options)
    {
        AddFixedWindowPolicy(options, "login", ResolveClientIp, permitLimit: 5, TimeSpan.FromSeconds(60));
        AddFixedWindowPolicy(options, "auth-session", ResolveAuthenticatedPartitionKey, permitLimit: 20, TimeSpan.FromMinutes(1));
        AddFixedWindowPolicy(options, "community-write", ResolveAuthenticatedPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));
        AddFixedWindowPolicy(options, "call-history", ResolveAuthenticatedPartitionKey, permitLimit: 60, TimeSpan.FromMinutes(1));
    }

    private static void AddFixedWindowPolicy(
        RateLimiterOptions options,
        string policyName,
        Func<HttpContext, string> partitionResolver,
        int permitLimit,
        TimeSpan window)
    {
        options.AddPolicy(policyName, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionResolver(httpContext),
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = permitLimit,
                    Window = window,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0,
                    AutoReplenishment = true
                }));
    }

    private static ValueTask WriteRateLimitProblemAsync(OnRejectedContext context, CancellationToken token)
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
            context.HttpContext.Response.Headers.RetryAfter = seconds.ToString(CultureInfo.InvariantCulture);
        }

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Too Many Requests",
            Type = "https://datatracker.ietf.org/doc/html/rfc6585#section-4",
            Detail = "Too many requests. Please try again later."
        };

        return new ValueTask(context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token));
    }

    private static string ResolveClientIp(HttpContext httpContext)
    {
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            var first = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(first))
            {
                return first;
            }
        }

        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private static string ResolveAuthenticatedPartitionKey(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return $"user:{userId}";
        }

        return $"ip:{ResolveClientIp(httpContext)}";
    }
}
