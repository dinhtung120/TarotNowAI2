namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu worker attach media community cho entity vừa tạo.
/// </summary>
public sealed class CommunityMediaAttachRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Chủ sở hữu media.
    /// </summary>
    public Guid OwnerUserId { get; init; }

    /// <summary>
    /// Context type (`post` hoặc `comment`).
    /// </summary>
    public string ContextType { get; init; } = string.Empty;

    /// <summary>
    /// Draft id để attach các asset upload trước.
    /// </summary>
    public string? ContextDraftId { get; init; }

    /// <summary>
    /// Entity id sau khi tạo thành công.
    /// </summary>
    public string ContextEntityId { get; init; } = string.Empty;

    /// <summary>
    /// Nội dung markdown để trích object keys cần attach.
    /// </summary>
    public string MarkdownContent { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm event phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
