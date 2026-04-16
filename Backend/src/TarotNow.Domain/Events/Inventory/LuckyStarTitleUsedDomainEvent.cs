namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi user dùng item Lucky Star.
/// </summary>
public sealed class LuckyStarTitleUsedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng sử dụng item Lucky Star.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã item nguồn.
    /// </summary>
    public string SourceItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
