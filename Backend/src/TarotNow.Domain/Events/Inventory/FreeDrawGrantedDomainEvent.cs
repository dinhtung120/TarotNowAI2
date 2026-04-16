namespace TarotNow.Domain.Events.Inventory;

/// <summary>
/// Domain event phát sinh khi người dùng nhận lượt rút/trải bài miễn phí.
/// </summary>
public sealed class FreeDrawGrantedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Người dùng được cấp free draw.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Số lượt free draw được cấp.
    /// </summary>
    public int GrantedCount { get; init; }

    /// <summary>
    /// Nhóm spread card count được cấp quota (3/5/10).
    /// </summary>
    public int SpreadCardCount { get; init; }

    /// <summary>
    /// Mã item nguồn tạo quyền lợi.
    /// </summary>
    public string SourceItemCode { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh sự kiện theo UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
