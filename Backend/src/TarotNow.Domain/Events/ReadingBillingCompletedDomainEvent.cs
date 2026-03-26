namespace TarotNow.Domain.Events;

public sealed class ReadingBillingCompletedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }

    public Guid AiRequestId { get; init; }

    public string ReadingSessionRef { get; init; } = string.Empty;

    public long ChargeDiamond { get; init; }

    public string FinalStatus { get; init; } = string.Empty;

    public bool WasRefunded { get; init; }

    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
