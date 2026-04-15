namespace TarotNow.Domain.Events.Gacha;

/// <summary>
/// Domain event phát sinh khi gacha cấp item vào inventory.
/// </summary>
public sealed class ItemGrantedFromGachaDomainEvent : IDomainEvent
{
    /// <summary>
    /// User nhận item.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Item definition id được cấp.
    /// </summary>
    public Guid ItemDefinitionId { get; init; }

    /// <summary>
    /// Item code được cấp.
    /// </summary>
    public string ItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Số lượng item được cấp.
    /// </summary>
    public int QuantityGranted { get; init; }

    /// <summary>
    /// Mã pool nguồn.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Tham chiếu pull operation.
    /// </summary>
    public Guid PullOperationId { get; init; }

    /// <summary>
    /// Mốc phát sinh sự kiện UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
