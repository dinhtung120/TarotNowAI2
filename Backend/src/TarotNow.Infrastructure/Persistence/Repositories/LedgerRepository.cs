

using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class LedgerRepository : ILedgerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LedgerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

        public async Task<int> GetTotalCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .CountAsync(cancellationToken);
    }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid userId, int page, int limit, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedLimit = limit <= 0 ? 20 : Math.Min(limit, 200);

        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedLimit)
            .Take(normalizedLimit)
            .ToListAsync(cancellationToken);
    }
}
