using TarotNow.Domain.ValueObjects;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Cấu hình tỉ lệ reward cho một pool gacha.
/// </summary>
public sealed class GachaPoolRewardRate
{
    /// <summary>
    /// Định danh reward rate.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Định danh pool.
    /// </summary>
    public Guid PoolId { get; private set; }

    /// <summary>
    /// Navigation tới pool.
    /// </summary>
    public GachaPool? Pool { get; private set; }

    /// <summary>
    /// Loại reward (currency/item).
    /// </summary>
    public string RewardKind { get; private set; } = string.Empty;

    /// <summary>
    /// Độ hiếm reward.
    /// </summary>
    public string Rarity { get; private set; } = string.Empty;

    /// <summary>
    /// Xác suất theo basis points.
    /// </summary>
    public int ProbabilityBasisPoints { get; private set; }

    /// <summary>
    /// Item definition id nếu reward là item.
    /// </summary>
    public Guid? ItemDefinitionId { get; private set; }

    /// <summary>
    /// Currency nếu reward là tiền.
    /// </summary>
    public string? Currency { get; private set; }

    /// <summary>
    /// Amount nếu reward là tiền.
    /// </summary>
    public long? Amount { get; private set; }

    /// <summary>
    /// Số lượng item cấp ra.
    /// </summary>
    public int QuantityGranted { get; private set; }

    /// <summary>
    /// Icon reward.
    /// </summary>
    public string? IconUrl { get; private set; }

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
    /// Trạng thái active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private GachaPoolRewardRate()
    {
    }

    /// <summary>
    /// Khởi tạo reward rate mới.
    /// </summary>
    public GachaPoolRewardRate(
        Guid poolId,
        string rewardKind,
        string rarity,
        int probabilityBasisPoints,
        Guid? itemDefinitionId,
        string? currency,
        long? amount,
        int quantityGranted,
        string? iconUrl,
        string nameVi,
        string nameEn,
        string nameZh,
        bool isActive)
    {
        var reward = new GachaReward(
            rewardKind,
            rarity,
            probabilityBasisPoints,
            itemDefinitionId,
            currency,
            amount,
            quantityGranted);

        if (poolId == Guid.Empty)
        {
            throw new ArgumentException("PoolId is required.", nameof(poolId));
        }

        Id = Guid.NewGuid();
        PoolId = poolId;
        RewardKind = reward.RewardKind;
        Rarity = reward.Rarity;
        ProbabilityBasisPoints = reward.ProbabilityBasisPoints;
        ItemDefinitionId = reward.ItemDefinitionId;
        Currency = reward.Currency;
        Amount = reward.Amount;
        QuantityGranted = reward.QuantityGranted;
        IconUrl = string.IsNullOrWhiteSpace(iconUrl) ? null : iconUrl.Trim();
        NameVi = NormalizeRequired(nameVi, nameof(nameVi), 256);
        NameEn = NormalizeRequired(nameEn, nameof(nameEn), 256);
        NameZh = NormalizeRequired(nameZh, nameof(nameZh), 256);
        IsActive = isActive;
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

    /// <summary>
    /// Bật/tắt reward rate theo trạng thái projection hiện hành.
    /// </summary>
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
