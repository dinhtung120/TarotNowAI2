using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

public sealed class SystemConfigSettings : ISystemConfigSettings
{
    public SystemConfigSettings(IOptions<SystemConfigOptions> options)
    {
        var config = options.Value;
        Spread3GoldCost = ResolveNonNegativeLong(config.Pricing.Spread3Gold, fallback: 50);
        Spread3DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread3Diamond, fallback: 5);

        Spread5GoldCost = ResolveNonNegativeLong(config.Pricing.Spread5Gold, fallback: 100);
        Spread5DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread5Diamond, fallback: 10);

        Spread10GoldCost = ResolveNonNegativeLong(config.Pricing.Spread10Gold, fallback: 500);
        Spread10DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread10Diamond, fallback: 50);

        DailyAiQuota = ResolvePositiveInt(config.DailyAiQuota, fallback: 3);
        InFlightAiCap = ResolvePositiveInt(config.InFlightAiCap, fallback: 3);
        ReadingRateLimitSeconds = ResolvePositiveInt(config.ReadingRateLimitSeconds, fallback: 30);
    }

    public long Spread3GoldCost { get; }
    public long Spread3DiamondCost { get; }
    public long Spread5GoldCost { get; }
    public long Spread5DiamondCost { get; }
    public long Spread10GoldCost { get; }
    public long Spread10DiamondCost { get; }

    public int DailyAiQuota { get; }
    public int InFlightAiCap { get; }
    public int ReadingRateLimitSeconds { get; }

    private static int ResolvePositiveInt(int configuredValue, int fallback)
    {
        return configuredValue > 0 ? configuredValue : fallback;
    }

    private static long ResolveNonNegativeLong(long configuredValue, long fallback)
    {
        return configuredValue >= 0 ? configuredValue : fallback;
    }
}
