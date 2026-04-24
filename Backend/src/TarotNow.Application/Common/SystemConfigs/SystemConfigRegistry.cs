using System.Text.Encodings.Web;
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
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
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

}
