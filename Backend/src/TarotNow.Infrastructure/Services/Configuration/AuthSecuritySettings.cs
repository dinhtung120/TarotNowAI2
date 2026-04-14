using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

/// <summary>
/// Adapter ánh xạ AuthSecurityOptions ở Infrastructure sang contract Application.
/// </summary>
public sealed class AuthSecuritySettings : IAuthSecuritySettings
{
    /// <summary>
    /// Khởi tạo settings bảo mật auth.
    /// </summary>
    public AuthSecuritySettings(IOptions<AuthSecurityOptions> options)
    {
        var value = options.Value;
        AccessTokenBlacklistTtlSeconds = value.AccessTokenBlacklistTtlSeconds > 0
            ? value.AccessTokenBlacklistTtlSeconds
            : 1200;
        SessionRevocationTtlSeconds = value.SessionRevocationTtlSeconds > 0
            ? value.SessionRevocationTtlSeconds
            : 1800;
    }

    /// <inheritdoc />
    public int AccessTokenBlacklistTtlSeconds { get; }

    /// <inheritdoc />
    public int SessionRevocationTtlSeconds { get; }
}
