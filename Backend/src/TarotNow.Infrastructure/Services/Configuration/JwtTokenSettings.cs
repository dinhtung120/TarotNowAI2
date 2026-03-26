using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class JwtTokenSettings : IJwtTokenSettings
{
    public JwtTokenSettings(IOptions<JwtOptions> options)
    {
        var jwtOptions = options.Value;
        AccessTokenExpiryMinutes = ResolvePositiveInt(
            jwtOptions.ExpiryMinutes.ToString(),
            fallback: 15);

        RefreshTokenExpiryDays = ResolvePositiveInt(
            jwtOptions.RefreshExpiryDays.ToString(),
            fallback: 7);
    }

    public int AccessTokenExpiryMinutes { get; }
    public int RefreshTokenExpiryDays { get; }

    private static int ResolvePositiveInt(string? configuredValue, int fallback)
    {
        return int.TryParse(configuredValue, out var parsed) && parsed > 0 ? parsed : fallback;
    }
}
