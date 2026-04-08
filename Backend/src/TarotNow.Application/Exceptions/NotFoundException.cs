
namespace TarotNow.Application.Exceptions;

// Exception chuẩn cho các trường hợp không tìm thấy tài nguyên theo định danh.
public class NotFoundException : Exception
{
    /// <summary>
    /// Khởi tạo exception not found với thông điệp mặc định.
    /// Luồng xử lý: ủy quyền về constructor có message để thống nhất thông điệp mặc định.
    /// </summary>
    public NotFoundException()
        : this("Resource was not found.")
    {
    }

    /// <summary>
    /// Khởi tạo exception not found với thông điệp tùy chỉnh.
    /// Luồng xử lý: truyền message lên lớp Exception gốc.
    /// </summary>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Khởi tạo exception not found có inner exception.
    /// Luồng xử lý: giữ nguyên nhân gốc để hỗ trợ log và truy vết.
    /// </summary>
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
