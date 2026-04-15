namespace TarotNow.Domain.ValueObjects;

/// <summary>
/// Value object biểu diễn một reward trong pool gacha.
/// </summary>
public sealed class GachaReward
{
    /// <summary>
    /// Loại reward (currency/item).
    /// </summary>
    public string RewardKind { get; }

    /// <summary>
    /// Độ hiếm reward.
    /// </summary>
    public string Rarity { get; }

    /// <summary>
    /// Xác suất theo basis points.
    /// </summary>
    public int ProbabilityBasisPoints { get; }

    /// <summary>
    /// Item definition id nếu reward là item.
    /// </summary>
    public Guid? ItemDefinitionId { get; }

    /// <summary>
    /// Currency nếu reward là tiền.
    /// </summary>
    public string? Currency { get; }

    /// <summary>
    /// Amount nếu reward là tiền.
    /// </summary>
    public long? Amount { get; }

    /// <summary>
    /// Số lượng item cấp ra.
    /// </summary>
    public int QuantityGranted { get; }

    /// <summary>
    /// Khởi tạo value object reward.
    /// </summary>
    public GachaReward(
        string rewardKind,
        string rarity,
        int probabilityBasisPoints,
        Guid? itemDefinitionId,
        string? currency,
        long? amount,
        int quantityGranted)
    {
        if (string.IsNullOrWhiteSpace(rewardKind))
        {
            throw new ArgumentException("Reward kind is required.", nameof(rewardKind));
        }

        if (string.IsNullOrWhiteSpace(rarity))
        {
            throw new ArgumentException("Rarity is required.", nameof(rarity));
        }

        if (probabilityBasisPoints <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(probabilityBasisPoints), "Probability must be greater than zero.");
        }

        if (quantityGranted <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantityGranted), "QuantityGranted must be greater than zero.");
        }

        RewardKind = rewardKind.Trim().ToLowerInvariant();
        Rarity = rarity.Trim();
        ProbabilityBasisPoints = probabilityBasisPoints;
        ItemDefinitionId = itemDefinitionId;
        Currency = string.IsNullOrWhiteSpace(currency) ? null : currency.Trim().ToLowerInvariant();
        Amount = amount;
        QuantityGranted = quantityGranted;

        ValidateByKind();
    }

    private void ValidateByKind()
    {
        if (string.Equals(RewardKind, "item", StringComparison.Ordinal))
        {
            if (ItemDefinitionId is null || ItemDefinitionId == Guid.Empty)
            {
                throw new ArgumentException("ItemDefinitionId is required when reward kind is item.");
            }

            return;
        }

        if (string.Equals(RewardKind, "currency", StringComparison.Ordinal))
        {
            if (string.IsNullOrWhiteSpace(Currency))
            {
                throw new ArgumentException("Currency is required when reward kind is currency.");
            }

            if (Amount is null || Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero when reward kind is currency.");
            }

            return;
        }

        throw new ArgumentException("Unsupported reward kind.", nameof(RewardKind));
    }
}
