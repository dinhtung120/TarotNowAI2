namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi một tin nhắn cần được đưa vào luồng kiểm duyệt.
/// </summary>
public sealed class ChatModerationRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh message cần kiểm duyệt.
    /// </summary>
    public string MessageId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh conversation chứa message.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh người gửi message.
    /// </summary>
    public string SenderId { get; init; } = string.Empty;

    /// <summary>
    /// Loại message (text/image/voice/payment_offer...).
    /// </summary>
    public string MessageType { get; init; } = string.Empty;

    /// <summary>
    /// Nội dung cần kiểm duyệt.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh message theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; init; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
