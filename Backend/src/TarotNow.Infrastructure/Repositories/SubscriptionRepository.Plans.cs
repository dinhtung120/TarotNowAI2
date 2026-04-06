using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    public Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId, CancellationToken ct)
    {
        return _context.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == planId, ct);
    }

    public Task<List<SubscriptionPlan>> GetActivePlansAsync(CancellationToken ct)
    {
        return _context.SubscriptionPlans
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(ct);
    }

    public Task AddPlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        _context.SubscriptionPlans.Add(plan);
        return Task.CompletedTask;
    }

    public Task UpdatePlanAsync(SubscriptionPlan plan, CancellationToken ct)
    {
        _context.SubscriptionPlans.Update(plan);
        return Task.CompletedTask;
    }
}
