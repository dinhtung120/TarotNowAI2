namespace TarotNow.Domain.Entities;

/// <summary>
/// Định nghĩa master data của item trong hệ thống Tarot Vault.
/// </summary>
public class ItemDefinition
{
    /// <summary>
    /// Định danh item.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Mã item duy nhất dùng trong nghiệp vụ.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Loại item.
    /// </summary>
    public string Type { get; private set; } = string.Empty;

    /// <summary>
    /// Kiểu enhancement nếu item có tác dụng nâng cấp.
    /// </summary>
    public string? EnhancementType { get; private set; }

    /// <summary>
    /// Độ hiếm item.
    /// </summary>
    public string Rarity { get; private set; } = string.Empty;

    /// <summary>
    /// Cờ item tiêu hao theo số lượng.
    /// </summary>
    public bool IsConsumable { get; private set; }

    /// <summary>
    /// Cờ item sở hữu vĩnh viễn.
    /// </summary>
    public bool IsPermanent { get; private set; }

    /// <summary>
    /// Giá trị hiệu ứng cơ bản.
    /// </summary>
    public int EffectValue { get; private set; }

    /// <summary>
    /// Tỉ lệ thành công dạng phần trăm (0-100).
    /// </summary>
    public decimal SuccessRatePercent { get; private set; }

    /// <summary>
    /// Tên hiển thị tiếng Việt.
    /// </summary>
    public string NameVi { get; private set; } = string.Empty;

    /// <summary>
    /// Tên hiển thị tiếng Anh.
    /// </summary>
    public string NameEn { get; private set; } = string.Empty;

    /// <summary>
    /// Tên hiển thị tiếng Trung.
    /// </summary>
    public string NameZh { get; private set; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Việt.
    /// </summary>
    public string DescriptionVi { get; private set; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Anh.
    /// </summary>
    public string DescriptionEn { get; private set; } = string.Empty;

    /// <summary>
    /// Mô tả tiếng Trung.
    /// </summary>
    public string DescriptionZh { get; private set; } = string.Empty;

    /// <summary>
    /// URL icon item.
    /// </summary>
    public string? IconUrl { get; private set; }

    /// <summary>
    /// Trạng thái kích hoạt item.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Mốc tạo bản ghi theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Mốc cập nhật gần nhất theo UTC.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor rỗng cho ORM.
    /// </summary>
    protected ItemDefinition()
    {
    }

    /// <summary>
    /// Khởi tạo item definition mới với đầy đủ thông tin nghiệp vụ.
    /// </summary>
    public ItemDefinition(
        Guid id,
        string code,
        string type,
        string rarity,
        bool isConsumable,
        bool isPermanent,
        int effectValue,
        decimal successRatePercent,
        string nameVi,
        string nameEn,
        string nameZh,
        string descriptionVi,
        string descriptionEn,
        string descriptionZh,
        string? enhancementType = null,
        string? iconUrl = null,
        bool isActive = true)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Code = NormalizeRequired(code, nameof(code), 64);
        Type = NormalizeRequired(type, nameof(type), 64);
        Rarity = NormalizeRequired(rarity, nameof(rarity), 32);
        NameVi = NormalizeRequired(nameVi, nameof(nameVi), 200);
        NameEn = NormalizeRequired(nameEn, nameof(nameEn), 200);
        NameZh = NormalizeRequired(nameZh, nameof(nameZh), 200);
        DescriptionVi = NormalizeRequired(descriptionVi, nameof(descriptionVi), 1000);
        DescriptionEn = NormalizeRequired(descriptionEn, nameof(descriptionEn), 1000);
        DescriptionZh = NormalizeRequired(descriptionZh, nameof(descriptionZh), 1000);
        EnhancementType = NormalizeOptional(enhancementType, 64);
        IconUrl = NormalizeOptional(iconUrl, 500);
        IsConsumable = isConsumable;
        IsPermanent = isPermanent;
        EffectValue = effectValue;
        SuccessRatePercent = EnsureRange(successRatePercent, 0m, 100m, nameof(successRatePercent));
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Đánh dấu item không còn sử dụng.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu item hoạt động trở lại.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static string NormalizeRequired(string value, string paramName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", paramName);
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"Value length must be <= {maxLength}.", paramName);
        }

        return normalized;
    }

    private static string? NormalizeOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"Value length must be <= {maxLength}.", nameof(value));
        }

        return normalized;
    }

    private static decimal EnsureRange(decimal value, decimal min, decimal max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, $"Value must be in range [{min}, {max}].");
        }

        return value;
    }
}
