namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi một participant đã đọc tin nhắn trong conversation.
/// </summary>
public sealed class ChatMessageReadDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh hội thoại.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh user thực hiện thao tác read.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm đọc tin nhắn theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
