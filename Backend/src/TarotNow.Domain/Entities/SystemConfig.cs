namespace TarotNow.Domain.Entities;

/// <summary>
/// Cấu hình hệ thống runtime theo mô hình key-value.
/// </summary>
public sealed class SystemConfig
{
    /// <summary>
    /// Khóa cấu hình dạng namespace (vd: withdrawal.fee_rate).
    /// </summary>
    public string Key { get; private set; } = string.Empty;

    /// <summary>
    /// Giá trị thô của cấu hình (scalar hoặc JSON text).
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Loại giá trị: scalar hoặc json.
    /// </summary>
    public string ValueKind { get; private set; } = "scalar";

    /// <summary>
    /// Mô tả ngắn mục đích của cấu hình.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Định danh admin cập nhật gần nhất.
    /// </summary>
    public Guid? UpdatedBy { get; private set; }

    /// <summary>
    /// Mốc cập nhật gần nhất (UTC).
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Constructor rỗng cho EF materialization.
    /// </summary>
    private SystemConfig()
    {
    }

    /// <summary>
    /// Tạo cấu hình mới.
    /// </summary>
    public SystemConfig(
        string key,
        string value,
        string valueKind,
        string? description,
        Guid? updatedBy)
    {
        Key = NormalizeKey(key);
        Value = NormalizeValue(value);
        ValueKind = NormalizeValueKind(valueKind);
        Description = NormalizeOptionalText(description);
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cập nhật nội dung cấu hình.
    /// </summary>
    public void Update(
        string value,
        string valueKind,
        string? description,
        Guid? updatedBy)
    {
        Value = NormalizeValue(value);
        ValueKind = NormalizeValueKind(valueKind);
        Description = NormalizeOptionalText(description);
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("System config key is required.", nameof(key));
        }

        var normalized = key.Trim().ToLowerInvariant();
        if (normalized.Length > 200)
        {
            throw new ArgumentException("System config key must be <= 200 chars.", nameof(key));
        }

        return normalized;
    }

    private static string NormalizeValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("System config value is required.", nameof(value));
        }

        return value.Trim();
    }

    private static string NormalizeValueKind(string valueKind)
    {
        var normalized = string.IsNullOrWhiteSpace(valueKind)
            ? "scalar"
            : valueKind.Trim().ToLowerInvariant();
        return normalized switch
        {
            "scalar" => "scalar",
            "json" => "json",
            _ => throw new ArgumentException("Value kind must be 'scalar' or 'json'.", nameof(valueKind))
        };
    }

    private static string? NormalizeOptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        return normalized.Length > 2000 ? normalized[..2000] : normalized;
    }
}
