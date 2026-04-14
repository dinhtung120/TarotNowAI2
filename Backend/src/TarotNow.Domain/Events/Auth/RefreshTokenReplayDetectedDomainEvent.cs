namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi phát hiện refresh token replay.
/// </summary>
public sealed class RefreshTokenReplayDetectedDomainEvent : IDomainEvent
{
    /// <summary>
    /// User sở hữu token bị replay.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session bị ảnh hưởng.
    /// </summary>
    public Guid SessionId { get; init; }

    /// <summary>
    /// Family id của token bị compromise.
    /// </summary>
    public Guid FamilyId { get; init; }

    /// <summary>
    /// Hash ip nguồn phát hiện replay.
    /// </summary>
    public string SourceIpHash { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
