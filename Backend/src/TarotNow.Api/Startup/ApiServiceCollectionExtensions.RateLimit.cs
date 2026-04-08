using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

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
    /// Luồng xử lý: cấu hình fixed-window cho login, auth-session, community-write và call-history.
    /// </summary>
    private static void ConfigureRateLimitPolicies(RateLimiterOptions options)
    {
        // Rule bảo vệ brute-force cho đăng nhập theo IP.
        AddFixedWindowPolicy(options, "login", ResolveClientIp, permitLimit: 5, TimeSpan.FromSeconds(60));
        // Rule theo user đã xác thực để hạn chế spam gọi endpoint auth/session.
        AddFixedWindowPolicy(options, "auth-session", ResolveAuthenticatedPartitionKey, permitLimit: 20, TimeSpan.FromMinutes(1));
        // Rule bảo vệ endpoint ghi community khỏi spam nội dung.
        AddFixedWindowPolicy(options, "community-write", ResolveAuthenticatedPartitionKey, permitLimit: 30, TimeSpan.FromMinutes(1));
        // Rule cho lịch sử cuộc gọi cần ngưỡng cao hơn do polling thường xuyên.
        AddFixedWindowPolicy(options, "call-history", ResolveAuthenticatedPartitionKey, permitLimit: 60, TimeSpan.FromMinutes(1));
        // Rule chuẩn cho các thao tác chat (inbox, tin nhắn) để chịu tải tốt hơn khi có SignalR reconnect/polling.
        AddFixedWindowPolicy(options, "chat-standard", ResolveAuthenticatedPartitionKey, permitLimit: 60, TimeSpan.FromMinutes(1));
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

        return new ValueTask(context.HttpContext.Response.WriteAsJsonAsync(problem, cancellationToken: token));
    }

    /// <summary>
    /// Resolve IP client ưu tiên từ `X-Forwarded-For` rồi fallback sang RemoteIpAddress.
    /// Luồng xử lý: parse header proxy theo phần tử đầu tiên, nếu không có thì dùng IP kết nối trực tiếp.
    /// </summary>
    private static string ResolveClientIp(HttpContext httpContext)
    {
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            var first = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(first))
            {
                // Khi qua proxy/load balancer, phần tử đầu là IP client gốc theo quy ước X-Forwarded-For.
                return first;
            }
        }

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
}
