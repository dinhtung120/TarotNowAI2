using MediatR;
using Microsoft.Extensions.Logging;

namespace TarotNow.Application.Behaviors;

// Pipeline behavior ghi log đầu-cuối cho mỗi request MediatR để hỗ trợ truy vết nghiệp vụ.
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Khởi tạo behavior ghi log cho request pipeline.
    /// Luồng xử lý: nhận logger typed theo generic request/response để log có ngữ cảnh rõ ràng.
    /// </summary>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Ghi log khi bắt đầu/kết thúc xử lý request và log lỗi nếu handler ném exception.
    /// Luồng xử lý: log start, chạy handler kế tiếp trong try-catch, log success hoặc log fail rồi ném lại.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[MediatR] Handling request {RequestName}", requestName);

        try
        {
            var response = await next();
            // Nhánh thành công: ghi log hoàn tất để đo luồng xử lý theo từng request type.
            _logger.LogInformation("[MediatR] Handled request {RequestName}", requestName);
            return response;
        }
        catch (Exception ex)
        {
            // Nhánh lỗi: log lỗi chi tiết nhưng vẫn ném lại để các lớp xử lý lỗi chuẩn phía trên tiếp nhận.
            _logger.LogError(ex, "[MediatR] Request {RequestName} failed", requestName);
            throw;
        }
    }
}
