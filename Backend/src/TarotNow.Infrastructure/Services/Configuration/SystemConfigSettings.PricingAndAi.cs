namespace TarotNow.Infrastructure.Services.Configuration;

public sealed partial class SystemConfigSettings
{
    // Giá trải bài 3 lá bằng vàng.
    public long Spread3GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_3.gold"], _options.Pricing.Spread3Gold),
        fallback: Math.Max(0, _options.Pricing.Spread3Gold));

    // Giá trải bài 3 lá bằng kim cương.
    public long Spread3DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_3.diamond"], _options.Pricing.Spread3Diamond),
        fallback: Math.Max(0, _options.Pricing.Spread3Diamond));

    // Giá trải bài 5 lá bằng vàng.
    public long Spread5GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_5.gold"], _options.Pricing.Spread5Gold),
        fallback: Math.Max(0, _options.Pricing.Spread5Gold));

    // Giá trải bài 5 lá bằng kim cương.
    public long Spread5DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_5.diamond"], _options.Pricing.Spread5Diamond),
        fallback: Math.Max(0, _options.Pricing.Spread5Diamond));

    // Giá trải bài 10 lá bằng vàng.
    public long Spread10GoldCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_10.gold"], _options.Pricing.Spread10Gold),
        fallback: Math.Max(0, _options.Pricing.Spread10Gold));

    // Giá trải bài 10 lá bằng kim cương.
    public long Spread10DiamondCost => ResolveNonNegativeLong(
        ReadLong(["pricing.spread_10.diamond"], _options.Pricing.Spread10Diamond),
        fallback: Math.Max(0, _options.Pricing.Spread10Diamond));

    // Hạn mức AI mỗi ngày của người dùng.
    public int DailyAiQuota => ResolvePositiveInt(
        ReadInt(["ai.daily_quota"], _options.DailyAiQuota),
        fallback: Math.Max(1, _options.DailyAiQuota));

    // Số request AI đồng thời tối đa cho mỗi người dùng.
    public int InFlightAiCap => ResolvePositiveInt(
        ReadInt(["ai.in_flight_cap"], _options.InFlightAiCap),
        fallback: Math.Max(1, _options.InFlightAiCap));

    // Khoảng giãn tối thiểu giữa hai lần đọc AI.
    public int ReadingRateLimitSeconds => ResolvePositiveInt(
        ReadInt(["reading.rate_limit_seconds"], _options.ReadingRateLimitSeconds),
        fallback: Math.Max(1, _options.ReadingRateLimitSeconds));

    // Vàng thưởng check-in mỗi ngày.
    public long DailyCheckinGold => ResolveNonNegativeLong(
        ReadLong(["checkin.daily_gold"], _options.DailyCheckinGold),
        fallback: Math.Max(0, _options.DailyCheckinGold));

    // Khung giờ cho phép đóng băng chuỗi streak.
    public int StreakFreezeWindowHours => ResolvePositiveInt(
        ReadInt(["streak.freeze_window_hours"], _options.StreakFreezeWindowHours),
        fallback: Math.Max(1, _options.StreakFreezeWindowHours));

    // Chi phí gacha bằng kim cương.
    public long GachaCostDiamond => ResolveNonNegativeLong(
        ReadLong(["gacha.cost_diamond"], _options.GachaCostDiamond),
        fallback: Math.Max(0, _options.GachaCostDiamond));

    // EXP cơ bản mỗi lá bài khi reveal.
    public decimal ProgressionReadingExpPerCard
    {
        get
        {
            var raw = ReadDecimal("progression.reading.exp_per_card");
            var configured = raw ?? _options.Progression.ReadingExpPerCard;
            return ClampDecimal(configured, 0m, 100m);
        }
    }

    // Hệ số nhân EXP cho non-daily khi dùng Diamond.
    public decimal ProgressionReadingDiamondMultiplierNonDaily
    {
        get
        {
            var raw = ReadDecimal("progression.reading.diamond_multiplier_non_daily");
            var configured = raw ?? _options.Progression.ReadingDiamondMultiplierNonDaily;
            return ClampDecimal(configured, 0m, 10m);
        }
    }
}
