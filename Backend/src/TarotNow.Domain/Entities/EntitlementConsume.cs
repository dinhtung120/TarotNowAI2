

using System;

namespace TarotNow.Domain.Entities;

public class EntitlementConsume
{
    public Guid Id { get; private set; }
    
        public Guid UserId { get; private set; }

        public Guid BucketId { get; private set; }

        public string EntitlementKey { get; private set; } = string.Empty;

    public DateTime ConsumedAt { get; private set; }

        public string? ReferenceSource { get; private set; }

        public string? ReferenceId { get; private set; }

        public string IdempotencyKey { get; private set; } = string.Empty;

    
    public SubscriptionEntitlementBucket Bucket { get; private set; } = null!;
    public User User { get; private set; } = null!;

    protected EntitlementConsume() { }

    public EntitlementConsume(
        Guid userId, 
        Guid bucketId, 
        string entitlementKey, 
        string? referenceSource, 
        string? referenceId, 
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        BucketId = bucketId;
        EntitlementKey = entitlementKey;
        ConsumedAt = DateTime.UtcNow;
        ReferenceSource = referenceSource;
        ReferenceId = referenceId;
        IdempotencyKey = idempotencyKey;
    }
}
