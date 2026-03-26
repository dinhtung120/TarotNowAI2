namespace TarotNow.Domain.Events;

public sealed class EscrowRefundedDomainEvent : IDomainEvent
{
    public Guid ItemId { get; init; }

    public Guid UserId { get; init; }

    public long AmountDiamond { get; init; }

    public string RefundSource { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
