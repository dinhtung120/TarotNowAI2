namespace TarotNow.Infrastructure.Options;

/// <summary>
/// Options bảo mật riêng cho auth/session rotation.
/// </summary>
public sealed class AuthSecurityOptions
{
    /// <summary>
    /// Thời gian giữ lock refresh token để tránh race condition.
    /// </summary>
    public int RefreshLockSeconds { get; set; } = 15;

    /// <summary>
    /// Cửa sổ idempotency cho refresh token.
    /// </summary>
    public int RefreshIdempotencyWindowSeconds { get; set; } = 60;

    /// <summary>
    /// TTL cho deny-list access token theo jti khi logout/replay.
    /// </summary>
    public int AccessTokenBlacklistTtlSeconds { get; set; } = 1200;

    /// <summary>
    /// TTL cho cờ session bị revoke để chặn access token cũ theo sid.
    /// </summary>
    public int SessionRevocationTtlSeconds { get; set; } = 1800;

    /// <summary>
    /// TTL cache snapshot session active (Redis).
    /// </summary>
    public int SessionCacheTtlSeconds { get; set; } = 30 * 24 * 60 * 60;
}
