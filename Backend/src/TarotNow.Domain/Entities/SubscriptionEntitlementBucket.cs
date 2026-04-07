

using System;

namespace TarotNow.Domain.Entities;

public class SubscriptionEntitlementBucket
{
    public Guid Id { get; private set; }
    
        public Guid UserSubscriptionId { get; private set; }

        public Guid UserId { get; private set; }

        public string EntitlementKey { get; private set; } = string.Empty;

        public int DailyQuota { get; private set; }

        public int UsedToday { get; private set; }

        public DateOnly BusinessDate { get; private set; }

        public DateTime SubscriptionEndDate { get; private set; }

    
    public UserSubscription UserSubscription { get; private set; } = null!;

    protected SubscriptionEntitlementBucket() { }

    public SubscriptionEntitlementBucket(
        Guid userSubscriptionId, 
        Guid userId, 
        string entitlementKey, 
        int dailyQuota, 
        DateOnly currentDate, 
        DateTime subscriptionEndDate)
    {
        Id = Guid.NewGuid();
        UserSubscriptionId = userSubscriptionId;
        UserId = userId;
        EntitlementKey = entitlementKey;
        DailyQuota = dailyQuota;
        UsedToday = 0;
        BusinessDate = currentDate;
        SubscriptionEndDate = subscriptionEndDate;
    }

        public bool CanConsume(DateOnly todayUtc)
    {
        return BusinessDate == todayUtc && UsedToday < DailyQuota;
    }

        public void Consume(DateOnly todayUtc)
    {
        if (!CanConsume(todayUtc))
            throw new InvalidOperationException($"Không thể consume entitlement {EntitlementKey}. Quota: {UsedToday}/{DailyQuota}. Date: {BusinessDate} vs {todayUtc}");

        UsedToday++;
    }

        public void ResetForNewDay(DateOnly newDate)
    {
        if (newDate <= BusinessDate)
        {
            
            return;
        }

        UsedToday = 0;
        BusinessDate = newDate;
    }
}
