namespace TarotNow.Application.Common.Realtime;

/// <summary>
/// Envelope chuẩn cho sự kiện realtime chat fast-lane.
/// </summary>
public sealed class ChatRealtimeEnvelopeV2
{
    /// <summary>
    /// Định danh sự kiện duy nhất phục vụ dedup.
    /// </summary>
    public string EventId { get; init; } = Guid.CreateVersion7().ToString("N");

    /// <summary>
    /// Tên lane phát sự kiện (fast|durable).
    /// </summary>
    public string Lane { get; init; } = "fast";

    /// <summary>
    /// Tên event realtime.
    /// </summary>
    public string EventType { get; init; } = string.Empty;

    /// <summary>
    /// Id conversation liên quan.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Id message liên quan (nếu có).
    /// </summary>
    public string? MessageId { get; init; }

    /// <summary>
    /// Id message phía client dùng cho optimistic reconcile.
    /// </summary>
    public string? ClientMessageId { get; init; }

    /// <summary>
    /// Thời điểm phát sinh sự kiện (UTC).
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Id sender liên quan (nếu có).
    /// </summary>
    public string? SenderId { get; init; }

    /// <summary>
    /// Phiên bản schema contract.
    /// </summary>
    public string SchemaVersion { get; init; } = "v2";

    /// <summary>
    /// Payload nghiệp vụ đính kèm.
    /// </summary>
    public object Payload { get; init; } = new { };
}
