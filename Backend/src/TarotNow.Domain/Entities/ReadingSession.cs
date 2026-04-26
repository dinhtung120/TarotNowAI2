

using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

// Entity phiên đọc bài để quản lý thông tin câu hỏi, bộ lá rút và trạng thái hoàn tất của một phiên.
public class ReadingSession
{
    // Định danh phiên đọc bài.
    public string Id { get; private set; } = string.Empty;

    // Định danh người dùng sở hữu phiên.
    public string UserId { get; private set; } = string.Empty;

    // Loại trải bài áp dụng cho phiên.
    public string SpreadType { get; private set; } = string.Empty;

    // Câu hỏi người dùng gửi cho phiên đọc bài.
    public string? Question { get; private set; }

    // Dữ liệu lá bài đã rút (dạng JSON).
    public string? CardsDrawn { get; private set; }

    // Loại tiền đã dùng để thanh toán phiên.
    public string? CurrencyUsed { get; private set; }

    // Số tiền đã thu cho phiên.
    public long AmountCharged { get; private set; }

    // Cờ cho biết phiên đã hoàn tất hay chưa.
    public bool IsCompleted { get; private set; }

    // Thời điểm tạo phiên.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm phiên được đánh dấu hoàn tất.
    public DateTime? CompletedAt { get; private set; }

    // Tóm tắt AI của phiên (nếu có).
    public string? AiSummary { get; private set; }

    // Danh sách follow-up đã phát sinh trong phiên.
    public IReadOnlyList<ReadingFollowup> Followups { get; private set; } = new List<ReadingFollowup>();

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khôi phục entity từ dữ liệu đã lưu.
    /// </summary>
    protected ReadingSession() { }

    /// <summary>
    /// Khởi tạo phiên đọc bài mới trước khi thực hiện rút lá và trả kết quả.
    /// Luồng xử lý: tạo id phiên, gán thông tin đầu vào và đặt trạng thái ban đầu chưa hoàn tất.
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

    /// <summary>
    /// Khôi phục entity ReadingSession từ snapshot để tái tạo đúng state domain đã lưu.
    /// Luồng xử lý: khởi tạo session nền từ dữ liệu chính rồi gán lại toàn bộ trường snapshot.
    /// </summary>
    public static ReadingSession Rehydrate(ReadingSessionSnapshot snapshot)
    {
        return new ReadingSession(
            snapshot.UserId,
            snapshot.SpreadType,
            snapshot.Question,
            snapshot.CurrencyUsed,
            snapshot.AmountCharged)
        {
            Id = snapshot.Id,
            CardsDrawn = snapshot.CardsDrawn,
            IsCompleted = snapshot.IsCompleted,
            CreatedAt = snapshot.CreatedAt,
            CompletedAt = snapshot.CompletedAt,
            AiSummary = snapshot.AiSummary,
            Followups = snapshot.Followups ?? new List<ReadingFollowup>()
            // Edge case: snapshot không có followup thì gán danh sách rỗng để tránh null downstream.
        };
    }

    /// <summary>
    /// Đánh dấu hoàn tất phiên sau khi đã có kết quả lá bài.
    /// Luồng xử lý: lưu cardsDrawn, set IsCompleted và chốt mốc CompletedAt.
    /// </summary>
    public void CompleteSession(string cardsDrawnJson)
    {
        CardsDrawn = cardsDrawnJson;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        // Chốt trạng thái phiên để ngăn các luồng xử lý coi đây là phiên đang mở.
    }
}

// DTO follow-up trong phiên đọc bài để lưu cặp hỏi/đáp phát sinh thêm.
public class ReadingFollowup
{
    // Nội dung câu hỏi follow-up.
    public string Question { get; set; } = string.Empty;

    // Nội dung câu trả lời follow-up.
    public string Answer { get; set; } = string.Empty;

    // AI request id tạo ra follow-up này, dùng cho idempotency khi callback lặp.
    public string? AiRequestId { get; set; }
}

// Snapshot dữ liệu phiên đọc bài dùng cho cơ chế rehydrate domain entity.
public sealed class ReadingSessionSnapshot
{
    // Định danh phiên.
    public string Id { get; init; } = string.Empty;

    // Định danh người dùng.
    public string UserId { get; init; } = string.Empty;

    // Loại trải bài.
    public string SpreadType { get; init; } = string.Empty;

    // Câu hỏi ban đầu.
    public string? Question { get; init; }

    // Dữ liệu lá bài đã rút.
    public string? CardsDrawn { get; init; }

    // Loại tiền đã thu.
    public string? CurrencyUsed { get; init; }

    // Số tiền đã thu.
    public long AmountCharged { get; init; }

    // Trạng thái hoàn tất.
    public bool IsCompleted { get; init; }

    // Thời điểm tạo.
    public DateTime CreatedAt { get; init; }

    // Thời điểm hoàn tất.
    public DateTime? CompletedAt { get; init; }

    // Tóm tắt AI đã lưu.
    public string? AiSummary { get; init; }

    // Danh sách follow-up đã lưu.
    public IReadOnlyList<ReadingFollowup>? Followups { get; init; }
}
