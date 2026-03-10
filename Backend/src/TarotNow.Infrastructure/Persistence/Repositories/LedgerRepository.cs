using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Interfaces;

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
        return await _dbContext.WalletTransactions
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
