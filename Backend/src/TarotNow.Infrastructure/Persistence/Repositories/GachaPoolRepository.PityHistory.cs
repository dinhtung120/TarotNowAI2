using Microsoft.EntityFrameworkCore;
using Npgsql;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class GachaPoolRepository
{
    private const string UserPityUniqueConstraint = "ix_user_gacha_pities_user_id_pool_id";

    /// <inheritdoc />
    public async Task<UserGachaPity> GetOrCreateUserPityAsync(
        Guid userId,
        Guid poolId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Set<UserGachaPity>()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.PoolId == poolId, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        var created = new UserGachaPity(userId, poolId);
        _dbContext.Set<UserGachaPity>().Add(created);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return created;
        }
        catch (DbUpdateException exception) when (IsUserPityUniqueViolation(exception))
        {
            _dbContext.Entry(created).State = EntityState.Detached;
            return await _dbContext.Set<UserGachaPity>()
                .FirstAsync(x => x.UserId == userId && x.PoolId == poolId, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task SaveUserPityAsync(UserGachaPity userGachaPity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userGachaPity);
        _dbContext.Set<UserGachaPity>().Update(userGachaPity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddHistoryEntryAsync(GachaHistoryEntry historyEntry, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(historyEntry);
        _dbContext.Set<GachaHistoryEntry>().Add(historyEntry);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<GachaPullRewardLog>> GetRewardLogsByOperationIdsAsync(
        IReadOnlyCollection<Guid> pullOperationIds,
        CancellationToken cancellationToken = default)
    {
        if (pullOperationIds.Count == 0)
        {
            return Array.Empty<GachaPullRewardLog>();
        }

        return await _dbContext.Set<GachaPullRewardLog>()
            .AsNoTracking()
            .Where(x => pullOperationIds.Contains(x.PullOperationId))
            .OrderByDescending(x => x.CreatedAtUtc)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<GachaHistoryPageReadModel> GetUserPullHistoryAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPage = Math.Max(1, page);
        var normalizedPageSize = Math.Clamp(pageSize, 1, 100);
        var skip = (normalizedPage - 1) * normalizedPageSize;

        var query = _dbContext.Set<GachaHistoryEntry>()
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip(skip)
            .Take(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return new GachaHistoryPageReadModel
        {
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            TotalCount = totalCount,
            Items = items,
        };
    }

    private static bool IsUserPityUniqueViolation(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
               && string.Equals(postgresException.ConstraintName, UserPityUniqueConstraint, StringComparison.OrdinalIgnoreCase);
    }
}
