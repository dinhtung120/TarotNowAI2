using System;

namespace TarotNow.Application.Exceptions;

// Exception chuẩn cho trường hợp người dùng không có quyền thực hiện thao tác.
public class ForbiddenException : Exception
{
    /// <summary>
    /// Khởi tạo exception forbidden không thông điệp.
    /// Luồng xử lý: sử dụng constructor base mặc định của Exception.
    /// </summary>
    public ForbiddenException() : base() { }

    /// <summary>
    /// Khởi tạo exception forbidden với thông điệp tùy chỉnh.
    /// Luồng xử lý: truyền message lên lớp Exception gốc.
    /// </summary>
    public ForbiddenException(string message) : base(message) { }

    /// <summary>
    /// Khởi tạo exception forbidden có inner exception.
    /// Luồng xử lý: giữ nguyên nhân gốc để phục vụ tracing lỗi bảo mật.
    /// </summary>
    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}
