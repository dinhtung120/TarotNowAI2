using System.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace TarotNow.Api.Middlewares;

// Chuẩn hóa correlation id cho toàn bộ request để truy vết log xuyên suốt pipeline.
public sealed class CorrelationIdMiddleware
{
    // Header chuẩn dùng để nhận/trả correlation id giữa client và server.
    public const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    /// <summary>
    /// Khởi tạo middleware gắn correlation id cho request.
    /// Luồng xử lý: nhận middleware kế tiếp và logger để mở scope log theo request.
    /// </summary>
    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Đảm bảo mỗi request có correlation id nhất quán trên trace identifier, response header và log scope.
    /// Luồng xử lý: resolve id ưu tiên từ header, gắn vào context, sau đó chạy middleware kế tiếp trong log scope.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);

        // Đồng bộ trạng thái nhận diện request giữa ASP.NET trace, header phản hồi và telemetry.
        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;
        Activity.Current?.SetTag("correlation.id", correlationId);

        using (_logger.BeginScope(new Dictionary<string, object>
               {
                   ["CorrelationId"] = correlationId
               }))
        {
            // Chạy phần còn lại của pipeline trong scope để mọi log downstream mang cùng correlation id.
            await _next(context);
        }
    }

    /// <summary>
    /// Ưu tiên tái sử dụng correlation id từ client; nếu không có thì fallback về Activity TraceId hoặc GUID mới.
    /// Luồng xử lý: đọc header hợp lệ, thử lấy trace id hiện tại, cuối cùng sinh id ngẫu nhiên.
    /// </summary>
    private static string ResolveCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out StringValues incoming)
            && !StringValues.IsNullOrEmpty(incoming))
        {
            var existing = incoming.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(existing))
            {
                // Ưu tiên giá trị client gửi để giữ liên kết trace giữa nhiều service.
                return existing;
            }
        }

        var activityId = Activity.Current?.TraceId.ToString();
        if (!string.IsNullOrWhiteSpace(activityId))
        {
            // Fallback sang trace id hiện tại khi header không hợp lệ hoặc không tồn tại.
            return activityId;
        }

        // Edge case cuối: sinh GUID mới để đảm bảo request luôn có id truy vết.
        return Guid.NewGuid().ToString("N");
    }
}
