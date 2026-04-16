namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi người dùng sử dụng item trong kho đồ.
/// </summary>
public sealed class ItemUsedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng sử dụng item.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã item đang được sử dụng.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Lá bài mục tiêu nếu item yêu cầu.
    /// </summary>
    public int? TargetCardId { get; init; }

    /// <summary>
    /// Khóa idempotency của request dùng item.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh sự kiện theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Cờ cho biết đây là replay theo idempotency key đã xử lý trước đó.
    /// </summary>
    public bool IsIdempotentReplay { get; set; }

    /// <summary>
    /// Snapshot kết quả áp dụng item để trả về command response.
    /// </summary>
    public InventoryItemEffectSummary? EffectSummary { get; set; }
}

/// <summary>
/// Tóm tắt hiệu ứng áp dụng của item.
/// </summary>
public sealed class InventoryItemEffectSummary
{
    /// <summary>
    /// Loại hiệu ứng đã áp dụng.
    /// </summary>
    public string EffectType { get; init; } = string.Empty;

    /// <summary>
    /// Giá trị roll thực tế (% hoặc EXP hoặc số vé).
    /// </summary>
    public decimal RolledValue { get; init; }

    /// <summary>
    /// Card mục tiêu nếu item áp dụng lên card.
    /// </summary>
    public int? CardId { get; init; }

    /// <summary>
    /// Snapshot trước khi áp dụng.
    /// </summary>
    public InventoryCardStatSnapshot? Before { get; init; }

    /// <summary>
    /// Snapshot sau khi áp dụng.
    /// </summary>
    public InventoryCardStatSnapshot? After { get; init; }
}

/// <summary>
/// Snapshot chỉ số card hiển thị trước/sau khi dùng item.
/// </summary>
public sealed class InventoryCardStatSnapshot
{
    /// <summary>
    /// Level của card.
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// EXP hiện tại.
    /// </summary>
    public decimal CurrentExp { get; init; }

    /// <summary>
    /// EXP cần để lên level kế tiếp.
    /// </summary>
    public decimal ExpToNextLevel { get; init; }

    /// <summary>
    /// Base ATK.
    /// </summary>
    public decimal BaseAtk { get; init; }

    /// <summary>
    /// Base DEF.
    /// </summary>
    public decimal BaseDef { get; init; }

    /// <summary>
    /// Bonus % ATK.
    /// </summary>
    public decimal BonusAtkPercent { get; init; }

    /// <summary>
    /// Bonus % DEF.
    /// </summary>
    public decimal BonusDefPercent { get; init; }

    /// <summary>
    /// Tổng ATK.
    /// </summary>
    public decimal TotalAtk { get; init; }

    /// <summary>
    /// Tổng DEF.
    /// </summary>
    public decimal TotalDef { get; init; }
}
