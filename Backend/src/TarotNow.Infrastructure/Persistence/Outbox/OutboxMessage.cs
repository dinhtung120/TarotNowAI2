namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Bản ghi outbox message lưu domain event để dispatch bất đồng bộ đáng tin cậy.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>
    /// Định danh message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tên type domain event.
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Payload JSON của event.
    /// </summary>
    public string PayloadJson { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm event phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; set; }

    /// <summary>
    /// Trạng thái xử lý outbox.
    /// </summary>
    public string Status { get; set; } = OutboxMessageStatus.Pending;

    /// <summary>
    /// Số lần đã thử dispatch.
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    /// Mốc thời gian retry kế tiếp theo UTC.
    /// </summary>
    public DateTime NextAttemptAtUtc { get; set; }

    /// <summary>
    /// Thời điểm xử lý thành công theo UTC.
    /// </summary>
    public DateTime? ProcessedAtUtc { get; set; }

    /// <summary>
    /// Lỗi cuối cùng khi xử lý message.
    /// </summary>
    public string? LastError { get; set; }

    /// <summary>
    /// Thời điểm tạo message theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Thời điểm lock message theo UTC.
    /// </summary>
    public DateTime? LockedAtUtc { get; set; }

    /// <summary>
    /// Định danh worker giữ lock.
    /// </summary>
    public string? LockOwner { get; set; }
}
