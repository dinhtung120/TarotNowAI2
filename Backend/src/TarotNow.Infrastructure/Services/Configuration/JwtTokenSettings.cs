using Microsoft.Extensions.Configuration;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class JwtTokenSettings : IJwtTokenSettings
{
    public JwtTokenSettings(IConfiguration configuration)
    {
        AccessTokenExpiryMinutes = ResolvePositiveInt(
            configuration["Jwt:ExpiryMinutes"] ?? configuration["Jwt:AccessTokenExpirationMinutes"],
            fallback: 15);

        RefreshTokenExpiryDays = ResolvePositiveInt(
            configuration["Jwt:RefreshExpiryDays"] ?? configuration["Jwt:RefreshTokenExpirationDays"],
            fallback: 7);
    }

    public int AccessTokenExpiryMinutes { get; }
    public int RefreshTokenExpiryDays { get; }

    private static int ResolvePositiveInt(string? configuredValue, int fallback)
    {
        return int.TryParse(configuredValue, out var parsed) && parsed > 0 ? parsed : fallback;
    }
}
