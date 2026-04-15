namespace TarotNow.Domain.Events.Gacha;

/// <summary>
/// Domain event phát sinh khi pull gacha trigger pity force.
/// </summary>
public sealed class PityTriggeredDomainEvent : IDomainEvent
{
    /// <summary>
    /// Định danh user trigger pity.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã pool xảy ra pity trigger.
    /// </summary>
    public string PoolCode { get; init; } = string.Empty;

    /// <summary>
    /// Tham chiếu pull operation.
    /// </summary>
    public Guid PullOperationId { get; init; }

    /// <summary>
    /// Độ hiếm reward bị force khi trigger.
    /// </summary>
    public string RarityForced { get; init; } = string.Empty;

    /// <summary>
    /// Mốc phát sinh event UTC.
    /// </summary>
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
