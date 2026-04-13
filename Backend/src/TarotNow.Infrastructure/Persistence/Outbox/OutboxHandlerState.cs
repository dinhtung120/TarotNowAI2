namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Bản ghi idempotency cho từng handler với từng outbox message.
/// </summary>
public sealed class OutboxHandlerState
{
    /// <summary>
    /// Định danh bản ghi.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Định danh outbox message.
    /// </summary>
    public Guid OutboxMessageId { get; set; }

    /// <summary>
    /// Tên handler đã xử lý.
    /// </summary>
    public string HandlerName { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm handler xử lý thành công theo UTC.
    /// </summary>
    public DateTime ProcessedAtUtc { get; set; }
}
