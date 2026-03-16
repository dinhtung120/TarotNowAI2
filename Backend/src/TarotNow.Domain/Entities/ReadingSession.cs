using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity đại diện cho một phiên đọc bài Tarot.
/// Mỗi phiên lưu trữ: loại trải bài, câu hỏi tùy chọn, các lá bài đã rút,
/// loại tiền tệ sử dụng, và số tiền đã trừ.
/// Thiết kế bổ sung các trường mà spec Phase 1.3 yêu cầu nhưng entity ban đầu thiếu.
/// </summary>
public class ReadingSession
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public string SpreadType { get; private set; }

    /// <summary>
    /// Câu hỏi tùy chọn mà người dùng nhập khi trải bài (nullable vì optional).
    /// Spec Phase 1.3: "Câu hỏi tùy chọn (optional question)"
    /// </summary>
    public string? Question { get; private set; }

    // JSON array chứa index các lá bài đã rút. Ex: "[12, 45, 71]"
    public string? CardsDrawn { get; private set; }

    /// <summary>
    /// Loại tiền đã sử dụng thanh toán cho phiên này (Gold hoặc Diamond).
    /// Dùng để tính EXP: Gold ít hơn Diamond theo spec Phase 1.3.
    /// </summary>
    public string? CurrencyUsed { get; private set; }

    /// <summary>
    /// Số tiền thực tế đã trừ cho phiên đọc bài này.
    /// </summary>
    public long AmountCharged { get; private set; }

    // Status
    public bool IsCompleted { get; private set; }

    // Timestamp
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Dành cho EF Core
    protected ReadingSession() { }

    /// <summary>
    /// Constructor chính khi khởi tạo phiên mới.
    /// </summary>
    public ReadingSession(string userId, string spreadType, string? question = null, string? currencyUsed = null, long amountCharged = 0)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        SpreadType = spreadType;
        Question = question;
        CurrencyUsed = currencyUsed;
        AmountCharged = amountCharged;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void CompleteSession(string cardsDrawnJson)
    {
        CardsDrawn = cardsDrawnJson;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}
