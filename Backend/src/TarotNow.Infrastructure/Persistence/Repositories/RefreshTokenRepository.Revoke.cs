using Microsoft.EntityFrameworkCore;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
    /// <inheritdoc />
    public async Task RevokeFamilyAsync(Guid familyId, string reason, CancellationToken cancellationToken = default)
    {
        if (familyId == Guid.Empty)
        {
            return;
        }

        await RevokeWhereAsync(
            _dbContext.RefreshTokens.Where(rt => rt.FamilyId == familyId && rt.RevokedAt == null),
            reason,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task RevokeSessionAsync(Guid sessionId, string reason, CancellationToken cancellationToken = default)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        await RevokeWhereAsync(
            _dbContext.RefreshTokens.Where(rt => rt.SessionId == sessionId && rt.RevokedAt == null),
            reason,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await RevokeWhereAsync(
            _dbContext.RefreshTokens.Where(rt => rt.UserId == userId && rt.RevokedAt == null),
            RefreshRevocationReasons.ManualRevoke,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CleanupRevokedOrExpiredBeforeAsync(
        DateTime cutoffUtc,
        int batchSize,
        CancellationToken cancellationToken = default)
    {
        if (batchSize <= 0)
        {
            return 0;
        }

        var candidateIds = await _dbContext.RefreshTokens
            .Where(rt =>
                (rt.RevokedAt != null && rt.RevokedAt <= cutoffUtc)
                || (rt.RevokedAt == null && rt.ExpiresAt <= cutoffUtc))
            .OrderBy(rt => rt.RevokedAt ?? rt.ExpiresAt)
            .Select(rt => rt.Id)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
        if (candidateIds.Count == 0)
        {
            return 0;
        }

        return await _dbContext.RefreshTokens
            .Where(rt => candidateIds.Contains(rt.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    private async Task RevokeWhereAsync(
        IQueryable<RefreshToken> query,
        string reason,
        CancellationToken cancellationToken)
    {
        var tokens = await query.ToListAsync(cancellationToken);
        if (tokens.Count == 0)
        {
            return;
        }

        foreach (var token in tokens)
        {
            token.Revoke(reason);
        }

        _dbContext.RefreshTokens.UpdateRange(tokens);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
