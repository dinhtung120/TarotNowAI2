using Microsoft.Extensions.Configuration;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class SystemConfigSettings : ISystemConfigSettings
{
    public SystemConfigSettings(IConfiguration configuration)
    {
        Spread3GoldCost = ResolveNonNegativeLong(configuration["SystemConfig:Pricing:Spread3Gold"], fallback: 50);
        Spread5GoldCost = ResolveNonNegativeLong(configuration["SystemConfig:Pricing:Spread5Gold"], fallback: 100);
        Spread10DiamondCost = ResolveNonNegativeLong(configuration["SystemConfig:Pricing:Spread10Diamond"], fallback: 50);

        DailyAiQuota = ResolvePositiveInt(configuration["SystemConfig:DailyAiQuota"], fallback: 3);
        InFlightAiCap = ResolvePositiveInt(configuration["SystemConfig:InFlightAiCap"], fallback: 3);
        ReadingRateLimitSeconds = ResolvePositiveInt(configuration["SystemConfig:ReadingRateLimitSeconds"], fallback: 30);
    }

    public long Spread3GoldCost { get; }
    public long Spread5GoldCost { get; }
    public long Spread10DiamondCost { get; }

    public int DailyAiQuota { get; }
    public int InFlightAiCap { get; }
    public int ReadingRateLimitSeconds { get; }

    private static int ResolvePositiveInt(string? configuredValue, int fallback)
    {
        return int.TryParse(configuredValue, out var parsed) && parsed > 0 ? parsed : fallback;
    }

    private static long ResolveNonNegativeLong(string? configuredValue, long fallback)
    {
        return long.TryParse(configuredValue, out var parsed) && parsed >= 0 ? parsed : fallback;
    }
}
