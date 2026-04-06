using System.Text.Json;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public partial class SubscribeCommandHandler
{
    private static UserSubscription CreateSubscription(Guid userId, SubscriptionPlan plan, string idempotencyKey, DateTime startDate)
    {
        var endDate = startDate.AddDays(plan.DurationDays);
        return new UserSubscription(
            userId: userId,
            planId: plan.Id,
            startDate: startDate,
            endDate: endDate,
            pricePaidDiamond: plan.PriceDiamond,
            idempotencyKey: idempotencyKey);
    }

    private async Task AddEntitlementBucketsAsync(
        Guid userId,
        UserSubscription subscription,
        SubscriptionPlan plan,
        DateTime currentUtc,
        CancellationToken ct)
    {
        var entitlements = JsonSerializer.Deserialize<List<EntitlementConfigDto>>(plan.EntitlementsJson);
        if (entitlements == null || entitlements.Count == 0) return;

        var todayUtc = DateOnly.FromDateTime(currentUtc);
        var endDate = currentUtc.AddDays(plan.DurationDays);
        var buckets = entitlements.Select(config => new SubscriptionEntitlementBucket(
            userSubscriptionId: subscription.Id,
            userId: userId,
            entitlementKey: config.key,
            dailyQuota: config.dailyQuota,
            currentDate: todayUtc,
            subscriptionEndDate: endDate)).ToList();

        await _subscriptionRepository.AddBucketsAsync(buckets, ct);
    }
}
