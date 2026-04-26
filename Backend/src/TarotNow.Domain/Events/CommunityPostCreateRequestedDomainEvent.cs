namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu tạo post community theo mô hình command event-only.
/// </summary>
public sealed class CommunityPostCreateRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh tác giả tạo bài.
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Nội dung bài viết.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Mức hiển thị bài viết.
    /// </summary>
    public string Visibility { get; init; } = string.Empty;

    /// <summary>
    /// Draft id gắn media upload trước khi tạo entity thật.
    /// </summary>
    public string? ContextDraftId { get; init; }

    /// <summary>
    /// Id post đã được tạo (được set bởi handler).
    /// </summary>
    public string CreatedPostId { get; set; } = string.Empty;

    /// <summary>
    /// Snapshot display name tác giả để trả về command response.
    /// </summary>
    public string AuthorDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Snapshot avatar tác giả để trả về command response.
    /// </summary>
    public string? AuthorAvatarUrl { get; set; }

    /// <summary>
    /// Thời điểm tạo post theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Trạng thái attach media sau bước create.
    /// </summary>
    public string MediaAttachStatus { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm event phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
