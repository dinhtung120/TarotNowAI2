using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Middlewares;

public partial class GlobalExceptionHandler
{
    /// <summary>
    /// Tạo ProblemDetails cho lỗi yêu cầu không hợp lệ.
    /// Luồng xử lý: ủy quyền về factory chung để giữ cấu trúc lỗi client nhất quán.
    /// </summary>
    private static ProblemDetails CreateBadRequestProblem(string? detail = null)
        => CreateClientProblem(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            string.IsNullOrWhiteSpace(detail)
                ? "Request payload is invalid or unsupported."
                : detail);

    /// <summary>
    /// Tạo ProblemDetails cho lỗi không tìm thấy tài nguyên.
    /// Luồng xử lý: đóng gói tiêu đề/type chuẩn 404 để client xử lý nhánh thiếu dữ liệu.
    /// </summary>
    private static ProblemDetails CreateNotFoundProblem(string? detail = null)
        => CreateClientProblem(
            StatusCodes.Status404NotFound,
            "Not Found",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            string.IsNullOrWhiteSpace(detail)
                ? "Requested resource was not found."
                : detail);

    /// <summary>
    /// Tạo ProblemDetails cho lỗi xung đột dữ liệu.
    /// Luồng xử lý: sử dụng mã 409 để biểu diễn trạng thái trùng/đụng độ nghiệp vụ.
    /// </summary>
    private static ProblemDetails CreateConflictProblem(string detail)
        => CreateClientProblem(
            StatusCodes.Status409Conflict,
            "Conflict",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            detail);

    /// <summary>
    /// Tạo ProblemDetails cho trường hợp truy cập chưa được xác thực/ủy quyền.
    /// Luồng xử lý: dùng phản hồi 401 chuẩn để client biết cần làm mới phiên đăng nhập.
    /// </summary>
    private static ProblemDetails CreateUnauthorizedProblem(string? detail = null)
        => CreateClientProblem(
            StatusCodes.Status401Unauthorized,
            "Unauthorized",
            "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            string.IsNullOrWhiteSpace(detail)
                ? "You are not authorized to access this resource."
                : detail);

    /// <summary>
    /// Tạo ProblemDetails cho trường hợp đã xác thực nhưng không đủ quyền thao tác.
    /// </summary>
    private static ProblemDetails CreateForbiddenProblem(string? detail = null)
        => CreateClientProblem(
            StatusCodes.Status403Forbidden,
            "Forbidden",
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
            string.IsNullOrWhiteSpace(detail)
                ? "You do not have permission to access this resource."
                : detail);

    /// <summary>
    /// Tạo ProblemDetails mặc định cho lỗi hệ thống chưa được map cụ thể.
    /// Luồng xử lý: trả thông tin 500 tối giản để không lộ chi tiết nội bộ.
    /// </summary>
    private static ProblemDetails CreateServerProblem()
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred while processing your request. Please try again later."
        };
    }

    /// <summary>
    /// Factory chung tạo ProblemDetails cho nhóm lỗi phía client.
    /// Luồng xử lý: nhận đầy đủ thành phần chuẩn rồi trả object dùng lại cho các nhánh mapping.
    /// </summary>
    private static ProblemDetails CreateClientProblem(int status, string title, string type, string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = type,
            Detail = detail
        };
    }
}
