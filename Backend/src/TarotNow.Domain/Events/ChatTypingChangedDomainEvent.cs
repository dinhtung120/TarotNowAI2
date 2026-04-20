namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi trạng thái typing của participant thay đổi.
/// </summary>
public sealed class ChatTypingChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh hội thoại.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh user phát sinh typing event.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// True nếu bắt đầu gõ, false nếu dừng gõ.
    /// </summary>
    public bool IsTyping { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
