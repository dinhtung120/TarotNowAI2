namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi người dùng mở mystery card pack.
/// </summary>
public sealed class MysteryPackOpenedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng mở gói.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã item nguồn kích hoạt mở gói.
    /// </summary>
    public string SourceItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh sự kiện theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
