namespace TarotNow.Application.Interfaces;

// Contract cấu hình hệ thống để tập trung các ngưỡng chi phí và hạn mức vận hành.
public interface ISystemConfigSettings
{
    // Chi phí Gold cho trải bài 3 lá.
    long Spread3GoldCost { get; }

    // Chi phí Diamond cho trải bài 3 lá.
    long Spread3DiamondCost { get; }

    // Chi phí Gold cho trải bài 5 lá.
    long Spread5GoldCost { get; }

    // Chi phí Diamond cho trải bài 5 lá.
    long Spread5DiamondCost { get; }

    // Chi phí Gold cho trải bài 10 lá.
    long Spread10GoldCost { get; }

    // Chi phí Diamond cho trải bài 10 lá.
    long Spread10DiamondCost { get; }

    // Hạn mức lượt AI mỗi ngày.
    int DailyAiQuota { get; }

    // Số request AI đồng thời tối đa.
    int InFlightAiCap { get; }

    // Khoảng thời gian chống spam request đọc bài (giây).
    int ReadingRateLimitSeconds { get; }

    // Thưởng Gold cho mỗi lần điểm danh ngày.
    long DailyCheckinGold { get; }

    // Cửa sổ giờ cho phép giữ streak trước khi reset.
    int StreakFreezeWindowHours { get; }

    // Chi phí Diamond cho một lần quay gacha.
    long GachaCostDiamond { get; }
}
