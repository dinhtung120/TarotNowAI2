using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Common.Constants;

namespace TarotNow.Api.Startup;

public static partial class ApiServiceCollectionExtensions
{
    /// <summary>
    /// Đăng ký rate limiter tổng cho API và chuẩn hóa phản hồi khi bị từ chối.
    /// Luồng xử lý: đặt status code 429, gắn callback trả ProblemDetails, rồi khai báo các policy cụ thể.
    /// </summary>
    private static void AddRateLimitPolicies(IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = WriteRateLimitProblemAsync;
            ConfigureRateLimitPolicies(options);
        });
    }

    /// <summary>
    /// Khai báo danh sách policy rate limit theo từng luồng nghiệp vụ.
    /// Luồng xử lý: cấu hình fixed-window cho login, auth-session, community-write và chat-standard.
    /// </summary>
    private static void ConfigureRateLimitPolicies(RateLimiterOptions options)
    {
        // Rule bảo vệ brute-force cho đăng nhập theo IP.
        AddFixedWindowPolicy(options, "auth-login", ResolveClientIp, permitLimit: 5, TimeSpan.FromSeconds(60));
        // Rule theo user đã xác thực để hạn chế spam gọi endpoint auth/session.
        // Nâng cao giới hạn (100 req/phút) để tránh chặn nhầm khi Load Profile/Navbar/Wallet đồng thời.
        AddFixedWindowPolicy(options, "auth-session", ResolveAuthenticatedPartitionKey, permitLimit: 100, TimeSpan.FromMinutes(1));
        AddFixedWindowPolicy(options, "auth-refresh", ResolveRefreshPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));
        AddFixedWindowPolicy(options, "auth-refresh-token-family", ResolveRefreshFamilyKey, permitLimit: 10, TimeSpan.FromMinutes(1));
        AddFixedWindowPolicy(options, "auth-logout", ResolveRefreshPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));
        // Rule bảo vệ endpoint ghi community khỏi spam nội dung.
        // Tăng lên 60 req/phút để hỗ trợ upload nhiều ảnh hoặc tương tác nhanh.
        AddFixedWindowPolicy(options, "community-write", ResolveAuthenticatedPartitionKey, permitLimit: 60, TimeSpan.FromMinutes(1));
        // Rule chuẩn cho các thao tác chat (inbox, tin nhắn) để chịu tải tốt hơn khi có SignalR reconnect/polling.
        AddFixedWindowPolicy(options, "chat-standard", ResolveAuthenticatedPartitionKey, permitLimit: 200, TimeSpan.FromMinutes(1));
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

    /// <summary>
    /// Resolve IP client từ RemoteIpAddress đã được chuẩn hóa ở tầng ForwardedHeaders.
    /// Luồng xử lý: không đọc trực tiếp X-Forwarded-For để tránh spoof header bypass limiter.
    /// </summary>
    private static string ResolveClientIp(HttpContext httpContext)
    {
        // Edge case thiếu IP: dùng khóa "unknown" để vẫn partition được limiter.
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    /// <summary>
    /// Resolve partition key theo user đã xác thực, fallback về IP nếu chưa đăng nhập.
    /// Luồng xử lý: đọc claim NameIdentifier, trả khóa user khi có; ngược lại dùng khóa theo IP.
    /// </summary>
    private static string ResolveAuthenticatedPartitionKey(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            // Ưu tiên key theo user để quota gắn đúng chủ thể nghiệp vụ.
            return $"user:{userId}";
        }

        // Nhánh anonymous: fallback theo IP để vẫn chặn được traffic bất thường trước bước auth.
        return $"ip:{ResolveClientIp(httpContext)}";
    }

    /// <summary>
    /// Resolve partition key cho refresh theo thứ tự ưu tiên:
    /// refresh token fingerprint -> user claim -> device -> IP.
    /// </summary>
    private static string ResolveRefreshPartitionKey(HttpContext httpContext)
    {
        if (TryResolveRefreshTokenPartition(httpContext, out var refreshTokenPartition))
        {
            return refreshTokenPartition;
        }

        var authenticatedPartition = ResolveAuthenticatedPartitionOrDefault(httpContext);
        if (!string.IsNullOrWhiteSpace(authenticatedPartition))
        {
            return authenticatedPartition;
        }

        var deviceId = httpContext.Request.Headers[AuthHeaders.DeviceId].ToString();
        if (string.IsNullOrWhiteSpace(deviceId)
            && httpContext.Request.Cookies.TryGetValue(AuthCookieNames.DeviceId, out var deviceCookie))
        {
            deviceId = deviceCookie;
        }

        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            return $"refresh-device:{HashPrefix(deviceId, 24)}";
        }

        return $"ip:{ResolveClientIp(httpContext)}";
    }

    /// <summary>
    /// Resolve partition key theo token-family proxy (token fingerprint trước, fallback user/device/ip).
    /// </summary>
    private static string ResolveRefreshFamilyKey(HttpContext httpContext)
    {
        if (TryResolveRefreshTokenPartition(httpContext, out var refreshTokenPartition))
        {
            return $"refresh-family:{refreshTokenPartition}";
        }

        var authenticatedPartition = ResolveAuthenticatedPartitionOrDefault(httpContext);
        if (!string.IsNullOrWhiteSpace(authenticatedPartition))
        {
            return $"refresh-family:{authenticatedPartition}";
        }

        var deviceId = httpContext.Request.Headers[AuthHeaders.DeviceId].ToString();
        if (string.IsNullOrWhiteSpace(deviceId)
            && httpContext.Request.Cookies.TryGetValue(AuthCookieNames.DeviceId, out var deviceCookie))
        {
            deviceId = deviceCookie;
        }

        if (!string.IsNullOrWhiteSpace(deviceId))
        {
            return $"refresh-family:device:{HashPrefix(deviceId, 24)}";
        }

        return $"refresh-family:ip:{ResolveClientIp(httpContext)}";
    }

    private static bool TryResolveRefreshTokenPartition(HttpContext httpContext, out string partition)
    {
        partition = string.Empty;
        if (!httpContext.Request.Cookies.TryGetValue(AuthCookieNames.RefreshToken, out var refreshToken)
            || string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        partition = $"refresh-token:{HashPrefix(refreshToken, 16)}";
        return true;
    }

    private static string? ResolveAuthenticatedPartitionOrDefault(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return $"user:{userId}";
    }

    private static string HashPrefix(string raw, int length)
    {
        var normalized = raw.Trim();
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(normalized));
        var hash = Convert.ToHexString(bytes).ToLowerInvariant();
        var max = Math.Clamp(length, 8, hash.Length);
        return hash[..max];
    }
}
