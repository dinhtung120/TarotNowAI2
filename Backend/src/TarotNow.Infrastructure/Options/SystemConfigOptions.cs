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

    public sealed class RealtimeOptions
    {
        public List<int> ReconnectScheduleMs { get; set; } = [0, 2_000, 5_000, 10_000, 30_000];
        public int NegotiationTimeoutMs { get; set; } = 8_000;
        public int PresenceNegotiationCooldownMs { get; set; } = 45_000;
        public int ChatUnauthorizedCooldownMs { get; set; } = 60_000;
        public int ServerTimeoutMs { get; set; } = 120_000;
        public int ChatTypingClearMs { get; set; } = 2_500;
        public int ChatInvalidateDebounceMs { get; set; } = 1_000;
        public int ChatInitialLoadGuardMs { get; set; } = 2_000;
        public int ChatAppStartGuardMs { get; set; } = 3_000;
    }

    public sealed class OperationalOptions
    {
        public HttpOptions Http { get; set; } = new();
        public RuntimePoliciesOptions RuntimePolicies { get; set; } = new();
        public RedisOptions Redis { get; set; } = new();
        public AiOptions Ai { get; set; } = new();
        public OutboxOptions Outbox { get; set; } = new();

        public sealed class HttpOptions
        {
            public int ClientTimeoutMs { get; set; } = 8_000;
            public int ServerTimeoutMs { get; set; } = 8_000;
            public int MinTimeoutMs { get; set; } = 1_000;
        }

        public sealed class RuntimePoliciesOptions
        {
            public int TimeoutMs { get; set; } = 8_000;
            public int StaleMs { get; set; } = 15_000;
        }

        public sealed class RedisOptions
        {
            public int ConnectTimeoutMs { get; set; } = 2_000;
            public int SyncTimeoutMs { get; set; } = 2_000;
            public int ConnectRetry { get; set; } = 1;
        }

        public sealed class AiOptions
        {
            public int TimeoutSeconds { get; set; } = 30;
            public int MaxRetries { get; set; } = 2;
            public int StreamingRetryBaseDelayMs { get; set; } = 200;
            public double StreamingTemperature { get; set; } = 0.7;
        }

        public sealed class OutboxOptions
        {
            public int BatchSize { get; set; } = 50;
            public int MaxRetryAttempts { get; set; } = 12;
            public int LockTimeoutSeconds { get; set; } = 120;
            public int MaxBackoffSeconds { get; set; } = 300;
            public int PollIntervalSeconds { get; set; } = 5;
        }
    }

    public sealed class UiOptions
    {
        public ReadersOptions Readers { get; set; } = new();
        public SearchOptions Search { get; set; } = new();
        public PrefetchOptions Prefetch { get; set; } = new();

        public sealed class ReadersOptions
        {
            public int DirectoryPageSize { get; set; } = 12;
            public int FeaturedLimit { get; set; } = 4;
            public int DirectoryStaleMs { get; set; } = 30_000;
        }

        public sealed class SearchOptions
        {
            public int DebounceMs { get; set; } = 300;
        }

        public sealed class PrefetchOptions
        {
            public int ChatInboxStaleMs { get; set; } = 30_000;
        }
    }

    public sealed class MediaUploadOptions
    {
        public long MaxImageBytes { get; set; } = 10L * 1024 * 1024;
        public long MaxVoiceBytes { get; set; } = 5L * 1024 * 1024;
        public int MaxVoiceDurationMs { get; set; } = 600_000;
        public long ImageCompressionTargetBytes { get; set; } = 80L * 1024;
        public List<ImageCompressionStepOptions> ImageCompressionSteps { get; set; } =
        [
            new() { InitialQuality = 0.68, MaxSizeMb = 0.15, MaxWidthOrHeight = 1440 },
            new() { InitialQuality = 0.58, MaxSizeMb = 0.10, MaxWidthOrHeight = 1200 },
            new() { InitialQuality = 0.48, MaxSizeMb = 0.06, MaxWidthOrHeight = 960 },
            new() { InitialQuality = 0.38, MaxSizeMb = 0.03, MaxWidthOrHeight = 640 }
        ];
        public int RetryAttempts { get; set; } = 3;
        public int RetryDelayMs { get; set; } = 350;

        public sealed class ImageCompressionStepOptions
        {
            public double InitialQuality { get; set; }
            public double MaxSizeMb { get; set; }
            public int MaxWidthOrHeight { get; set; }
        }
    }

    public sealed class EconomyOptions
    {
        public long VndPerDiamond { get; set; } = 100;
    }

    public sealed class ReaderOptions
    {
        public int MinYearsOfExperience { get; set; } = 1;
        public long MinDiamondPerQuestion { get; set; } = 50;
    }

    public sealed class GamificationOptions
    {
        public string DefaultQuestType { get; set; } = "daily";
        public string DefaultLeaderboardTrack { get; set; } = "spent_gold_daily";
    }
}
