using System.Text.Json;
using System.Text.RegularExpressions;

namespace TarotNow.Application.Common.SystemConfigs;

public static partial class SystemConfigRegistry
{
    private static IReadOnlyList<SystemConfigDefinition> BuildDefinitions()
    {
        var definitions = new List<SystemConfigDefinition>(BuildScalarDefinitions());
        definitions.AddRange(BuildJsonDefinitions());
        return definitions;
    }

    private static IEnumerable<SystemConfigDefinition> BuildScalarDefinitions()
    {
        return
        [
            .. BuildPricingScalarDefinitions(),
            .. BuildCorePolicyScalarDefinitions(),
            .. BuildReaderFollowupGamificationScalarDefinitions(),
            .. BuildOperationalScalarDefinitions(),
            .. BuildDisputeProgressionLegalScalarDefinitions(),
            .. BuildRuntimeOperationalScalarDefinitions(),
            .. BuildUiAndMediaScalarDefinitions()
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildPricingScalarDefinitions()
    {
        return
        [
            Scalar("pricing.spread_3.gold", "50", "Giá Gold cho trải bài 3 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_3.diamond", "5", "Giá Diamond cho trải bài 3 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_5.gold", "100", "Giá Gold cho trải bài 5 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_5.diamond", "10", "Giá Diamond cho trải bài 5 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_10.gold", "500", "Giá Gold cho trải bài 10 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_10.diamond", "50", "Giá Diamond cho trải bài 10 lá.", ValidateLong(0, 10_000_000))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildCorePolicyScalarDefinitions()
    {
        return
        [
            Scalar("ai.daily_quota", "3", "Hạn mức request AI theo ngày.", ValidateInt(1, 10000)),
            Scalar("ai.in_flight_cap", "3", "Số request AI đồng thời tối đa mỗi user.", ValidateInt(1, 100)),
            Scalar("reading.rate_limit_seconds", "30", "Khoảng cách tối thiểu giữa 2 lần đọc bài AI.", ValidateInt(1, 300)),
            Scalar("checkin.daily_gold", "5", "Gold thưởng check-in hằng ngày.", ValidateLong(0, 1_000_000)),
            Scalar("streak.freeze_window_hours", "24", "Cửa sổ mua streak freeze theo giờ.", ValidateInt(1, 168)),
            Scalar("gacha.cost_diamond", "5", "Chi phí Diamond mặc định cho một lượt quay gacha.", ValidateLong(0, 1_000_000)),
            Scalar("withdrawal.min_diamond", "500", "Ngưỡng Diamond tối thiểu cho một lệnh rút.", ValidateLong(0, 10_000_000)),
            Scalar("withdrawal.fee_rate", "0.10", "Tỷ lệ phí rút (0-1).", ValidateDecimal(0m, 1m)),
            Scalar("chat.default_sla_hours", "12", "SLA mặc định cho conversation (giờ).", ValidateInt(1, 168)),
            Scalar("chat.max_active_conversations_per_user", "5", "Số conversation active tối đa mỗi user.", ValidateInt(1, 200)),
            Scalar("economy.vnd_per_diamond", "100", "Tỷ giá quy đổi 1 Diamond sang VND.", ValidateLong(1, 1_000_000))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildReaderFollowupGamificationScalarDefinitions()
    {
        return
        [
            Scalar("reader.min_years_of_experience", "1", "Số năm kinh nghiệm tối thiểu cho Reader.", ValidateInt(1, 100)),
            Scalar("reader.min_diamond_per_question", "50", "Mức Diamond/câu hỏi tối thiểu của Reader.", ValidateLong(1, 1_000_000)),
            Scalar("followup.max_allowed", "5", "Số follow-up tối đa trong một phiên.", ValidateInt(1, 20)),
            Scalar("followup.free_slots.threshold_high", "16", "Ngưỡng level để có 3 lượt free.", ValidateInt(1, 200)),
            Scalar("followup.free_slots.threshold_mid", "11", "Ngưỡng level để có 2 lượt free.", ValidateInt(1, 200)),
            Scalar("followup.free_slots.threshold_low", "6", "Ngưỡng level để có 1 lượt free.", ValidateInt(1, 200)),
            Scalar(
                "gamification.default_quest_type",
                "daily",
                "Quest type mặc định khi mở trang gamification.",
                ValidateGamificationDefaultQuestType),
            Scalar(
                "gamification.default_leaderboard_track",
                "spent_gold_daily",
                "Leaderboard track mặc định khi mở trang leaderboard.",
                ValidateGamificationDefaultLeaderboardTrack)
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildOperationalScalarDefinitions()
    {
        return
        [
            Scalar("presence.timeout_minutes", "15", "Ngưỡng timeout online presence theo phút.", ValidateInt(1, 240)),
            Scalar("presence.scan_interval_seconds", "60", "Chu kỳ quét timeout presence theo giây.", ValidateInt(5, 600)),
            Scalar("escrow.dispute_window_hours", "48", "Cửa sổ mở dispute theo giờ.", ValidateInt(1, 720)),
            Scalar("escrow.dispute.min_reason_length", "10", "Độ dài tối thiểu lý do mở dispute.", ValidateInt(1, 2000)),
            Scalar("escrow.reader_response_due_hours", "24", "Deadline reader trả lời sau khi nhận câu hỏi.", ValidateInt(1, 168)),
            Scalar("escrow.auto_refund_hours", "24", "Deadline tự động refund theo giờ.", ValidateInt(1, 168)),
            Scalar("deposit.link_expiry_minutes", "15", "Thời gian sống payment link nạp tiền.", ValidateInt(1, 120))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildDisputeProgressionLegalScalarDefinitions()
    {
        return
        [
            Scalar("admin.dispute.default_split_percent_to_reader", "50", "Split % mặc định cho Reader khi resolve dispute.", ValidateInt(1, 99)),
            Scalar("admin.dispute.reader_freeze.lookback_days", "7", "Số ngày lookback để xét freeze Reader theo dispute.", ValidateInt(1, 365)),
            Scalar("admin.dispute.reader_freeze.threshold", "3", "Ngưỡng dispute gần đây để freeze Reader.", ValidateInt(1, 100)),
            Scalar("progression.reading.exp_per_card", "1.0", "EXP cơ bản mỗi lá bài khi reveal reading.", ValidateDecimal(0m, 100m)),
            Scalar("progression.reading.diamond_multiplier_non_daily", "2.0", "Hệ số nhân EXP cho non-daily khi trả bằng Diamond.", ValidateDecimal(0m, 10m)),
            Scalar("inventory.lucky_star.owned_title_gold_reward", "500", "Gold thưởng khi dùng Lucky Star đã sở hữu title.", ValidateLong(0, 100_000_000)),
            Scalar("legal.minimum_age", "18", "Tuổi tối thiểu để đăng ký tài khoản.", ValidateInt(13, 100))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildRuntimeOperationalScalarDefinitions()
    {
        return
        [
            Scalar("chat.payment_offer.default_amount", "10", "Diamond mặc định đề xuất trong payment offer.", ValidateLong(1, 1_000_000)),
            Scalar("chat.payment_offer.max_note_length", "100", "Độ dài tối đa ghi chú payment offer.", ValidateInt(1, 4000)),
            Scalar("chat.history.page_size", "50", "Page size lịch sử message trong chat room.", ValidateInt(1, 200)),
            Scalar("chat.participants.default_page_size", "50", "Page size mặc định khi query conversation ids theo participant.", ValidateInt(1, 500)),
            Scalar("chat.participants.max_page_size", "200", "Page size tối đa khi query conversation ids theo participant.", ValidateInt(1, 1000)),
            Scalar("realtime.negotiation_timeout_ms", "8000", "Timeout negotiation SignalR theo ms.", ValidateInt(500, 120000)),
            Scalar("realtime.presence_negotiation_cooldown_ms", "45000", "Cooldown retry presence realtime khi lỗi negotiation.", ValidateInt(0, 600000)),
            Scalar("realtime.chat_unauthorized_cooldown_ms", "60000", "Cooldown retry chat realtime khi unauthorized.", ValidateInt(0, 600000)),
            Scalar("realtime.server_timeout_ms", "120000", "SignalR server timeout theo ms.", ValidateInt(5000, 600000)),
            Scalar("realtime.chat.typing_clear_ms", "2500", "Thời gian clear typing indicator (ms).", ValidateInt(100, 60000)),
            Scalar("realtime.chat.invalidate_debounce_ms", "1000", "Debounce invalidate query realtime (ms).", ValidateInt(0, 10000)),
            Scalar("realtime.chat.initial_load_guard_ms", "2000", "Guard ms sau initial load trước khi refetch conversation.", ValidateInt(0, 60000)),
            Scalar("realtime.chat.app_start_guard_ms", "3000", "Guard ms trong vài giây đầu app start để giảm invalidate dồn dập.", ValidateInt(0, 60000)),
            Scalar("operational.http.client_timeout_ms", "8000", "Timeout mặc định client fetch (ms).", ValidateInt(500, 120000)),
            Scalar("operational.http.server_timeout_ms", "8000", "Timeout mặc định server proxy request (ms).", ValidateInt(500, 120000)),
            Scalar("operational.http.min_timeout_ms", "1000", "Timeout tối thiểu cho HTTP helpers (ms).", ValidateInt(100, 60000)),
            Scalar("operational.runtime_policies.timeout_ms", "8000", "Timeout fetch runtime policies (ms).", ValidateInt(500, 120000)),
            Scalar("operational.runtime_policies.stale_ms", "15000", "Stale time runtime policies cache (ms).", ValidateInt(1000, 600000)),
            Scalar("operational.redis.connect_timeout_ms", "2000", "Redis connect timeout (ms).", ValidateInt(100, 60000)),
            Scalar("operational.redis.sync_timeout_ms", "2000", "Redis sync timeout (ms).", ValidateInt(100, 60000)),
            Scalar("operational.redis.connect_retry", "1", "Số lần retry kết nối Redis.", ValidateInt(0, 20)),
            Scalar("operational.ai.timeout_seconds", "30", "Timeout request AI (giây).", ValidateInt(1, 600)),
            Scalar("operational.ai.max_retries", "2", "Số lần retry tối đa request AI.", ValidateInt(0, 20)),
            Scalar("operational.ai.streaming_retry_base_delay_ms", "200", "Base delay retry streaming AI (ms).", ValidateInt(10, 60000)),
            Scalar("operational.ai.streaming_temperature", "0.7", "Temperature mặc định cho streaming AI.", ValidateDecimal(0m, 2m)),
            Scalar("operational.outbox.batch_size", "50", "Số message outbox claim mỗi batch.", ValidateInt(1, 5000)),
            Scalar("operational.outbox.max_retry_attempts", "12", "Số lần retry outbox trước dead-letter.", ValidateInt(1, 100)),
            Scalar("operational.outbox.lock_timeout_seconds", "120", "Outbox processing lock timeout (giây).", ValidateInt(30, 3600)),
            Scalar("operational.outbox.max_backoff_seconds", "300", "Outbox retry backoff tối đa (giây).", ValidateInt(1, 3600)),
            Scalar("operational.outbox.poll_interval_seconds", "5", "Chu kỳ poll outbox worker (giây).", ValidateInt(1, 300))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildUiAndMediaScalarDefinitions()
    {
        return
        [
            Scalar("ui.readers.directory_page_size", "12", "Page size mặc định readers directory FE.", ValidateInt(1, 100)),
            Scalar("ui.readers.featured_limit", "4", "Số reader featured mặc định FE.", ValidateInt(1, 50)),
            Scalar("ui.search.debounce_ms", "300", "Debounce search input FE (ms).", ValidateInt(0, 5000)),
            Scalar("ui.readers.directory_stale_ms", "30000", "Stale time readers directory FE (ms).", ValidateInt(0, 600000)),
            Scalar("ui.prefetch.chat_inbox_stale_ms", "30000", "Stale time prefetch inbox FE (ms).", ValidateInt(0, 600000)),
            Scalar("media.upload.max_image_bytes", "10485760", "Giới hạn dung lượng ảnh upload (bytes).", ValidateLong(1024, 100_000_000)),
            Scalar("media.upload.max_voice_bytes", "5242880", "Giới hạn dung lượng voice upload (bytes).", ValidateLong(1024, 100_000_000)),
            Scalar("media.upload.max_voice_duration_ms", "600000", "Giới hạn thời lượng voice upload (ms).", ValidateInt(1000, 3_600_000)),
            Scalar("media.upload.image_compression_target_bytes", "81920", "Mục tiêu kích thước ảnh sau nén (bytes).", ValidateLong(1024, 50_000_000)),
            Scalar("media.upload.retry_attempts", "3", "Số lần retry upload mặc định.", ValidateInt(0, 10)),
            Scalar("media.upload.retry_delay_ms", "350", "Delay retry upload mặc định (ms).", ValidateInt(0, 10000))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildJsonDefinitions()
    {
        return
        [
            Json("chat.allowed_sla_hours", SerializeDefault(new[] { 6, 12, 24 }), "Danh sách SLA giờ được phép cho conversation.", ValidateIntArrayRange(1, 24, 1, 168)),
            Json("deposit.packages", SerializeDefault(BuildDefaultDepositPackages()), "Danh sách package nạp tiền hiển thị trên wallet.", ValidateJson),
            Json("followup.price_tiers", SerializeDefault(BuildDefaultFollowupPriceTiers()), "Bậc giá follow-up trả phí.", ValidateIntArray(1, 20)),
            Json("realtime.reconnect_schedule_ms", SerializeDefault(new[] { 0, 2000, 5000, 10000, 30000 }), "Lịch reconnect SignalR theo milliseconds.", ValidateIntArrayRange(1, 10, 0, 300000)),
            Json("media.upload.image_compression_steps", SerializeDefault(BuildDefaultImageCompressionSteps()), "Danh sách bước nén ảnh upload cho FE.", ValidateJson),
            Json("gacha.pools", SerializeDefault(BuildDefaultGachaPools()), "Định nghĩa pool/rate gacha để projection xuống PostgreSQL.", ValidateJson),
            Json("gamification.quests", SerializeDefault(BuildDefaultQuestDefinitions()), "Định nghĩa quest để projection xuống Mongo.", ValidateJson),
            Json("gamification.achievements", SerializeDefault(BuildDefaultAchievementDefinitions()), "Định nghĩa achievement để projection xuống Mongo.", ValidateJson),
            Json("gamification.titles", SerializeDefault(BuildDefaultTitleDefinitions()), "Định nghĩa title để projection xuống Mongo.", ValidateJson)
        ];
    }

    private static object[] BuildDefaultImageCompressionSteps()
    {
        return
        [
            new { initialQuality = 0.68, maxSizeMb = 0.15, maxWidthOrHeight = 1440 },
            new { initialQuality = 0.58, maxSizeMb = 0.10, maxWidthOrHeight = 1200 },
            new { initialQuality = 0.48, maxSizeMb = 0.06, maxWidthOrHeight = 960 },
            new { initialQuality = 0.38, maxSizeMb = 0.03, maxWidthOrHeight = 640 }
        ];
    }

    private static (bool IsValid, string? Error) ValidateGamificationDefaultQuestType(string rawValue)
    {
        var normalized = rawValue.Trim().ToLowerInvariant();
        return normalized is "daily" or "weekly"
            ? (true, null)
            : (false, "Value must be one of: daily, weekly.");
    }

    private static (bool IsValid, string? Error) ValidateGamificationDefaultLeaderboardTrack(string rawValue)
    {
        var normalized = rawValue.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return (false, "Value cannot be empty.");
        }

        if (normalized.Length > 64)
        {
            return (false, "Value length must be <= 64.");
        }

        return Regex.IsMatch(normalized, "^[a-z0-9_]+$")
            ? (true, null)
            : (false, "Value must match pattern ^[a-z0-9_]+$.");
    }

    private static string SerializeDefault(object value)
    {
        return JsonSerializer.Serialize(value, JsonOptions);
    }

    private static SystemConfigDefinition Scalar(
        string key,
        string defaultValue,
        string description,
        Func<string, (bool IsValid, string? Error)>? validator)
    {
        return new SystemConfigDefinition
        {
            Key = key,
            ValueKind = SystemConfigValueKind.Scalar,
            Description = description,
            DefaultValue = defaultValue,
            Validator = validator
        };
    }

    private static SystemConfigDefinition Json(
        string key,
        string defaultValue,
        string description,
        Func<string, (bool IsValid, string? Error)>? validator)
    {
        return new SystemConfigDefinition
        {
            Key = key,
            ValueKind = SystemConfigValueKind.Json,
            Description = description,
            DefaultValue = defaultValue,
            Validator = validator
        };
    }
}
