using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Common.Constants;

namespace TarotNow.Api.Extensions;

// Extension helper trả ProblemDetails chuẩn hóa cho các lỗi phổ biến ở controller.
public static class ControllerProblemDetailsExtensions
{
    /// <summary>
    /// Trả về ProblemDetails 401 Unauthorized chuẩn hóa.
    /// Luồng xử lý: dùng thông điệp mặc định khi caller không cung cấp chi tiết.
    /// </summary>
    /// <param name="controller">Controller gọi extension.</param>
    /// <param name="detail">Chi tiết lỗi tùy chọn.</param>
    /// <returns>IActionResult dạng ProblemDetails với mã 401.</returns>
    public static IActionResult UnauthorizedProblem(this ControllerBase controller, string? detail = null)
    {
        return controller.ApiProblem(
            StatusCodes.Status401Unauthorized,
            AuthErrorCodes.Unauthorized,
            detail ?? "Yêu cầu xác thực hợp lệ.");
    }

    /// <summary>
    /// Trả về ProblemDetails 404 Not Found chuẩn hóa.
    /// </summary>
    /// <param name="controller">Controller gọi extension.</param>
    /// <param name="detail">Chi tiết lý do không tìm thấy.</param>
    /// <returns>IActionResult dạng ProblemDetails với mã 404.</returns>
    public static IActionResult NotFoundProblem(this ControllerBase controller, string detail)
    {
        return controller.ApiProblem(
            StatusCodes.Status404NotFound,
            "NOT_FOUND",
            detail);
    }

    /// <summary>
    /// Trả về ProblemDetails chuẩn hóa cho lỗi controller.
    /// </summary>
    public static IActionResult ApiProblem(
        this ControllerBase controller,
        int statusCode,
        string errorCode,
        string detail)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ResolveTitle(statusCode),
            Type = ResolveType(statusCode),
            Detail = detail
        };
        problem.Extensions["errorCode"] = errorCode;
        return new ObjectResult(problem) { StatusCode = statusCode };
    }

    private static string ResolveTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status409Conflict => "Conflict",
            _ => "Error"
        };
    }

    private static string ResolveType(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            StatusCodes.Status401Unauthorized => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            _ => "about:blank"
        };
    }
}
