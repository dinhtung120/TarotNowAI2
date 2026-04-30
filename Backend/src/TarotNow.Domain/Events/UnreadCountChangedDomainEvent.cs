namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi số lượng unread trong hội thoại thay đổi.
/// </summary>
public sealed class UnreadCountChangedDomainEvent : IIdempotentDomainEvent
{
    /// <summary>
    /// Định danh hội thoại.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh user của hội thoại.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh reader của hội thoại.
    /// </summary>
    public string ReaderId { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey =>
        $"chat:unread_changed:{ConversationId}:{UserId}:{ReaderId}:{OccurredAtUtc:O}";
}
