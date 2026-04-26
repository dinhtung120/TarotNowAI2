using Microsoft.EntityFrameworkCore;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed class ReadingRevealSagaStateRepository : IReadingRevealSagaStateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReadingRevealSagaStateRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ReadingRevealSagaState?> GetBySessionIdAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<ReadingRevealSagaState>()
            .SingleOrDefaultAsync(
                x => x.SessionId == sessionId,
                cancellationToken);
    }

    public async Task<ReadingRevealSagaState> GetOrCreateAsync(
        string sessionId,
        Guid userId,
        string language,
        CancellationToken cancellationToken = default)
    {
        var existing = await GetBySessionIdAsync(sessionId, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var created = ReadingRevealSagaState.Create(sessionId, userId, language, DateTime.UtcNow);
        _dbContext.Set<ReadingRevealSagaState>().Add(created);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return created;
        }
        catch (DbUpdateException)
        {
            var duplicate = await GetBySessionIdAsync(sessionId, cancellationToken);
            if (duplicate is not null)
            {
                return duplicate;
            }

            throw;
        }
    }

    public async Task UpdateAsync(ReadingRevealSagaState state, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<ReadingRevealSagaState>().Update(state);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ReadingRevealSagaState>> ListDueRepairAsync(
        DateTime nowUtc,
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedBatch = Math.Clamp(batchSize, 1, 200);
        return await _dbContext.Set<ReadingRevealSagaState>()
            .Where(x => (x.Status == ReadingRevealSagaStatus.Failed || x.Status == ReadingRevealSagaStatus.Processing)
                        && x.NextRepairAtUtc.HasValue
                        && x.NextRepairAtUtc <= nowUtc)
            .OrderBy(x => x.NextRepairAtUtc)
            .Take(normalizedBatch)
            .ToListAsync(cancellationToken);
    }
}
