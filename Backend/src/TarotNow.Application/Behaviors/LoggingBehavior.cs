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
        /*
         * Lấy tên của Request để đưa vào log. 
         * Việc sử dụng typeof(TRequest).Name giúp xác định rõ loại command hoặc query nào đang được xử lý.
         */
        var requestName = typeof(TRequest).Name;

        /*
         * Ghi log khi bắt đầu xử lý request. 
         * Theo yêu cầu của người dùng, chúng ta chỉ giữ lại 1 dòng log này và loại bỏ tiền tố [MediatR]
         * để log hiển thị gọn gàng nhất trên console.
         */
        _logger.LogInformation("Handling request {RequestName}", requestName);

        try
        {
            /*
             * Gọi handler tiếp theo trong pipeline. 
             * Kết quả được trả về trực tiếp, bỏ qua bước log hoàn tất để tiết kiệm không gian log.
             */
            return await next();
        }
        catch (Exception ex)
        {
            /*
             * Khi có lỗi, chúng ta vẫn log thông tin lỗi nhưng loại bỏ tiền tố [MediatR] 
             * để đảm bảo tính nhất quán với log thành công.
             */
            _logger.LogError(ex, "Request {RequestName} failed", requestName);
            throw;
        }
    }
}
