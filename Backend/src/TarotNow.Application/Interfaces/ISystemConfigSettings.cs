namespace TarotNow.Application.Interfaces;

/// <summary>
/// Cấu hình runtime cho pricing và giới hạn thao tác đọc bài/AI.
/// </summary>
public interface ISystemConfigSettings
{
    long Spread3GoldCost { get; }
    long Spread3DiamondCost { get; }
    long Spread5GoldCost { get; }
    long Spread5DiamondCost { get; }
    long Spread10GoldCost { get; }
    long Spread10DiamondCost { get; }

    int DailyAiQuota { get; }
    int InFlightAiCap { get; }
    int ReadingRateLimitSeconds { get; }
}
