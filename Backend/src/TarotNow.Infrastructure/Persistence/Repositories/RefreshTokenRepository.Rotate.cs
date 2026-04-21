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
            var current = await LoadForUpdateAsync(context.NormalizedToken, context.TokenHash, cancellationToken);
            if (current is null)
            {
                return await FinalizeResultAsync(localTransaction, RefreshRotateResult.Invalid(), cancellationToken);
            }

            var idempotencyKey = context.NormalizedIdempotencyKey;
            var idemCacheKey = BuildRefreshIdempotencyKey(current.SessionId, idempotencyKey);
            var earlyExit = await TryBuildEarlyExitResultAsync(
                current,
                idempotencyKey,
                idemCacheKey,
                context.TokenIdempotencyCacheKey,
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

    private async Task<RefreshRotateResult?> TryBuildEarlyExitResultAsync(
        RefreshToken current,
        string idempotencyKey,
        string idemCacheKey,
        string tokenIdempotencyCacheKey,
        CancellationToken cancellationToken)
    {
        var cached = await _cacheService.GetAsync<RefreshRotateCacheItem>(idemCacheKey, cancellationToken);
        if (cached is null)
        {
            cached = await _cacheService.GetAsync<RefreshRotateCacheItem>(tokenIdempotencyCacheKey, cancellationToken);
        }

        if (cached is not null
            && string.IsNullOrWhiteSpace(cached.NewRefreshTokenRaw) == false
            && cached.NewTokenExpiresAtUtc > DateTime.MinValue)
        {
            return RefreshRotateResult.Idempotent(current, cached.NewRefreshTokenRaw, cached.NewTokenExpiresAtUtc);
        }

        if (current.IsExpired)
        {
            return RefreshRotateResult.Expired(current);
        }

        if (current.IsActive)
        {
            return null;
        }

        if (IsLockContention(current, idempotencyKey, DateTime.UtcNow))
        {
            return RefreshRotateResult.Locked();
        }

        return RefreshRotateResult.ReplayDetected(current);
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
                normalizedToken,
                tokenHash,
                normalizedIdempotencyKey,
                tokenIdempotencyCacheKey));
    }

    private async Task<RefreshRotateResult> HandleLockContentionAsync(
        string rawToken,
        string tokenIdempotencyCacheKey,
        CancellationToken cancellationToken)
    {
        var tokenCached = await _cacheService.GetAsync<RefreshRotateCacheItem>(tokenIdempotencyCacheKey, cancellationToken);
        if (tokenCached is null
            || string.IsNullOrWhiteSpace(tokenCached.NewRefreshTokenRaw)
            || tokenCached.NewTokenExpiresAtUtc <= DateTime.MinValue)
        {
            return RefreshRotateResult.Locked();
        }

        var tokenSnapshot = await GetByTokenAsync(rawToken, cancellationToken);
        return tokenSnapshot is not null
            ? RefreshRotateResult.Idempotent(
                tokenSnapshot,
                tokenCached.NewRefreshTokenRaw,
                tokenCached.NewTokenExpiresAtUtc)
            : RefreshRotateResult.Locked();
    }

}
