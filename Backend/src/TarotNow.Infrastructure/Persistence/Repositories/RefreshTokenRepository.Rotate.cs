using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
    /// <inheritdoc />
    public async Task<RefreshRotateResult> RotateAsync(
        RefreshRotateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (_authSecurityOptions.RequireRedisForRefreshConsistency && !_cacheBackendState.UsesRedis)
        {
            return RefreshRotateResult.Locked();
        }

        if (!IsValidRotateRequest(request))
        {
            return RefreshRotateResult.Invalid();
        }

        var context = BuildRotateExecutionContext(request);
        if (!await TryAcquireLockAsync(context.LockKey, context.LockOwner, cancellationToken))
        {
            return await HandleLockContentionAsync(
                request.RawToken,
                request.NewRawToken,
                context.TokenIdempotencyCacheKey,
                cancellationToken);
        }

        try
        {
            return await RotateWithTransactionAsync(context.TransactionContext, cancellationToken);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(context.LockKey, context.LockOwner, cancellationToken);
        }
    }

    private async Task<RefreshRotateResult> RotateWithTransactionAsync(
        RotateTransactionContext context,
        CancellationToken cancellationToken)
    {
        IDbContextTransaction? localTransaction = null;
        if (_dbContext.Database.CurrentTransaction is null)
        {
            localTransaction = await _dbContext.Database
                .BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        }

        try
        {
            var current = await LoadForUpdateAsync(context.TokenHash, cancellationToken);
            if (current is null)
            {
                return await FinalizeResultAsync(localTransaction, RefreshRotateResult.Invalid(), cancellationToken);
            }

            var idempotencyKey = context.NormalizedIdempotencyKey;
            var idemCacheKey = BuildRefreshIdempotencyKey(current.SessionId, idempotencyKey);
            var earlyExit = await TryBuildEarlyExitResultAsync(
                current,
                idemCacheKey,
                context,
                cancellationToken);

            if (earlyExit is not null)
            {
                return await FinalizeResultAsync(localTransaction, earlyExit, cancellationToken);
            }

            var persistContext = new RotatePersistContext(
                context.Request,
                current,
                idempotencyKey,
                idemCacheKey,
                context.TokenIdempotencyCacheKey);
            return await RotateAndPersistAsync(persistContext, localTransaction, cancellationToken);
        }
        finally
        {
            if (localTransaction is not null)
            {
                await localTransaction.DisposeAsync();
            }
        }
    }

    private RotateExecutionContext BuildRotateExecutionContext(RefreshRotateRequest request)
    {
        var normalizedToken = request.RawToken.Trim();
        var tokenHash = RefreshToken.HashToken(normalizedToken);
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(
            request.IdempotencyKey,
            tokenHash,
            request.DeviceId,
            request.UserAgentHash);
        var tokenIdempotencyCacheKey = BuildRefreshTokenIdempotencyKey(tokenHash, normalizedIdempotencyKey);
        return new RotateExecutionContext(
            LockKey: BuildRefreshLockKey(tokenHash),
            LockOwner: Guid.NewGuid().ToString("N"),
            TokenIdempotencyCacheKey: tokenIdempotencyCacheKey,
            TransactionContext: new RotateTransactionContext(
                request,
                tokenHash,
                normalizedIdempotencyKey,
                tokenIdempotencyCacheKey));
    }

    private async Task<RefreshRotateResult> HandleLockContentionAsync(
        string rawToken,
        string newRawToken,
        string tokenIdempotencyCacheKey,
        CancellationToken cancellationToken)
    {
        var tokenCached = await _cacheService.GetAsync<RefreshRotateCacheItem>(tokenIdempotencyCacheKey, cancellationToken);
        if (tokenCached is null
            || tokenCached.NewTokenId == Guid.Empty
            || tokenCached.NewTokenExpiresAtUtc <= DateTime.MinValue)
        {
            return RefreshRotateResult.Locked();
        }

        var replacementToken = await TryLoadReplacementTokenAsync(tokenCached.NewTokenId, cancellationToken);
        if (replacementToken is null || !replacementToken.MatchesToken(newRawToken))
        {
            return RefreshRotateResult.Locked();
        }

        var tokenSnapshot = await GetByTokenAsync(rawToken, cancellationToken);
        return tokenSnapshot is not null
            ? RefreshRotateResult.Idempotent(
                tokenSnapshot,
                newRawToken,
                replacementToken.ExpiresAt)
            : RefreshRotateResult.Locked();
    }
}
