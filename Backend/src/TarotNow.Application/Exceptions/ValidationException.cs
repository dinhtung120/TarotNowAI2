namespace TarotNow.Application.Exceptions;

/// <summary>
/// Exception tung ra khi dữ liệu đầu vào không vượt qua FluentValidation.
/// Kế thừa Exception để GlobalExceptionHandler dễ dàng bắt 
/// và chuyển đổi thành ProblemDetails HTTP 400 Bad Request.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Danh sách chi tiết các field bị lỗi và thông báo tương ứng.
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors) 
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }
}
