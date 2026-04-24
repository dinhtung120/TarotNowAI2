using System.Text.Json;

namespace TarotNow.Application.Common.SystemConfigs;

/// <summary>
/// Kiểu giá trị của cấu hình hệ thống.
/// </summary>
public enum SystemConfigValueKind
{
    Scalar,
    Json
}

/// <summary>
/// Định nghĩa một key cấu hình hệ thống.
/// </summary>
public sealed class SystemConfigDefinition
{
    public required string Key { get; init; }
    public required SystemConfigValueKind ValueKind { get; init; }
    public required string Description { get; init; }
    public required string DefaultValue { get; init; }
    public Func<string, (bool IsValid, string? Error)>? Validator { get; init; }
}

/// <summary>
/// Danh mục key cấu hình hợp lệ của hệ thống.
/// </summary>
public static partial class SystemConfigRegistry
{
    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly IReadOnlyDictionary<string, SystemConfigDefinition> Definitions =
        BuildDefinitions()
            .ToDictionary(
                x => x.Key,
                x => x,
                StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Trả toàn bộ định nghĩa key cấu hình.
    /// </summary>
    public static IReadOnlyList<SystemConfigDefinition> GetAll()
    {
        return Definitions.Values.OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    /// <summary>
    /// Thử lấy định nghĩa theo key.
    /// </summary>
    public static bool TryGetDefinition(string key, out SystemConfigDefinition definition)
    {
        var normalizedKey = NormalizeKey(key);
        return Definitions.TryGetValue(normalizedKey, out definition!);
    }

    /// <summary>
    /// Validate giá trị theo định nghĩa key.
    /// </summary>
    public static (bool IsValid, string? Error) Validate(string key, string value, string valueKind)
    {
        if (!TryGetDefinition(key, out var definition))
        {
            return (false, $"Unknown system config key: {NormalizeKey(key)}.");
        }

        var normalizedKind = NormalizeValueKind(valueKind);
        if (!string.Equals(
                definition.ValueKind.ToString(),
                normalizedKind,
                StringComparison.OrdinalIgnoreCase))
        {
            return (false, $"Key '{definition.Key}' requires value kind '{definition.ValueKind.ToString().ToLowerInvariant()}'.");
        }

        return definition.Validator is null ? (true, null) : definition.Validator(value);
    }

    /// <summary>
    /// Chuẩn hóa key theo convention chung.
    /// </summary>
    public static string NormalizeKey(string key)
    {
        return key.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// Chuẩn hóa value kind theo convention chung.
    /// </summary>
    public static string NormalizeValueKind(string valueKind)
    {
        if (string.IsNullOrWhiteSpace(valueKind))
        {
            return "scalar";
        }

        var normalized = valueKind.Trim().ToLowerInvariant();
        return normalized switch
        {
            "scalar" => "scalar",
            "json" => "json",
            _ => normalized
        };
    }

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
            Scalar("pricing.spread_3.gold", "50", "Giá Gold cho trải bài 3 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_3.diamond", "5", "Giá Diamond cho trải bài 3 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_5.gold", "100", "Giá Gold cho trải bài 5 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_5.diamond", "10", "Giá Diamond cho trải bài 5 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_10.gold", "500", "Giá Gold cho trải bài 10 lá.", ValidateLong(0, 10_000_000)),
            Scalar("pricing.spread_10.diamond", "50", "Giá Diamond cho trải bài 10 lá.", ValidateLong(0, 10_000_000)),
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
            Scalar("economy.vnd_per_diamond", "100", "Tỷ giá quy đổi 1 Diamond sang VND.", ValidateLong(1, 1_000_000)),
            Scalar("followup.max_allowed", "5", "Số follow-up tối đa trong một phiên.", ValidateInt(1, 20)),
            Scalar("followup.free_slots.threshold_high", "16", "Ngưỡng level để có 3 lượt free.", ValidateInt(1, 200)),
            Scalar("followup.free_slots.threshold_mid", "11", "Ngưỡng level để có 2 lượt free.", ValidateInt(1, 200)),
            Scalar("followup.free_slots.threshold_low", "6", "Ngưỡng level để có 1 lượt free.", ValidateInt(1, 200)),
            Scalar("presence.timeout_minutes", "15", "Ngưỡng timeout online presence theo phút.", ValidateInt(1, 240)),
            Scalar("presence.scan_interval_seconds", "60", "Chu kỳ quét timeout presence theo giây.", ValidateInt(5, 600)),
            Scalar("escrow.dispute_window_hours", "48", "Cửa sổ mở dispute theo giờ.", ValidateInt(1, 720)),
            Scalar("escrow.reader_response_due_hours", "24", "Deadline reader trả lời sau khi nhận câu hỏi.", ValidateInt(1, 168)),
            Scalar("escrow.auto_refund_hours", "24", "Deadline tự động refund theo giờ.", ValidateInt(1, 168)),
            Scalar("deposit.link_expiry_minutes", "15", "Thời gian sống payment link nạp tiền.", ValidateInt(1, 120))
        ];
    }

    private static IEnumerable<SystemConfigDefinition> BuildJsonDefinitions()
    {
        return
        [
            Json("chat.allowed_sla_hours", SerializeDefault(new[] { 6, 12, 24 }), "Danh sách SLA giờ được phép cho conversation.", ValidateIntArrayRange(1, 24, 1, 168)),
            Json("deposit.packages", SerializeDefault(BuildDefaultDepositPackages()), "Danh sách package nạp tiền hiển thị trên wallet.", ValidateJson),
            Json("followup.price_tiers", SerializeDefault(BuildDefaultFollowupPriceTiers()), "Bậc giá follow-up trả phí.", ValidateIntArray(1, 20)),
            Json("gacha.pools", SerializeDefault(BuildDefaultGachaPools()), "Định nghĩa pool/rate gacha để projection xuống PostgreSQL.", ValidateJson),
            Json("gamification.quests", SerializeDefault(BuildDefaultQuestDefinitions()), "Định nghĩa quest để projection xuống Mongo.", ValidateJson),
            Json("gamification.achievements", SerializeDefault(BuildDefaultAchievementDefinitions()), "Định nghĩa achievement để projection xuống Mongo.", ValidateJson),
            Json("gamification.titles", SerializeDefault(BuildDefaultTitleDefinitions()), "Định nghĩa title để projection xuống Mongo.", ValidateJson)
        ];
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
