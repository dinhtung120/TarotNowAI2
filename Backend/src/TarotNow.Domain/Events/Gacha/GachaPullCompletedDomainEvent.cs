namespace TarotNow.Domain.Events.Gacha;

/// <summary>
/// Domain event follow-up sau khi pull gacha hoàn tất thành công.
/// </summary>
public sealed class GachaPullCompletedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh user.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã pool đã pull.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Số lượt pull.
    /// </summary>
    public int PullCount { get; init; }

    /// <summary>
    /// Cờ trigger pity.
    /// </summary>
    public bool WasPityTriggered { get; init; }

    /// <summary>
    /// Mốc phát sinh UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
