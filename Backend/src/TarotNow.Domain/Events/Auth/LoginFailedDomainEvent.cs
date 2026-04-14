namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi login thất bại.
/// </summary>
public sealed class LoginFailedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Identity hash của thông tin đăng nhập (email/username).
    /// </summary>
    public string IdentityHash { get; init; } = string.Empty;

    /// <summary>
    /// Ip hash của request.
    /// </summary>
    public string IpHash { get; init; } = string.Empty;

    /// <summary>
    /// Mã lý do thất bại.
    /// </summary>
    public string ReasonCode { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
