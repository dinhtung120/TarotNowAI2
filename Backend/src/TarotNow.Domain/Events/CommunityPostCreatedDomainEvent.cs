namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi bài viết community mới được tạo.
/// </summary>
public sealed class CommunityPostCreatedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh tác giả bài viết.
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Định danh bài viết mới.
    /// </summary>
    public string PostId { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
