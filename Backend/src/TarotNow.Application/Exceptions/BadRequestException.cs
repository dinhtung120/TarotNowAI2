
namespace TarotNow.Application.Exceptions;

// Exception chuẩn cho các lỗi đầu vào/tham số không hợp lệ ở tầng application.
public class BadRequestException : Exception
{
    /// <summary>
    /// Khởi tạo exception bad request với thông điệp mặc định.
    /// Luồng xử lý: ủy quyền về constructor có message để dùng chung một điểm khởi tạo.
    /// </summary>
    public BadRequestException()
        : this("Bad request.")
    {
    }

    /// <summary>
    /// Khởi tạo exception bad request với thông điệp tùy chỉnh.
    /// Luồng xử lý: truyền message lên lớp Exception gốc.
    /// </summary>
    public BadRequestException(string message) : base(message)
    {
    }

    /// <summary>
    /// Khởi tạo exception bad request có inner exception.
    /// Luồng xử lý: giữ lại nguyên nhân gốc để phục vụ logging và truy vết.
    /// </summary>
    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
