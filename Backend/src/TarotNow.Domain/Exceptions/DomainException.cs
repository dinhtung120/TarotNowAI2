namespace TarotNow.Domain.Exceptions;

/// <summary>
/// Exception gốc cho tất cả các lỗi thuộc về Business Logic / Domain Rule.
/// Bất kỳ lỗi nào phát sinh do vi phạm logic kinh doanh (ví dụ: số dư nhỏ hơn 0, 
/// lá bài không tồn tại, trạng thái không hợp lệ) đều nên kế thừa lớp này.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Mã lỗi chuẩn (ví dụ: "INSUFFICIENT_FUNDS", "CARD_NOT_FOUND").
    /// Dành cho client xử lý thay vì parse text message.
    /// </summary>
    public string ErrorCode { get; }

    public DomainException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
