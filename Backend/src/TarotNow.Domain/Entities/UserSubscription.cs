

using System;
using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

public class UserSubscription
{
    public Guid Id { get; private set; }
    
        public Guid UserId { get; private set; }
    
        public Guid PlanId { get; private set; }

        public string Status { get; private set; } = string.Empty;

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

        public long PricePaidDiamond { get; private set; }

        public string IdempotencyKey { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    
    public SubscriptionPlan Plan { get; private set; } = null!;
    public User User { get; private set; } = null!;

    protected UserSubscription() { } 

    public UserSubscription(
        Guid userId, 
        Guid planId, 
        DateTime startDate, 
        DateTime endDate, 
        long pricePaidDiamond, 
        string idempotencyKey)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PlanId = planId;
        StartDate = startDate;
        EndDate = endDate;
        PricePaidDiamond = pricePaidDiamond;
        IdempotencyKey = idempotencyKey;
        Status = SubscriptionStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

        public bool IsActive => Status == SubscriptionStatus.Active && EndDate > DateTime.UtcNow;

        public void Expire()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException($"Không thể hết hạn một gói đang trong trạng thái: {Status}");

        Status = SubscriptionStatus.Expired;
    }

        public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
    }
}
