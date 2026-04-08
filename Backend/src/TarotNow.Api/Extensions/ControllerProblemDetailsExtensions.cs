using Microsoft.AspNetCore.Mvc;

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
        return controller.Problem(
            statusCode: StatusCodes.Status401Unauthorized,
            title: "Unauthorized",
            detail: detail ?? "Yêu cầu xác thực hợp lệ.");
    }

    /// <summary>
    /// Trả về ProblemDetails 404 Not Found chuẩn hóa.
    /// </summary>
    /// <param name="controller">Controller gọi extension.</param>
    /// <param name="detail">Chi tiết lý do không tìm thấy.</param>
    /// <returns>IActionResult dạng ProblemDetails với mã 404.</returns>
    public static IActionResult NotFoundProblem(this ControllerBase controller, string detail)
    {
        return controller.Problem(
            statusCode: StatusCodes.Status404NotFound,
            title: "Not Found",
            detail: detail);
    }
}
