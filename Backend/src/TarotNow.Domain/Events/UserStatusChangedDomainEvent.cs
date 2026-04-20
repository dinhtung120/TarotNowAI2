namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi trạng thái hiện diện của user thay đổi.
/// </summary>
public sealed class UserStatusChangedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh user.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Trạng thái hiện diện (online/offline).
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm phát sinh theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
