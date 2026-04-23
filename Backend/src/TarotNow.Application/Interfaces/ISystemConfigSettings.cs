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

    // Bậc giá follow-up trả phí theo thứ tự lượt hỏi.
    IReadOnlyList<int> FollowupPriceTiers { get; }

    // Số follow-up tối đa cho một phiên đọc bài.
    int FollowupMaxAllowed { get; }

    // Ngưỡng level để được 1 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdLow { get; }

    // Ngưỡng level để được 2 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdMid { get; }

    // Ngưỡng level để được 3 lượt follow-up miễn phí.
    int FollowupFreeSlotThresholdHigh { get; }

    // Ngưỡng tối thiểu Diamond cho một lệnh rút.
    long WithdrawalMinDiamond { get; }

    // Tỷ lệ phí rút (0..1).
    decimal WithdrawalFeeRate { get; }

    // Timeout online presence theo phút.
    int PresenceTimeoutMinutes { get; }

    // Chu kỳ quét timeout presence theo giây.
    int PresenceScanIntervalSeconds { get; }

    // Cửa sổ mở dispute theo giờ.
    int EscrowDisputeWindowHours { get; }

    // Deadline reader phản hồi theo giờ.
    int EscrowReaderResponseDueHours { get; }

    // Deadline tự động refund theo giờ.
    int EscrowAutoRefundHours { get; }
}
