namespace TarotNow.Application.Interfaces;

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

    long DailyCheckinGold { get; }
    int StreakFreezeWindowHours { get; }
    
    long GachaCostDiamond { get; }
}
