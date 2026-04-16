namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi item tác động lên chỉ số lá bài.
/// </summary>
public sealed class CardEnhancedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng sở hữu lá bài.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Định danh lá bài được áp dụng.
    /// </summary>
    public int CardId { get; init; }

    /// <summary>
    /// Kiểu enhancement đã áp dụng.
    /// </summary>
    public string EnhancementType { get; init; } = string.Empty;

    /// <summary>
    /// Delta EXP áp dụng.
    /// </summary>
    public decimal ExpDelta { get; init; }

    /// <summary>
    /// Delta Attack áp dụng.
    /// </summary>
    public decimal AttackDelta { get; init; }

    /// <summary>
    /// Delta Defense áp dụng.
    /// </summary>
    public decimal DefenseDelta { get; init; }

    /// <summary>
    /// Cờ cho biết item level upgrader có thành công hay không.
    /// </summary>
    public bool UpgradeSucceeded { get; init; }

    /// <summary>
    /// Mã item nguồn đã tạo hiệu ứng.
    /// </summary>
    public string SourceItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh sự kiện theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
