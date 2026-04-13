namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi tin nhắn chat mới được tạo.
/// </summary>
public sealed class ChatMessageCreatedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh hội thoại.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh tin nhắn.
    /// </summary>
    public string MessageId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh người gửi.
    /// </summary>
    public string SenderId { get; init; } = string.Empty;

    /// <summary>
    /// Loại tin nhắn.
    /// </summary>
    public string MessageType { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
