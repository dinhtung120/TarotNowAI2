

using System;

namespace TarotNow.Domain.Events;

public sealed class SubscriptionExpiredDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public Guid SubscriptionId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public SubscriptionExpiredDomainEvent(Guid userId, Guid subscriptionId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
    }
}
