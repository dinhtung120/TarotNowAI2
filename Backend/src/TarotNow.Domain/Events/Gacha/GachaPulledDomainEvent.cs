namespace TarotNow.Domain.Events.Gacha;

/// <summary>
/// Domain event phát sinh khi user thực hiện pull gacha.
/// </summary>
public sealed class GachaPulledDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh user pull.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã pool được pull.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Số lượt pull trong request.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Idempotency key của request.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Cờ cho biết đây là replay idempotent.
    /// </summary>
    public bool IsIdempotentReplay { get; set; }

    /// <summary>
    /// Cờ cho biết replay idempotent nhưng operation vẫn đang xử lý.
    /// </summary>
    public bool IsProcessingReplay { get; set; }

    /// <summary>
    /// Định danh operation idempotent tương ứng.
    /// </summary>
    public Guid OperationId { get; set; }

    /// <summary>
    /// Pity hiện tại sau pull.
    /// </summary>
    public int CurrentPityCount { get; set; }

    /// <summary>
    /// Ngưỡng hard pity.
    /// </summary>
    public int HardPityThreshold { get; set; }

    /// <summary>
    /// Cờ trigger pity trong request.
    /// </summary>
    public bool WasPityTriggered { get; set; }

    /// <summary>
    /// Snapshot rewards trả về command response.
    /// </summary>
    public IReadOnlyList<GachaPullRewardSnapshot> Rewards { get; set; } = Array.Empty<GachaPullRewardSnapshot>();

    /// <summary>
    /// Mốc phát sinh sự kiện UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Snapshot reward trả về cho client sau pull.
/// </summary>
public sealed class GachaPullRewardSnapshot
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
    /// Icon hiển thị reward.
    /// </summary>
    public string? IconUrl { get; init; }

    /// <summary>
    /// Tên reward tiếng Việt.
    /// </summary>
    public string NameVi { get; init; } = string.Empty;

    /// <summary>
    /// Tên reward tiếng Anh.
    /// </summary>
    public string NameEn { get; init; } = string.Empty;

    /// <summary>
    /// Tên reward tiếng Trung.
    /// </summary>
    public string NameZh { get; init; } = string.Empty;
}
