namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi người dùng check-in hằng ngày thành công.
/// </summary>
public sealed class DailyCheckInCompletedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh người dùng check-in.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Streak hiện tại sau khi check-in.
    /// </summary>
    public int CurrentStreak { get; init; }

    /// <summary>
    /// Business date của lần check-in.
    /// </summary>
    public string BusinessDate { get; init; } = string.Empty;

    /// <summary>
    /// Vàng thưởng check-in.
    /// </summary>
    public long GoldRewarded { get; init; }

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
