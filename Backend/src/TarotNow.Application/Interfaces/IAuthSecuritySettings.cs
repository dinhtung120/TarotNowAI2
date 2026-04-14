namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract đọc các ngưỡng bảo mật auth để tránh hard-code trong handlers.
/// </summary>
public interface IAuthSecuritySettings
{
    /// <summary>
    /// TTL deny-list access token (theo jti) tính bằng giây.
    /// </summary>
    int AccessTokenBlacklistTtlSeconds { get; }

    /// <summary>
    /// TTL cờ session revoked tính bằng giây.
    /// </summary>
    int SessionRevocationTtlSeconds { get; }

    /// <summary>
    /// TTL cache session snapshot (theo giây).
    /// </summary>
    int SessionCacheTtlSeconds { get; }
}
