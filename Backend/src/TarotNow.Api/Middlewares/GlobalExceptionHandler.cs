using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Middlewares;

// Chuẩn hóa phản hồi lỗi toàn cục về ProblemDetails để client nhận thông điệp nhất quán.
public partial class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Khởi tạo exception handler toàn cục.
    /// Luồng xử lý: nhận logger để ghi lại thông tin lỗi trước khi map phản hồi cho client.
    /// </summary>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Bắt mọi exception chưa được xử lý và trả về ProblemDetails phù hợp.
    /// Luồng xử lý: log lỗi gốc, map sang cấu trúc lỗi chuẩn, ghi response JSON và kết thúc pipeline lỗi.
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Ghi lỗi gốc ngay tại điểm bắt cuối để giữ đủ context điều tra production incident.
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = CreateProblemDetails(exception);

        // Đồng bộ status code/content type theo RFC Problem Details trước khi ghi body.
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        // Trả payload chuẩn hóa để client xử lý đồng nhất mọi nhánh lỗi.
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
