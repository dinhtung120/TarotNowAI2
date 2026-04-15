namespace TarotNow.Domain.Entities;

/// <summary>
/// Bản ghi phần thưởng của từng line reward trong một pull operation.
/// </summary>
public sealed class GachaPullRewardLog
{
    /// <summary>
    /// Định danh bản ghi reward.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Operation cha.
    /// </summary>
    public Guid PullOperationId { get; private set; }

    /// <summary>
    /// Người dùng nhận reward.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Pool sinh reward.
    /// </summary>
    public Guid PoolId { get; private set; }

    /// <summary>
    /// Mã pool sinh reward.
    /// </summary>
    public string PoolCode { get; private set; } = string.Empty;

    /// <summary>
    /// Định danh reward rate đã trúng.
    /// </summary>
    public Guid RewardRateId { get; private set; }

    /// <summary>
    /// Loại reward (currency/item).
    /// </summary>
    public string RewardKind { get; private set; } = string.Empty;

    /// <summary>
    /// Độ hiếm reward.
    /// </summary>
    public string Rarity { get; private set; } = string.Empty;

    /// <summary>
    /// Mã item nếu reward là item.
    /// </summary>
    public string? ItemCode { get; private set; }

    /// <summary>
    /// Định danh item definition nếu reward là item.
    /// </summary>
    public Guid? ItemDefinitionId { get; private set; }

    /// <summary>
    /// Loại tiền tệ nếu reward là currency.
    /// </summary>
    public string? Currency { get; private set; }

    /// <summary>
    /// Số lượng tiền thưởng nếu reward là currency.
    /// </summary>
    public long? Amount { get; private set; }

    /// <summary>
    /// Số lượng item được cấp.
    /// </summary>
    public int QuantityGranted { get; private set; }

    /// <summary>
    /// URL icon hiển thị.
    /// </summary>
    public string? IconUrl { get; private set; }

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; private set; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; private set; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
    /// </summary>
    public string NameZh { get; private set; } = string.Empty;

    /// <summary>
    /// Cờ cho biết line reward này do hard pity.
    /// </summary>
    public bool IsHardPityReward { get; private set; }

    /// <summary>
    /// Pity count tại thời điểm roll.
    /// </summary>
    public int PityCountAtReward { get; private set; }

    /// <summary>
    /// Seed phục vụ audit RNG.
    /// </summary>
    public string? RngSeed { get; private set; }

    /// <summary>
    /// Mốc tạo theo UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Constructor cho ORM.
    /// </summary>
    private GachaPullRewardLog()
    {
    }

    /// <summary>
    /// Khởi tạo bản ghi reward từ request.
    /// </summary>
    public GachaPullRewardLog(GachaPullRewardLogCreateRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.PullOperationId == Guid.Empty)
        {
            throw new ArgumentException("PullOperationId is required.", nameof(request));
        }

        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(request));
        }

        if (request.PoolId == Guid.Empty)
        {
            throw new ArgumentException("PoolId is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.PoolCode))
        {
            throw new ArgumentException("PoolCode is required.", nameof(request));
        }

        if (request.RewardRateId == Guid.Empty)
        {
            throw new ArgumentException("RewardRateId is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.RewardKind))
        {
            throw new ArgumentException("RewardKind is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Rarity))
        {
            throw new ArgumentException("Rarity is required.", nameof(request));
        }

        if (request.QuantityGranted <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "QuantityGranted must be greater than zero.");
        }

        Id = Guid.NewGuid();
        PullOperationId = request.PullOperationId;
        UserId = request.UserId;
        PoolId = request.PoolId;
        PoolCode = request.PoolCode.Trim().ToLowerInvariant();
        RewardRateId = request.RewardRateId;
        RewardKind = request.RewardKind.Trim().ToLowerInvariant();
        Rarity = request.Rarity.Trim();
        ItemCode = request.ItemCode?.Trim().ToLowerInvariant();
        ItemDefinitionId = request.ItemDefinitionId;
        Currency = request.Currency?.Trim().ToLowerInvariant();
        Amount = request.Amount;
        QuantityGranted = request.QuantityGranted;
        IconUrl = request.IconUrl?.Trim();
        NameVi = request.NameVi.Trim();
        NameEn = request.NameEn.Trim();
        NameZh = request.NameZh.Trim();
        IsHardPityReward = request.IsHardPityReward;
        PityCountAtReward = request.PityCountAtReward;
        RngSeed = request.RngSeed?.Trim();
        CreatedAtUtc = DateTime.UtcNow;
    }
}

/// <summary>
/// Request tạo GachaPullRewardLog.
/// </summary>
public sealed record GachaPullRewardLogCreateRequest(
    Guid PullOperationId,
    Guid UserId,
    Guid PoolId,
    string PoolCode,
    Guid RewardRateId,
    string RewardKind,
    string Rarity,
    string? ItemCode,
    Guid? ItemDefinitionId,
    string? Currency,
    long? Amount,
    int QuantityGranted,
    string? IconUrl,
    string NameVi,
    string NameEn,
    string NameZh,
    bool IsHardPityReward,
    int PityCountAtReward,
    string? RngSeed);
