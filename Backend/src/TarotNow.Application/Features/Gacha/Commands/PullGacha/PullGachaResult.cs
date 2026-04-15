namespace TarotNow.Application.Features.Gacha.Commands.PullGacha;

/// <summary>
/// Kết quả pull gacha.
/// </summary>
public sealed class PullGachaResult
{
    /// <summary>
    /// Cờ thành công.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Cờ replay idempotency.
    /// </summary>
    public bool IsIdempotentReplay { get; init; }

    /// <summary>
    /// Mã pool đã pull.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Pity hiện tại sau pull.
    /// </summary>
    public int CurrentPityCount { get; init; }

    /// <summary>
    /// Ngưỡng hard pity của pool.
    /// </summary>
    public int HardPityThreshold { get; init; }

    /// <summary>
    /// Cờ trigger pity.
    /// </summary>
    public bool WasPityTriggered { get; init; }

    /// <summary>
    /// Danh sách rewards trả về.
    /// </summary>
    public IReadOnlyList<PullGachaRewardResult> Rewards { get; init; } = Array.Empty<PullGachaRewardResult>();
}

/// <summary>
/// Reward line trả về sau pull.
/// </summary>
public sealed class PullGachaRewardResult
{
    /// <summary>
    /// Loại reward (currency/item).
    /// </summary>
    public string Kind { get; init; } = string.Empty;

    /// <summary>
    /// Độ hiếm reward.
    /// </summary>
    public string Rarity { get; init; } = string.Empty;

    /// <summary>
    /// Currency nếu reward là tiền.
    /// </summary>
    public string? Currency { get; init; }

    /// <summary>
    /// Amount nếu reward là tiền.
    /// </summary>
    public long? Amount { get; init; }

    /// <summary>
    /// Item definition id nếu reward là item.
    /// </summary>
    public Guid? ItemDefinitionId { get; init; }

    /// <summary>
    /// Item code nếu reward là item.
    /// </summary>
    public string? ItemCode { get; init; }

    /// <summary>
    /// Số lượng item cấp ra.
    /// </summary>
    public int QuantityGranted { get; init; }

    /// <summary>
    /// Icon hiển thị.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Tên tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;
}
