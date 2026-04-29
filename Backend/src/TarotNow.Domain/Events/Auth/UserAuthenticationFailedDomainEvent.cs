namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi xác thực đăng nhập thất bại.
/// </summary>
public sealed class UserAuthenticationFailedDomainEvent : IDomainEvent
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

    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
