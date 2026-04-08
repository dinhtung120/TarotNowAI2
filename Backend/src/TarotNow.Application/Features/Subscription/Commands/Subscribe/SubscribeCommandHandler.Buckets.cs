using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Subscription.Commands.Subscribe;

public partial class SubscribeCommandHandler
{
    /// <summary>
    /// Tạo entity UserSubscription từ user + plan + thời điểm mua.
    /// Luồng xử lý: tính endDate theo DurationDays và dựng bản ghi subscription với idempotency key tương ứng.
    /// </summary>
    private static UserSubscription CreateSubscription(
        Guid userId,
        SubscriptionPlan plan,
        string idempotencyKey,
        DateTime startDate)
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

    /// <summary>
    /// Tạo entitlement bucket theo cấu hình gói.
    /// Luồng xử lý: parse EntitlementsJson, build bucket cho từng entitlement và lưu batch vào repository.
    /// </summary>
    private async Task AddEntitlementBucketsAsync(
        Guid userId,
        UserSubscription subscription,
        SubscriptionPlan plan,
        DateTime currentUtc,
        CancellationToken ct)
    {
        var entitlements = JsonSerializer.Deserialize<List<EntitlementConfigDto>>(plan.EntitlementsJson);
        if (entitlements is null || entitlements.Count == 0)
        {
            // Edge case: gói không có quyền lợi thì bỏ qua tạo bucket.
            return;
        }

        var todayUtc = DateOnly.FromDateTime(currentUtc);
        var endDate = currentUtc.AddDays(plan.DurationDays);
        var buckets = entitlements.Select(config => new SubscriptionEntitlementBucket(
            userSubscriptionId: subscription.Id,
            userId: userId,
            entitlementKey: config.key,
            dailyQuota: config.dailyQuota,
            currentDate: todayUtc,
            subscriptionEndDate: endDate)).ToList();
        // Map từng entitlement sang bucket để entitlement service có dữ liệu quota theo ngày.

        await _subscriptionRepository.AddBucketsAsync(buckets, ct);
        // Persist batch bucket trong cùng transaction mua gói.
    }
}
