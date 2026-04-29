namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi refresh token rotation thành công.
/// </summary>
public sealed class RefreshTokenRotatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }

    public Guid SessionId { get; init; }

    public Guid OldTokenId { get; init; }

    public Guid NewTokenId { get; init; }

    public string AccessTokenJti { get; init; } = string.Empty;

    public string DeviceId { get; init; } = string.Empty;

    public string IpHash { get; init; } = string.Empty;

    public string UserAgentHash { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
