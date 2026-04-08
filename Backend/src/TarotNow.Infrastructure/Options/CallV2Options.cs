namespace TarotNow.Infrastructure.Options;

public sealed class CallV2Options
{
    public int RingTimeoutSeconds { get; set; } = 60;

    public int JoinTimeoutSeconds { get; set; } = 45;

    public int ReconnectGracePeriodSeconds { get; set; } = 15;

    public int TimeoutSweepIntervalSeconds { get; set; } = 15;
}
