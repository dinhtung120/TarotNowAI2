using System;

namespace TarotNow.Application.Exceptions;

/// <summary>
/// Ngoại lệ ném ra khi User không có quyền truy cập tài nguyên (403 Forbidden).
/// Tuân thủ quy tắc RCS1194: Triển khai đầy đủ các constructor chuẩn của Exception.
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException() : base() { }

    public ForbiddenException(string message) : base(message) { }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}
