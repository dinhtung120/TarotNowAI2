namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu đồng bộ nội dung ReadingSession ở Mongo sau khi AI stream hoàn tất.
/// </summary>
public sealed class ReadingSessionContentSyncRequestedDomainEvent : IIdempotentDomainEvent
{
    private const string EventKeyPrefix = "reading:session-content-sync";

    /// <summary>
    /// Định danh reading session cần đồng bộ.
    /// </summary>
    public string SessionId { get; init; } = string.Empty;

    /// <summary>
    /// Định danh AI request dùng để dedupe follow-up.
    /// </summary>
    public Guid AiRequestId { get; init; }

    /// <summary>
    /// Câu hỏi follow-up (nếu có).
    /// </summary>
    public string? FollowupQuestion { get; init; }

    /// <summary>
    /// Nội dung response đầy đủ từ provider.
    /// </summary>
    public string FullResponse { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventIdempotencyKey => $"{EventKeyPrefix}:{AiRequestId:N}";
}
