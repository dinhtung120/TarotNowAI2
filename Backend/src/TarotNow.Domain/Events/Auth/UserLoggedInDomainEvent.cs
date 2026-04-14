namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi user đăng nhập thành công.
/// </summary>
public sealed class UserLoggedInDomainEvent : IDomainEvent
{
    /// <summary>
    /// User đăng nhập.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Session được tạo.
    /// </summary>
    public Guid SessionId { get; init; }

    /// <summary>
    /// Device id của session.
    /// </summary>
    public string DeviceId { get; init; } = string.Empty;

    /// <summary>
    /// User-agent hash tại thời điểm login.
    /// </summary>
    public string UserAgentHash { get; init; } = string.Empty;

    /// <summary>
    /// Ip hash tại thời điểm login.
    /// </summary>
    public string IpHash { get; init; } = string.Empty;

    /// <summary>
    /// JTI của access token cấp khi login thành công.
    /// </summary>
    public string AccessTokenJti { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
