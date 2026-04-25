using System.Globalization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Api.Options;
using TarotNow.Application.Common.Constants;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    private const int HashPrefixMinLength = 8;

    /// <summary>
    /// Đăng ký rate limiter tổng cho API và chuẩn hóa phản hồi khi bị từ chối.
    /// Luồng xử lý: đặt status code 429, gắn callback trả ProblemDetails, rồi khai báo các policy cụ thể.
    /// </summary>
    private static void AddRateLimitPolicies(
        IServiceCollection services,
        IConfiguration configuration)
    {
        var rateLimitPolicies = ResolveRateLimitPolicies(configuration);
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = WriteRateLimitProblemAsync;
            ConfigureRateLimitPolicies(options, rateLimitPolicies);
        });
    }

    /// <summary>
    /// Khai báo danh sách policy rate limit theo từng luồng nghiệp vụ.
    /// Luồng xử lý: cấu hình fixed-window cho login, auth-session, community-write và chat-standard.
    /// </summary>
    private static void ConfigureRateLimitPolicies(
        RateLimiterOptions options,
        RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddLoginPolicy(options, rateLimitPolicies);
        AddSessionPolicy(options, rateLimitPolicies);
        AddRefreshPolicies(options, rateLimitPolicies);
        AddCommunityPolicy(options, rateLimitPolicies);
        AddChatPolicy(options, rateLimitPolicies);
        AddPaymentWebhookPolicy(options, rateLimitPolicies);
    }

    private static void AddLoginPolicy(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-login",
            ResolveClientIp,
            permitLimit: rateLimitPolicies.AuthLogin.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.AuthLogin.WindowSeconds));
    }

    private static void AddSessionPolicy(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-session",
            ResolveAuthenticatedPartitionKey,
            permitLimit: rateLimitPolicies.AuthSession.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.AuthSession.WindowSeconds));
    }

    private static void AddRefreshPolicies(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "auth-refresh",
            httpContext => ResolveRefreshPartitionKey(
                httpContext,
                rateLimitPolicies.HashPrefixes.RefreshDeviceLength,
                rateLimitPolicies.HashPrefixes.RefreshTokenLength),
            permitLimit: rateLimitPolicies.AuthRefresh.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.AuthRefresh.WindowSeconds));
        AddFixedWindowPolicy(
            options,
            "auth-refresh-token-family",
            httpContext => ResolveRefreshFamilyKey(
                httpContext,
                rateLimitPolicies.HashPrefixes.RefreshDeviceLength,
                rateLimitPolicies.HashPrefixes.RefreshTokenLength),
            permitLimit: rateLimitPolicies.AuthRefreshTokenFamily.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.AuthRefreshTokenFamily.WindowSeconds));
        AddFixedWindowPolicy(
            options,
            "auth-logout",
            httpContext => ResolveRefreshPartitionKey(
                httpContext,
                rateLimitPolicies.HashPrefixes.RefreshDeviceLength,
                rateLimitPolicies.HashPrefixes.RefreshTokenLength),
            permitLimit: rateLimitPolicies.AuthLogout.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.AuthLogout.WindowSeconds));
    }

    private static void AddCommunityPolicy(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "community-write",
            ResolveAuthenticatedPartitionKey,
            permitLimit: rateLimitPolicies.CommunityWrite.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.CommunityWrite.WindowSeconds));
    }

    private static void AddChatPolicy(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "chat-standard",
            ResolveAuthenticatedPartitionKey,
            permitLimit: rateLimitPolicies.ChatStandard.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.ChatStandard.WindowSeconds));
    }

    private static void AddPaymentWebhookPolicy(RateLimiterOptions options, RateLimitPoliciesOptions rateLimitPolicies)
    {
        AddFixedWindowPolicy(
            options,
            "payment-webhook",
            ResolveClientIp,
            permitLimit: rateLimitPolicies.PaymentWebhook.PermitLimit,
            window: TimeSpan.FromSeconds(rateLimitPolicies.PaymentWebhook.WindowSeconds));
    }

    /// <summary>
    /// Đăng ký một fixed-window policy dùng partition key tùy biến.
    /// Luồng xử lý: resolve partition từ HttpContext, tạo limiter options với queue bằng 0 để từ chối ngay.
    /// </summary>
    private static void AddFixedWindowPolicy(
        RateLimiterOptions options,
        string policyName,
        Func<HttpContext, string> partitionResolver,
        int permitLimit,
        TimeSpan window)
    {
        // Dùng partition-based limiter để tách quota theo user/IP, tránh người này ảnh hưởng người khác.
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

    /// <summary>
    /// Trả phản hồi chuẩn khi request bị rate limit.
    /// Luồng xử lý: thêm Retry-After nếu có metadata, sau đó ghi ProblemDetails 429 cho client.
    /// </summary>
    private static ValueTask WriteRateLimitProblemAsync(OnRejectedContext context, CancellationToken token)
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            // Chuẩn hóa Retry-After theo giây để client dễ dùng cho cơ chế retry/backoff.
            var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
            context.HttpContext.Response.Headers.RetryAfter = seconds.ToString(CultureInfo.InvariantCulture);
        }

        // Payload lỗi cố định giúp frontend xử lý thống nhất mọi nhánh bị giới hạn tần suất.
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Too Many Requests",
            Type = "https://datatracker.ietf.org/doc/html/rfc6585#section-4",
            Detail = "Too many requests. Please try again later."
        };
        problem.Extensions["errorCode"] = AuthErrorCodes.RateLimited;

        return new ValueTask(context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token));
    }
}
