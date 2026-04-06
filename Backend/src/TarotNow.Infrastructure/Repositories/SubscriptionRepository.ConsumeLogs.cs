using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Repositories;

public partial class SubscriptionRepository
{
    public Task<List<EntitlementMappingRule>> GetEnabledMappingRulesAsync(string sourceKey, CancellationToken ct)
    {
        return _context.EntitlementMappingRules
            .Where(m => m.SourceKey == sourceKey && m.IsEnabled)
            .ToListAsync(ct);
    }

    public Task AddConsumeLogAsync(EntitlementConsume consume, CancellationToken ct)
    {
        _context.EntitlementConsumes.Add(consume);
        return Task.CompletedTask;
    }

    public Task<bool> ConsumeLogExistsAsync(string idempotencyKey, CancellationToken ct)
    {
        return _context.EntitlementConsumes.AnyAsync(c => c.IdempotencyKey == idempotencyKey, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
