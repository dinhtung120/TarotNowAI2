namespace TarotNow.Infrastructure.Persistence.Outbox;

/// <summary>
/// Bản ghi idempotency cho inline domain events theo event key và handler.
/// </summary>
public sealed class OutboxInlineHandlerState
{
    /// <summary>
    /// Định danh bản ghi.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Event idempotency key ổn định cho inline dispatch.
    /// </summary>
    public string EventKey { get; set; } = string.Empty;

    /// <summary>
    /// Tên handler đã xử lý thành công event key.
    /// </summary>
    public string HandlerName { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm xử lý thành công theo UTC.
    /// </summary>
    public DateTime ProcessedAtUtc { get; set; }
}
