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
}
