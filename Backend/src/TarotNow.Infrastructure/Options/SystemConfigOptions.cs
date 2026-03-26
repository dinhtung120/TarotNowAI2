namespace TarotNow.Infrastructure.Options;

public sealed class SystemConfigOptions
{
    public PricingOptions Pricing { get; set; } = new();
    public int DailyAiQuota { get; set; } = 3;
    public int InFlightAiCap { get; set; } = 3;
    public int ReadingRateLimitSeconds { get; set; } = 30;

    public sealed class PricingOptions
    {
        public long Spread3Gold { get; set; } = 50;
        public long Spread3Diamond { get; set; } = 5;
        public long Spread5Gold { get; set; } = 100;
        public long Spread5Diamond { get; set; } = 10;
        public long Spread10Gold { get; set; } = 500;
        public long Spread10Diamond { get; set; } = 50;
    }
}

