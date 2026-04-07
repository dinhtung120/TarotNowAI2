

using System;

namespace TarotNow.Domain.Events;

public sealed class EntitlementConsumedDomainEvent : IDomainEvent
{
    public Guid UserId { get; init; }
    public string EntitlementKey { get; init; }
    public Guid BucketId { get; init; }
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;

    public EntitlementConsumedDomainEvent(Guid userId, string entitlementKey, Guid bucketId)
    {
        UserId = userId;
        EntitlementKey = entitlementKey;
        BucketId = bucketId;
    }
}
