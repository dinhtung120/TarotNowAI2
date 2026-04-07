

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface ISubscriptionRepository
{
    
    
    
    Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct);
    Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct);
    Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct);
    Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct);

    
    
    
    Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct);
    Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct);
    
        Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct);
    Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct);
    Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct);

    
    
    
    
        Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct);

        Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId, 
        string entitlementKey, 
        DateOnly businessDate, 
        CancellationToken ct);

        Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(
        Guid userId, 
        DateOnly businessDate, 
        CancellationToken ct);

        Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(
        DateOnly oldBusinessDate, 
        CancellationToken ct);

    
    
    
    Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(
        string sourceKey, 
        CancellationToken ct);

    
    
    
    Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct);
    Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct);

    
    Task SaveChangesAsync(CancellationToken ct);
}
