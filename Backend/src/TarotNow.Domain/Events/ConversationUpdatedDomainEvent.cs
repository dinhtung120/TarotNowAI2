namespace TarotNow.Domain.Events;

// Domain event phát sinh khi conversation thay đổi trạng thái hoặc metadata quan trọng.
public class ConversationUpdatedDomainEvent : IDomainEvent
{
    // Định danh conversation được cập nhật.
    public string ConversationId { get; }

    // Loại cập nhật để subscriber xử lý đúng nhánh.
    public string Type { get; }

    // Thời điểm phát sinh sự kiện (UTC).
    public DateTime OccurredAtUtc { get; }

    /// <summary>
    /// Khởi tạo sự kiện cập nhật conversation cho luồng publish domain event.
    /// Luồng xử lý: nhận conversationId/type và giữ nguyên occurredAtUtc do caller cung cấp.
    /// </summary>
    public ConversationUpdatedDomainEvent(string conversationId, string type, DateTime occurredAtUtc)
    {
        ConversationId = conversationId;
        Type = type;
        OccurredAtUtc = occurredAtUtc;
    }
}
