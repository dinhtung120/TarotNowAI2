using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository truy cập dữ liệu pool/reward/operation của gacha mới.
/// </summary>
public sealed partial class GachaPoolRepository : IGachaPoolRepository
{
    private const string PullOperationUniqueConstraint = "ix_gacha_pull_operations_user_id_idempotency_key";

    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Khởi tạo <see cref="GachaPoolRepository"/>.
    /// </summary>
    public GachaPoolRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<GachaPool>> GetActivePoolsAsync(CancellationToken cancellationToken = default)
    {
        var nowUtc = DateTime.UtcNow;
        return await _dbContext.Set<GachaPool>()
            .AsNoTracking()
            .Where(x => x.IsActive && x.EffectiveFrom <= nowUtc && (x.EffectiveTo == null || x.EffectiveTo >= nowUtc))
            .OrderBy(x => x.CostAmount)
            .ThenBy(x => x.Code)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GachaPool?> GetActivePoolByCodeAsync(string poolCode, CancellationToken cancellationToken = default)
    {
        var normalizedPoolCode = poolCode.Trim().ToLowerInvariant();
        var nowUtc = DateTime.UtcNow;

        return await _dbContext.Set<GachaPool>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Code == normalizedPoolCode
                     && x.IsActive
                     && x.EffectiveFrom <= nowUtc
                     && (x.EffectiveTo == null || x.EffectiveTo >= nowUtc),
                cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<GachaPoolRewardRate>> GetActiveRewardRatesAsync(Guid poolId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<GachaPoolRewardRate>()
            .AsNoTracking()
            .Where(x => x.PoolId == poolId && x.IsActive)
            .OrderBy(x => x.Rarity)
            .ThenByDescending(x => x.ProbabilityBasisPoints)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> GetUserCurrentPityCountAsync(Guid userId, Guid poolId, CancellationToken cancellationToken = default)
    {
        var currentPity = await _dbContext.Set<UserGachaPity>()
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.PoolId == poolId)
            .Select(x => new { x.PullCount })
            .FirstOrDefaultAsync(cancellationToken);

        return currentPity?.PullCount ?? 0;
    }

    /// <inheritdoc />
    public async Task<GachaPullOperationCreateResult> TryCreatePullOperationAsync(
        GachaPullOperation operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        _dbContext.Set<GachaPullOperation>().Add(operation);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new GachaPullOperationCreateResult
            {
                IsCreated = true,
                Operation = operation,
            };
        }
        catch (DbUpdateException exception) when (IsPullOperationUniqueViolation(exception))
        {
            _dbContext.Entry(operation).State = EntityState.Detached;

            var existing = await _dbContext.Set<GachaPullOperation>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UserId == operation.UserId && x.IdempotencyKey == operation.IdempotencyKey,
                    cancellationToken);
            if (existing is null)
            {
                throw new InvalidOperationException("Cannot load existing gacha pull operation after idempotency conflict.", exception);
            }

            return new GachaPullOperationCreateResult
            {
                IsCreated = false,
                Operation = existing,
            };
        }
    }

    /// <inheritdoc />
    public async Task MarkPullOperationCompletedAsync(
        GachaPullOperation operation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        _dbContext.Set<GachaPullOperation>().Update(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddPullRewardLogsAsync(
        IReadOnlyCollection<GachaPullRewardLog> rewardLogs,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(rewardLogs);
        if (rewardLogs.Count == 0)
        {
            return;
        }

        await _dbContext.Set<GachaPullRewardLog>().AddRangeAsync(rewardLogs, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<GachaPullRewardLog>> GetRewardLogsByOperationIdAsync(
        Guid pullOperationId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<GachaPullRewardLog>()
            .AsNoTracking()
            .Where(x => x.PullOperationId == pullOperationId)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<GachaPullRewardLog>> GetUserRewardHistoryAsync(
        Guid userId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = Math.Clamp(limit, 1, 200);

        return await _dbContext.Set<GachaPullRewardLog>()
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(normalizedLimit)
            .ToListAsync(cancellationToken);
    }

    private static bool IsPullOperationUniqueViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(
                   postgresException.ConstraintName,
                   PullOperationUniqueConstraint,
                   StringComparison.OrdinalIgnoreCase);
    }
}
