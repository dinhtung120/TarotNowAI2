using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.Configuration;

// Adapter cấu hình hệ thống: pricing, quota và các tham số vận hành.
public sealed class SystemConfigSettings : ISystemConfigSettings
{
    /// <summary>
    /// Khởi tạo settings hệ thống với cơ chế chuẩn hóa và fallback.
    /// Luồng này đảm bảo các giá trị chi phí/quota luôn nằm trong miền hợp lệ trước khi vào nghiệp vụ.
    /// </summary>
    public SystemConfigSettings(IOptions<SystemConfigOptions> options)
    {
        var config = options.Value;
        // Chuẩn hóa chi phí trải bài để tránh giá âm gây lệch logic trừ ví.
        Spread3GoldCost = ResolveNonNegativeLong(config.Pricing.Spread3Gold, fallback: 50);
        Spread3DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread3Diamond, fallback: 5);

        Spread5GoldCost = ResolveNonNegativeLong(config.Pricing.Spread5Gold, fallback: 100);
        Spread5DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread5Diamond, fallback: 10);

        Spread10GoldCost = ResolveNonNegativeLong(config.Pricing.Spread10Gold, fallback: 500);
        Spread10DiamondCost = ResolveNonNegativeLong(config.Pricing.Spread10Diamond, fallback: 50);

        // Chuẩn hóa quota/rate limit để bảo vệ hệ thống khi cấu hình thiếu hoặc bằng 0.
        DailyAiQuota = ResolvePositiveInt(config.DailyAiQuota, fallback: 3);
        InFlightAiCap = ResolvePositiveInt(config.InFlightAiCap, fallback: 3);
        ReadingRateLimitSeconds = ResolvePositiveInt(config.ReadingRateLimitSeconds, fallback: 30);

        // Giá trị cố định theo chính sách hiện tại, chưa mở cấu hình động.
        DailyCheckinGold = 5;
        StreakFreezeWindowHours = 24;
    }

    // Giá trải bài 3 lá bằng vàng.
    public long Spread3GoldCost { get; }
    // Giá trải bài 3 lá bằng kim cương.
    public long Spread3DiamondCost { get; }
    // Giá trải bài 5 lá bằng vàng.
    public long Spread5GoldCost { get; }
    // Giá trải bài 5 lá bằng kim cương.
    public long Spread5DiamondCost { get; }
    // Giá trải bài 10 lá bằng vàng.
    public long Spread10GoldCost { get; }
    // Giá trải bài 10 lá bằng kim cương.
    public long Spread10DiamondCost { get; }

    // Hạn mức lượt AI mỗi ngày của người dùng.
    public int DailyAiQuota { get; }
    // Số request AI đồng thời tối đa cho mỗi người dùng.
    public int InFlightAiCap { get; }
    // Khoảng giãn tối thiểu giữa hai lần đọc AI.
    public int ReadingRateLimitSeconds { get; }

    // Vàng thưởng check-in mỗi ngày.
    public long DailyCheckinGold { get; }
    // Khung giờ cho phép đóng băng chuỗi streak.
    public int StreakFreezeWindowHours { get; }
    // Chi phí gacha bằng kim cương (đang giữ mặc định 0 khi chưa map cấu hình).
    public long GachaCostDiamond { get; }

    /// <summary>
    /// Chuẩn hóa số nguyên dương cho các tham số giới hạn.
    /// Luồng fallback giữ ổn định khi cấu hình sai hoặc để trống.
    /// </summary>
    private static int ResolvePositiveInt(int configuredValue, int fallback)
    {
        return configuredValue > 0 ? configuredValue : fallback;
    }

    /// <summary>
    /// Chuẩn hóa số không âm cho các tham số chi phí.
    /// Luồng này chặn giá trị âm làm sai lệch phép tính kinh tế trong hệ thống.
    /// </summary>
    private static long ResolveNonNegativeLong(long configuredValue, long fallback)
    {
        return configuredValue >= 0 ? configuredValue : fallback;
    }
}
