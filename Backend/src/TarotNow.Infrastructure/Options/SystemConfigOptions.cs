namespace TarotNow.Infrastructure.Options;

// Options cấu hình tham số hệ thống và bảng giá đọc bài.
public sealed partial class SystemConfigOptions
{
    // Cấu hình giá các loại trải bài.
    public PricingOptions Pricing { get; set; } = new();

    // Hạn mức AI mỗi ngày.
    public int DailyAiQuota { get; set; } = 3;

    // Số request AI đồng thời tối đa.
    public int InFlightAiCap { get; set; } = 3;

    // Khoảng cách tối thiểu giữa các request đọc bài (giây).
    public int ReadingRateLimitSeconds { get; set; } = 30;

    // Gold thưởng check-in hàng ngày.
    public long DailyCheckinGold { get; set; } = 5;

    // Cửa sổ giờ cho streak freeze.
    public int StreakFreezeWindowHours { get; set; } = 24;

    // Chi phí kim cương mặc định cho một lượt quay gacha.
    public long GachaCostDiamond { get; set; } = 5;

    // Cấu hình follow-up cho phiên đọc bài.
    public FollowupOptions Followup { get; set; } = new();

    // Cấu hình nghiệp vụ rút tiền.
    public WithdrawalOptions Withdrawal { get; set; } = new();

    // Cấu hình presence realtime.
    public PresenceOptions Presence { get; set; } = new();

    // Cấu hình policy escrow.
    public EscrowOptions Escrow { get; set; } = new();

    // Cấu hình policy resolve dispute phía admin.
    public AdminDisputeOptions AdminDispute { get; set; } = new();

    // Cấu hình progression cho reading.
    public ProgressionOptions Progression { get; set; } = new();

    // Cấu hình reward inventory.
    public InventoryOptions Inventory { get; set; } = new();

    // Cấu hình policy chat conversation.
    public ChatOptions Chat { get; set; } = new();

    // Cấu hình policy pháp lý/auth.
    public AuthOptions Auth { get; set; } = new();

    // Cấu hình realtime FE.
    public RealtimeOptions Realtime { get; set; } = new();

    // Cấu hình operational FE/BE.
    public OperationalOptions Operational { get; set; } = new();

    // Cấu hình UI defaults FE.
    public UiOptions Ui { get; set; } = new();

    // Cấu hình upload media FE.
    public MediaUploadOptions MediaUpload { get; set; } = new();

    // Cấu hình quy đổi economy.
    public EconomyOptions Economy { get; set; } = new();

    // Cấu hình ràng buộc business cho Reader.
    public ReaderOptions Reader { get; set; } = new();

    // Cấu hình default cho gamification.
    public GamificationOptions Gamification { get; set; } = new();

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
        public int DisputeMinReasonLength { get; set; } = 10;
        public int ReaderResponseDueHours { get; set; } = 24;
        public int AutoRefundHours { get; set; } = 24;
    }

    public sealed class AdminDisputeOptions
    {
        public int DefaultSplitPercentToReader { get; set; } = 50;
        public int ReaderFreezeLookbackDays { get; set; } = 7;
        public int ReaderFreezeThreshold { get; set; } = 3;
    }

    public sealed class ProgressionOptions
    {
        public decimal ReadingExpPerCard { get; set; } = 1m;
        public decimal ReadingDiamondMultiplierNonDaily { get; set; } = 2m;
    }

    public sealed class InventoryOptions
    {
        public LuckyStarOptions LuckyStar { get; set; } = new();

        public sealed class LuckyStarOptions
        {
            public long OwnedTitleGoldReward { get; set; } = 500;
        }
    }

    public sealed class ChatOptions
    {
        public List<int> AllowedSlaHours { get; set; } = [6, 12, 24];
        public int DefaultSlaHours { get; set; } = 12;
        public int MaxActiveConversationsPerUser { get; set; } = 5;
        public PaymentOfferOptions PaymentOffer { get; set; } = new();
        public HistoryOptions History { get; set; } = new();
        public ParticipantsOptions Participants { get; set; } = new();

        public sealed class PaymentOfferOptions
        {
            public long DefaultAmount { get; set; } = 10;
            public int MaxNoteLength { get; set; } = 100;
        }

        public sealed class HistoryOptions
        {
            public int PageSize { get; set; } = 50;
        }

        public sealed class ParticipantsOptions
        {
            public int DefaultPageSize { get; set; } = 50;
            public int MaxPageSize { get; set; } = 200;
        }
    }

    public sealed class AuthOptions
    {
        public int MinimumAge { get; set; } = 18;
    }

}
