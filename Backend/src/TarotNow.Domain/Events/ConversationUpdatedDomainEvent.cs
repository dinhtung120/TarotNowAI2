namespace TarotNow.Domain.Events;

/// <summary>
/// Sự kiện Domain: Cuộc trò chuyện đã được cập nhật trạng thái hoặc thông tin quan trọng.
/// Dùng để kích hoạt các side-effect như gửi tin nhắn SignalR báo cho Frontend reload.
/// </summary>
public class ConversationUpdatedDomainEvent : IDomainEvent
{
    public string ConversationId { get; }
    public string Type { get; }
    public DateTime OccurredAtUtc { get; }

    public ConversationUpdatedDomainEvent(string conversationId, string type, DateTime occurredAtUtc)
    {
        ConversationId = conversationId;
        Type = type;
        OccurredAtUtc = occurredAtUtc;
    }
}
