namespace TarotNow.Domain.Entities;

/// <summary>
/// Cấu hình một pool quay gacha.
/// </summary>
public sealed class GachaPool
{
    /// <summary>
    /// Định danh pool.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Mã pool duy nhất.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Loại pool (normal/premium/special).
    /// </summary>
    public string PoolType { get; private set; } = string.Empty;

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
    /// Loại tiền tệ dùng để pull.
    /// </summary>
    public string CostCurrency { get; private set; } = string.Empty;

    /// <summary>
    /// Số tiền phải trả cho mỗi lượt pull.
    /// </summary>
    public long CostAmount { get; private set; }

    /// <summary>
    /// Phiên bản odds phục vụ audit.
    /// </summary>
    public string OddsVersion { get; private set; } = string.Empty;

    /// <summary>
    /// Bật/tắt cơ chế pity.
    /// </summary>
    public bool PityEnabled { get; private set; }

    /// <summary>
    /// Ngưỡng hard pity.
    /// </summary>
    public int HardPityCount { get; private set; }

    /// <summary>
    /// Thời điểm bắt đầu hiệu lực.
    /// </summary>
    public DateTime EffectiveFrom { get; private set; }

    /// <summary>
    /// Thời điểm kết thúc hiệu lực.
    /// </summary>
    public DateTime? EffectiveTo { get; private set; }

    /// <summary>
    /// Trạng thái active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Mốc tạo theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Mốc cập nhật theo UTC.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private GachaPool()
    {
    }

    /// <summary>
    /// Khởi tạo pool mới.
    /// </summary>
    public GachaPool(
        string code,
        string poolType,
        string nameVi,
        string nameEn,
        string nameZh,
        string descriptionVi,
        string descriptionEn,
        string descriptionZh,
        string costCurrency,
        long costAmount,
        string oddsVersion,
        bool pityEnabled,
        int hardPityCount,
        DateTime effectiveFrom,
        DateTime? effectiveTo,
        bool isActive)
    {
        Id = Guid.NewGuid();
        Code = NormalizeRequired(code, nameof(code), 64).ToLowerInvariant();
        PoolType = NormalizeRequired(poolType, nameof(poolType), 32).ToLowerInvariant();
        NameVi = NormalizeRequired(nameVi, nameof(nameVi), 256);
        NameEn = NormalizeRequired(nameEn, nameof(nameEn), 256);
        NameZh = NormalizeRequired(nameZh, nameof(nameZh), 256);
        DescriptionVi = NormalizeRequired(descriptionVi, nameof(descriptionVi), 1024);
        DescriptionEn = NormalizeRequired(descriptionEn, nameof(descriptionEn), 1024);
        DescriptionZh = NormalizeRequired(descriptionZh, nameof(descriptionZh), 1024);
        CostCurrency = NormalizeRequired(costCurrency, nameof(costCurrency), 32).ToLowerInvariant();
        CostAmount = EnsurePositive(costAmount, nameof(costAmount));
        OddsVersion = NormalizeRequired(oddsVersion, nameof(oddsVersion), 32);
        PityEnabled = pityEnabled;
        HardPityCount = pityEnabled ? EnsurePositiveInt(hardPityCount, nameof(hardPityCount)) : 0;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// Kiểm tra pool có active tại thời điểm hiện tại hay không.
    /// </summary>
    public bool IsCurrentlyActive(DateTime utcNow)
    {
        if (!IsActive)
        {
            return false;
        }

        if (utcNow < EffectiveFrom)
        {
            return false;
        }

        return !EffectiveTo.HasValue || utcNow <= EffectiveTo.Value;
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

    private static long EnsurePositive(long value, string paramName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
        }

        return value;
    }

    private static int EnsurePositiveInt(int value, string paramName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
        }

        return value;
    }
}
