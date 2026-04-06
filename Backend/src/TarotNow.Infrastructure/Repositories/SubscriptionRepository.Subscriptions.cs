using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    public Task<UserSubscription?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct)
    {
        return _context.UserSubscriptions.FirstOrDefaultAsync(s => s.IdempotencyKey == idempotencyKey, ct);
    }

    public Task<List<UserSubscription>> GetActiveSubscriptionsAsync(Guid userId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        return _context.UserSubscriptions
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active && s.EndDate > now)
            .ToListAsync(ct);
    }

    public Task<List<UserSubscription>> GetExpiredSubscriptionsToProcessAsync(DateTime cutoff, CancellationToken ct)
    {
        return _context.UserSubscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= cutoff)
            .OrderBy(s => s.EndDate)
            .Take(50)
            .ToListAsync(ct);
    }

    public Task AddSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        _context.UserSubscriptions.Add(subscription);
        return Task.CompletedTask;
    }

    public Task UpdateSubscriptionAsync(UserSubscription subscription, CancellationToken ct)
    {
        _context.UserSubscriptions.Update(subscription);
        return Task.CompletedTask;
    }
}
