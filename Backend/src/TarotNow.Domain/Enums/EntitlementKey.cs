
namespace TarotNow.Domain.Enums;

// Tập hằng khóa entitlement để ánh xạ quyền lợi theo gói và quy tắc sử dụng.
public static class EntitlementKey
{
    // Miễn phí trải bài 3 lá theo ngày.
    public const string FreeSpread3Daily = "free_spread_3_daily";

    // Miễn phí trải bài 5 lá theo ngày.
    public const string FreeSpread5Daily = "free_spread_5_daily";

    // Miễn phí lượt stream AI theo ngày.
    public const string FreeAiStreamDaily = "free_ai_stream_daily";

    // Quyền lợi hệ số nhân EXP thưởng.
    public const string BonusExpMultiplier = "bonus_exp_multiplier";
}
