namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Giá trải bài 3 lá bằng vàng.
    public long Spread3GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_3.gold", "reading_cost_spread_3_gold"], _options.Pricing.Spread3Gold),
        fallback: 50);

    // Giá trải bài 3 lá bằng kim cương.
    public long Spread3DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_3.diamond"], _options.Pricing.Spread3Diamond),
        fallback: 5);

    // Giá trải bài 5 lá bằng vàng.
    public long Spread5GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_5.gold", "reading_cost_spread_5_gold"], _options.Pricing.Spread5Gold),
        fallback: 100);

    // Giá trải bài 5 lá bằng kim cương.
    public long Spread5DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_5.diamond"], _options.Pricing.Spread5Diamond),
        fallback: 10);

    // Giá trải bài 10 lá bằng vàng.
    public long Spread10GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_10.gold", "reading_cost_spread_10_gold"], _options.Pricing.Spread10Gold),
        fallback: 500);

    // Giá trải bài 10 lá bằng kim cương.
    public long Spread10DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_10.diamond"], _options.Pricing.Spread10Diamond),
        fallback: 50);

    // Hạn mức AI mỗi ngày của người dùng.
    public int DailyAiQuota => ResolvePositiveInt(
        ReadInt(["ai.daily_quota", "ai_daily_quota_free"], _options.DailyAiQuota),
        fallback: 3);

    // Số request AI đồng thời tối đa cho mỗi người dùng.
    public int InFlightAiCap => ResolvePositiveInt(
        ReadInt(["ai.in_flight_cap", "ai_in_flight_cap"], _options.InFlightAiCap),
        fallback: 3);

    // Khoảng giãn tối thiểu giữa hai lần đọc AI.
    public int ReadingRateLimitSeconds => ResolvePositiveInt(
        ReadInt(["reading.rate_limit_seconds"], _options.ReadingRateLimitSeconds),
        fallback: 30);

    // Vàng thưởng check-in mỗi ngày.
    public long DailyCheckinGold => ResolveNonNegativeLong(
        ReadLong(["checkin.daily_gold", "daily_checkin_gold"], fallback: 5),
        fallback: 5);

    // Khung giờ cho phép đóng băng chuỗi streak.
    public int StreakFreezeWindowHours => ResolvePositiveInt(
        ReadInt(["streak.freeze_window_hours", "streak_freeze_window_hours"], fallback: 24),
        fallback: 24);

    // Chi phí gacha bằng kim cương.
    public long GachaCostDiamond => ResolveNonNegativeLong(
        ReadLong(["gacha.cost_diamond", "gacha_cost_diamond"], fallback: 5),
        fallback: 5);
}
