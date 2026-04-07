

using System;

namespace TarotNow.Domain.Events;

public sealed class SubscriptionActivatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public Guid SubscriptionId { get; init; }
    public Guid PlanId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public SubscriptionActivatedDomainEvent(Guid userId, Guid subscriptionId, Guid planId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        PlanId = planId;
    }
}
