using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Api.Middlewares;

/// <summary>
/// Global exception handler using .NET 8/9 IExceptionHandler.
/// Bắt mọi exception không được xử lý và trả về ProblemDetails (RFC 7807) chuẩn hóa.
/// Nó map từng loại Exception thành mã HTTP và Response thống nhất.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        // Khởi tạo ProblemDetails mặc định là HTTP 500
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred while processing your request. Please try again later."
        };

        // Tuỳ chỉnh ProblemDetails dựa trên loại Exception (Clean Architecture rule)
        switch (exception)
        {
            case ValidationException validationException:
                // Lỗi dữ liệu đầu vào (HTTP 400)
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = validationException.Message;
                problemDetails.Extensions["errors"] = validationException.Errors;
                break;

            case BadRequestException badRequestException:
                // Lỗi yêu cầu không hợp lệ (HTTP 400)
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                problemDetails.Detail = badRequestException.Message;
                break;

            case NotFoundException notFoundException:
                // Lỗi không tìm thấy tài nguyên (HTTP 404)
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
                problemDetails.Detail = notFoundException.Message;
                break;

            case DomainException domainException:
                // Lỗi vi phạm business logic (HTTP 422 hoặc 400 tuy business rule)
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "Domain Rule Violation";
                problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2";
                problemDetails.Detail = domainException.Message;
                problemDetails.Extensions["errorCode"] = domainException.ErrorCode;
                break;

            case UnauthorizedAccessException:
                // Lỗi chưa xác thực/phân quyền (HTTP 401/403)
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "You are not authorized to access this resource.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}
