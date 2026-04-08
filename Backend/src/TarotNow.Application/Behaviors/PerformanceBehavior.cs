using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TarotNow.Application.Behaviors;

// Pipeline behavior đo thời gian xử lý request để cảnh báo các luồng chậm bất thường.
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Khởi tạo behavior theo dõi hiệu năng request.
    /// Luồng xử lý: nhận logger để ghi cảnh báo khi request vượt ngưỡng thời gian xử lý.
    /// </summary>
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Đo thời gian chạy handler kế tiếp và cảnh báo khi vượt ngưỡng chậm.
    /// Luồng xử lý: bắt đầu stopwatch, chạy handler, dừng đo, so ngưỡng và log warning nếu cần.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;
        if (elapsedMs > 500)
        {
            // Rule cảnh báo: request trên 500ms cần theo dõi để tối ưu truy vấn hoặc logic xử lý.
            _logger.LogWarning(
                "[MediatR] Slow request {RequestName} took {ElapsedMs}ms",
                typeof(TRequest).Name,
                elapsedMs);
        }

        return response;
    }
}
