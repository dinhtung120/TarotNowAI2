namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi một phiên đọc bài hoàn tất.
/// </summary>
public sealed class ReadingCompletedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng hoàn tất phiên đọc.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
