namespace TarotNow.Domain.Events;

public sealed class EscrowReleasedDomainEvent : IDomainEvent
{
    public Guid ItemId { get; init; }

    public Guid PayerId { get; init; }

    public Guid ReceiverId { get; init; }

    public long GrossAmountDiamond { get; init; }

    public long ReleasedAmountDiamond { get; init; }

    public long FeeAmountDiamond { get; init; }

    public bool IsAutoRelease { get; init; }

    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
