namespace TarotNow.Infrastructure.Options;

// Options cấu hình tham số hệ thống và bảng giá đọc bài.
public sealed class SystemConfigOptions
{
    // Cấu hình giá các loại trải bài.
    public PricingOptions Pricing { get; set; } = new();

    // Hạn mức AI mỗi ngày.
    public int DailyAiQuota { get; set; } = 3;

    // Số request AI đồng thời tối đa.
    public int InFlightAiCap { get; set; } = 3;

    // Khoảng cách tối thiểu giữa các request đọc bài (giây).
    public int ReadingRateLimitSeconds { get; set; } = 30;

    // Cấu hình follow-up cho phiên đọc bài.
    public FollowupOptions Followup { get; set; } = new();

    // Cấu hình nghiệp vụ rút tiền.
    public WithdrawalOptions Withdrawal { get; set; } = new();

    // Cấu hình presence realtime.
    public PresenceOptions Presence { get; set; } = new();

    // Cấu hình policy escrow.
    public EscrowOptions Escrow { get; set; } = new();

    // Cấu hình policy chat conversation.
    public ChatOptions Chat { get; set; } = new();

    // Cấu hình quy đổi economy.
    public EconomyOptions Economy { get; set; } = new();

    // Cấu hình chi tiết giá theo từng loại trải bài.
    public sealed class PricingOptions
    {
        // Giá Gold cho trải bài 3 lá.
        public long Spread3Gold { get; set; } = 50;

        // Giá Diamond cho trải bài 3 lá.
        public long Spread3Diamond { get; set; } = 5;

        // Giá Gold cho trải bài 5 lá.
        public long Spread5Gold { get; set; } = 100;

        // Giá Diamond cho trải bài 5 lá.
        public long Spread5Diamond { get; set; } = 10;

        // Giá Gold cho trải bài 10 lá.
        public long Spread10Gold { get; set; } = 500;

        // Giá Diamond cho trải bài 10 lá.
        public long Spread10Diamond { get; set; } = 50;
    }

    public sealed class FollowupOptions
    {
        public List<int> PriceTiers { get; set; } = [1, 2, 4, 8, 16];
        public int MaxAllowed { get; set; } = 5;
        public int FreeSlotThresholdLow { get; set; } = 6;
        public int FreeSlotThresholdMid { get; set; } = 11;
        public int FreeSlotThresholdHigh { get; set; } = 16;
    }

    public sealed class WithdrawalOptions
    {
        public long MinDiamond { get; set; } = 500;
        public decimal FeeRate { get; set; } = 0.10m;
    }

    public sealed class PresenceOptions
    {
        public int TimeoutMinutes { get; set; } = 15;
        public int ScanIntervalSeconds { get; set; } = 60;
    }

    public sealed class EscrowOptions
    {
        public int DisputeWindowHours { get; set; } = 48;
        public int ReaderResponseDueHours { get; set; } = 24;
        public int AutoRefundHours { get; set; } = 24;
    }

    public sealed class ChatOptions
    {
        public List<int> AllowedSlaHours { get; set; } = [6, 12, 24];
        public int DefaultSlaHours { get; set; } = 12;
        public int MaxActiveConversationsPerUser { get; set; } = 5;
    }

    public sealed class EconomyOptions
    {
        public long VndPerDiamond { get; set; } = 100;
    }
}
