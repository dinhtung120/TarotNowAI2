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

        var normalizedToken = request.RawToken.Trim();
        var tokenHash = RefreshToken.HashToken(normalizedToken);
        var lockKey = BuildRefreshLockKey(tokenHash);
        var lockOwner = Guid.NewGuid().ToString("N");
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(request.IdempotencyKey);
        var tokenIdempotencyCacheKey = BuildRefreshTokenIdempotencyKey(tokenHash, normalizedIdempotencyKey);

        if (!await TryAcquireLockAsync(lockKey, lockOwner, cancellationToken))
        {
            var tokenCached = await _cacheService.GetAsync<RefreshRotateCacheItem>(tokenIdempotencyCacheKey, cancellationToken);
            if (tokenCached is not null
                && string.IsNullOrWhiteSpace(tokenCached.NewRefreshTokenRaw) == false
                && tokenCached.NewTokenExpiresAtUtc > DateTime.MinValue)
            {
                var tokenSnapshot = await GetByTokenAsync(request.RawToken, cancellationToken);
                if (tokenSnapshot is not null)
                {
                    return RefreshRotateResult.Idempotent(
                        tokenSnapshot,
                        tokenCached.NewRefreshTokenRaw,
                        tokenCached.NewTokenExpiresAtUtc);
                }
            }

            return RefreshRotateResult.Locked();
        }

        try
        {
            return await RotateWithTransactionAsync(
                request,
                normalizedToken,
                tokenHash,
                normalizedIdempotencyKey,
                tokenIdempotencyCacheKey,
                cancellationToken);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockOwner, cancellationToken);
        }
    }

    private static bool IsValidRotateRequest(RefreshRotateRequest request)
    {
        return string.IsNullOrWhiteSpace(request.RawToken) == false
            && string.IsNullOrWhiteSpace(request.NewRawToken) == false;
    }

    private async Task<bool> TryAcquireLockAsync(string lockKey, string lockOwner, CancellationToken cancellationToken)
    {
        var lockWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));
        return await _cacheService.AcquireLockAsync(lockKey, lockOwner, lockWindow, cancellationToken);
    }

    private async Task<RefreshRotateResult> RotateWithTransactionAsync(
        RefreshRotateRequest request,
        string normalizedToken,
        string tokenHash,
        string normalizedIdempotencyKey,
        string tokenIdempotencyCacheKey,
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
            var current = await LoadForUpdateAsync(normalizedToken, tokenHash, cancellationToken);
            if (current is null)
            {
                return await FinalizeResultAsync(localTransaction, RefreshRotateResult.Invalid(), cancellationToken);
            }

            var idempotencyKey = normalizedIdempotencyKey;
            var idemCacheKey = BuildRefreshIdempotencyKey(current.SessionId, idempotencyKey);
            var earlyExit = await TryBuildEarlyExitResultAsync(
                current,
                idempotencyKey,
                idemCacheKey,
                tokenIdempotencyCacheKey,
                cancellationToken);

            if (earlyExit is not null)
            {
                return await FinalizeResultAsync(localTransaction, earlyExit, cancellationToken);
            }

            var persistContext = new RotatePersistContext(
                request,
                current,
                idempotencyKey,
                idemCacheKey,
                tokenIdempotencyCacheKey);
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

        if (IsLockContention(current, idempotencyKey))
        {
            return RefreshRotateResult.Locked();
        }

        return RefreshRotateResult.ReplayDetected(current);
    }

    private bool IsLockContention(RefreshToken current, string idempotencyKey)
    {
        return current.ReplacedByTokenId is not null
               && string.Equals(current.LastRotateIdempotencyKey, idempotencyKey, StringComparison.Ordinal);
    }

    private static async Task<RefreshRotateResult> FinalizeResultAsync(
        IDbContextTransaction? localTransaction,
        RefreshRotateResult result,
        CancellationToken cancellationToken)
    {
        if (localTransaction is not null)
        {
            await localTransaction.CommitAsync(cancellationToken);
        }

        return result;
    }
}
