namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi user logout hoặc revoke sessions.
/// </summary>
public sealed class UserLoggedOutDomainEvent : IDomainEvent
{
    /// <summary>
    /// User bị revoke session.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session cụ thể bị logout (null nếu revoke all).
    /// </summary>
    public Guid? SessionId { get; init; }

    /// <summary>
    /// Cờ logout all devices.
    /// </summary>
    public bool RevokeAll { get; init; }

    /// <summary>
    /// Lý do revoke.
    /// </summary>
    public string Reason { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
