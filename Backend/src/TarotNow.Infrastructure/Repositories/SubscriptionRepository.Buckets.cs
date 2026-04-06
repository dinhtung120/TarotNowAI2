using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    public Task AddBucketsAsync(List<SubscriptionEntitlementBucket> buckets, CancellationToken ct)
    {
        _context.SubscriptionEntitlementBuckets.AddRange(buckets);
        return Task.CompletedTask;
    }

    public async Task<List<SubscriptionEntitlementBucket>> GetBucketsForConsumeAsync(
        Guid userId,
        string entitlementKey,
        DateOnly businessDate,
        CancellationToken ct)
    {
        const string sqlQuery = """
            SELECT * FROM subscription_entitlement_buckets
            WHERE user_id = {0}
              AND entitlement_key = {1}
              AND business_date = {2}
              AND used_today < daily_quota
              AND subscription_end_date > {3}
            FOR UPDATE
            """;

        var now = DateTime.UtcNow;
        var buckets = await _context.SubscriptionEntitlementBuckets
            .FromSqlRaw(sqlQuery, userId, entitlementKey, businessDate, now)
            .ToListAsync(ct);

        return buckets
            .OrderBy(b => b.SubscriptionEndDate)
            .ThenBy(b => b.UserSubscriptionId)
            .ToList();
    }

    public async Task<List<EntitlementBalanceDto>> GetBalanceSummaryAsync(Guid userId, DateOnly businessDate, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var buckets = await _context.SubscriptionEntitlementBuckets
            .AsNoTracking()
            .Where(b => b.UserId == userId && b.SubscriptionEndDate > now)
            .ToListAsync(ct);

        return buckets
            .GroupBy(b => b.EntitlementKey)
            .Select(g => new EntitlementBalanceDto(
                g.Key,
                g.Sum(b => b.DailyQuota),
                g.Sum(b => b.BusinessDate == businessDate ? b.UsedToday : 0),
                g.Sum(b => b.BusinessDate == businessDate ? (b.DailyQuota - b.UsedToday) : b.DailyQuota)))
            .ToList();
    }

    public Task<List<SubscriptionEntitlementBucket>> GetAllBucketsForResetAsync(DateOnly oldBusinessDate, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return _context.SubscriptionEntitlementBuckets
            .Where(b => b.BusinessDate < oldBusinessDate && b.SubscriptionEndDate > now)
            .OrderBy(b => b.Id)
            .Take(1000)
            .ToListAsync(ct);
    }
}
