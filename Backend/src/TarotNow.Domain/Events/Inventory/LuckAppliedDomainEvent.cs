namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi item may mắn được áp dụng.
/// </summary>
public sealed class LuckAppliedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng nhận hiệu ứng may mắn.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Giá trị may mắn đã áp dụng.
    /// </summary>
    public int LuckValue { get; init; }

    /// <summary>
    /// Mã item nguồn kích hoạt hiệu ứng.
    /// </summary>
    public string SourceItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh sự kiện theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
