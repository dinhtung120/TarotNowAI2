namespace TarotNow.Application.Exceptions;

// Exception biểu diễn vi phạm quy tắc nghiệp vụ domain.
public sealed class BusinessRuleException : Exception
{
    // Mã lỗi mặc định khi caller không truyền error code cụ thể.
    private const string DefaultErrorCode = "business_rule_violation";
    // Thông điệp mặc định cho lỗi vi phạm business rule.
    private const string DefaultMessage = "A business rule has been violated.";

    // Mã lỗi nghiệp vụ phục vụ map sang API response có cấu trúc.
    public string ErrorCode { get; }

    /// <summary>
    /// Khởi tạo exception business rule với mã và thông điệp mặc định.
    /// Luồng xử lý: ủy quyền về constructor đầy đủ để đảm bảo ErrorCode luôn có giá trị.
    /// </summary>
    public BusinessRuleException()
        : this(DefaultErrorCode, DefaultMessage)
    {
    }

    /// <summary>
    /// Khởi tạo exception business rule với thông điệp tùy chỉnh và mã mặc định.
    /// Luồng xử lý: ủy quyền về constructor đầy đủ với error code mặc định.
    /// </summary>
    public BusinessRuleException(string message)
        : this(DefaultErrorCode, message)
    {
    }

    /// <summary>
    /// Khởi tạo exception business rule có inner exception và mã mặc định.
    /// Luồng xử lý: giữ nguyên nhân gốc phục vụ log/debug đồng thời giữ chuẩn error code.
    /// </summary>
    public BusinessRuleException(string message, Exception innerException)
        : this(DefaultErrorCode, message, innerException)
    {
    }

    /// <summary>
    /// Khởi tạo exception business rule với mã lỗi và thông điệp tùy chỉnh.
    /// Luồng xử lý: truyền message lên base và lưu error code ở tầng application.
    /// </summary>
    public BusinessRuleException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Khởi tạo exception business rule đầy đủ gồm mã lỗi, thông điệp và inner exception.
    /// Luồng xử lý: truyền message/inner lên base rồi gán ErrorCode để downstream dùng nhất quán.
    /// </summary>
    public BusinessRuleException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
