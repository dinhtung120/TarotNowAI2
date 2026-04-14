namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi refresh token rotate thành công.
/// </summary>
public sealed class TokenRefreshedDomainEvent : IDomainEvent
{
    /// <summary>
    /// User sở hữu token.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session chứa token.
    /// </summary>
    public Guid SessionId { get; init; }

    /// <summary>
    /// Token cũ.
    /// </summary>
    public Guid OldTokenId { get; init; }

    /// <summary>
    /// Token mới.
    /// </summary>
    public Guid NewTokenId { get; init; }

    /// <summary>
    /// JWT jti mới cấp cho access token.
    /// </summary>
    public string AccessTokenJti { get; init; } = string.Empty;

    /// <summary>
    /// Device id của request refresh.
    /// </summary>
    public string DeviceId { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
