namespace TarotNow.Domain.Events;

// Domain event yêu cầu đồng bộ projection conversation/message sau khi timeout completion đã settle ở write model.
public sealed class CompletionTimeoutConversationSyncRequestedDomainEvent : IIdempotentDomainEvent
{
    // Prefix cố định để dễ truy vấn log và phân biệt với event sync khác.
    private const string EventKeyPrefix = "escrow:completion-timeout";

    // Định danh conversation cần đồng bộ projection.
    public string ConversationId { get; }

    // Actor dùng để tạo system message trong timeline.
    public string ActorId { get; }

    // Nội dung system message hiển thị khi auto-resolve completion timeout.
    public string MessageContent { get; }

    // Thời điểm muốn materialize message/conversation update.
    public DateTime ResolvedAtUtc { get; }

    // Mốc phát sinh event theo UTC.
    public DateTime OccurredAtUtc { get; }

    // Khóa idempotency business-level để dedupe xuyên outbox message.
    public string EventIdempotencyKey => $"{EventKeyPrefix}:{ConversationId}";

    /// <summary>
    /// Khởi tạo event đồng bộ completion-timeout projection.
    /// Luồng xử lý: giữ nguyên dữ liệu context conversation để event handler cập nhật Mongo idempotent.
    /// </summary>
    public CompletionTimeoutConversationSyncRequestedDomainEvent(
        string conversationId,
        string actorId,
        string messageContent,
        DateTime resolvedAtUtc)
    {
        ConversationId = conversationId;
        ActorId = actorId;
        MessageContent = messageContent;
        ResolvedAtUtc = resolvedAtUtc;
        OccurredAtUtc = resolvedAtUtc;
    }
}
