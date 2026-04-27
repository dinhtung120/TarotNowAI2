namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu đồng bộ projection conversation/message sau khi escrow đã settle ở write-model.
/// </summary>
public sealed class EscrowConversationSyncRequestedDomainEvent : IIdempotentDomainEvent
{
    private const string EventKeyPrefix = "escrow:conversation-sync";

    /// <summary>
    /// Định danh conversation cần đồng bộ.
    /// </summary>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// Trạng thái conversation mục tiêu cần áp dụng.
    /// </summary>
    public string TargetStatus { get; init; } = string.Empty;

    /// <summary>
    /// Actor tạo system message.
    /// </summary>
    public string ActorId { get; init; } = string.Empty;

    /// <summary>
    /// Loại system message.
    /// </summary>
    public string MessageType { get; init; } = string.Empty;

    /// <summary>
    /// Nội dung system message.
    /// </summary>
    public string MessageContent { get; init; } = string.Empty;

    /// <summary>
    /// Lý do sync để tạo khóa idempotency.
    /// </summary>
    public string SyncReason { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm materialize update.
    /// </summary>
    public DateTime ResolvedAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey => $"{EventKeyPrefix}:{SyncReason}:{ConversationId}";
}
