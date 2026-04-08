namespace TarotNow.Application.Exceptions;

// Exception chuẩn cho lỗi validation có cấu trúc theo từng trường dữ liệu.
public class ValidationException : Exception
{
    // Tập lỗi theo dạng PropertyName -> Danh sách thông điệp lỗi.
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Khởi tạo exception validation với tập lỗi rỗng.
    /// Luồng xử lý: ủy quyền về constructor nhận dictionary để đồng nhất điểm khởi tạo.
    /// </summary>
    public ValidationException()
        : this(new Dictionary<string, string[]>())
    {
    }

    /// <summary>
    /// Khởi tạo exception validation chỉ với thông điệp tổng quát.
    /// Luồng xử lý: truyền message lên base và khởi tạo tập lỗi rỗng.
    /// </summary>
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Khởi tạo exception validation với tập lỗi chi tiết theo trường.
    /// Luồng xử lý: gán thông điệp chuẩn và lưu dictionary lỗi để API trả về cho client.
    /// </summary>
    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }

    /// <summary>
    /// Khởi tạo exception validation có inner exception.
    /// Luồng xử lý: giữ nguyên nhân gốc và tạo tập lỗi rỗng mặc định.
    /// </summary>
    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = new Dictionary<string, string[]>();
    }
}
