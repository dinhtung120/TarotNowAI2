namespace TarotNow.Domain.Entities;

/// <summary>
/// Domain Entity mapping tới bảng `ai_requests` trong Postgresql.
/// Quản lý trạng thái gọi AI, chi phí bị trừ, và Idempotency cho chức năng Hoàn tiền.
/// </summary>
public class AiRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Tham chiếu qua cột MongoDB ObjectID của Reading Sessions.
    /// Dài đúng 24 kí tự Hex.
    /// </summary>
    public string ReadingSessionRef { get; set; } = null!;
    
    /// <summary>
    /// NULL = Lần đọc chính. Từ 1 tới 5 = Câu hỏi Follow-up phụ
    /// </summary>
    public short? FollowupSequence { get; set; }

    /// <summary>
    /// Xem TarotNow.Domain.Enums.AiRequestStatus
    /// </summary>
    public string Status { get; set; } = Enums.AiRequestStatus.Requested;

    public DateTimeOffset? FirstTokenAt { get; set; }
    public DateTimeOffset? CompletionMarkerAt { get; set; }
    public string? FinishReason { get; set; }
    public short RetryCount { get; set; }

    public string? PromptVersion { get; set; }
    public string? PolicyVersion { get; set; }
    public Guid? CorrelationId { get; set; }
    public string? TraceId { get; set; }

    public long ChargeGold { get; set; }
    public long ChargeDiamond { get; set; }

    public string? RequestedLocale { get; set; }
    public string? ReturnedLocale { get; set; }
    public string? FallbackReason { get; set; }
    
    /// <summary>
    /// Unique Key chống Deduplication, ví dụ: 'req_xyz_123'
    /// </summary>
    public string? IdempotencyKey { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // Thuộc tính Navigation (Chỉ vào User)
    public User User { get; set; } = null!;
}
