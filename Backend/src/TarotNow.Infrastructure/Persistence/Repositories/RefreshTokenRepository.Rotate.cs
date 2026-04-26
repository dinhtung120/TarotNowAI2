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
                context.TransactionContext.NormalizedIdempotencyKey,
                cancellationToken);
        }

        try
        {
            return await RotateWithTransactionAsync(context.TransactionContext, cancellationToken);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(context.LockKey, context.LockOwner, CancellationToken.None);
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
            var earlyExit = await TryBuildEarlyExitResultAsync(
                current,
                context,
                cancellationToken);

            if (earlyExit is not null)
            {
                return await FinalizeResultAsync(localTransaction, earlyExit, cancellationToken);
            }

            var persistContext = new RotatePersistContext(
                context.Request,
                current,
                idempotencyKey);
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
        return new RotateExecutionContext(
            LockKey: BuildRefreshLockKey(tokenHash),
            LockOwner: Guid.NewGuid().ToString("N"),
            TransactionContext: new RotateTransactionContext(
                request,
                tokenHash,
                normalizedIdempotencyKey));
    }

    private async Task<RefreshRotateResult> HandleLockContentionAsync(
        string rawToken,
        string newRawToken,
        string normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken) || string.IsNullOrWhiteSpace(newRawToken))
        {
            return RefreshRotateResult.Locked();
        }

        var tokenHash = RefreshToken.HashToken(rawToken.Trim());
        const int maxAttempts = 5;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var tokenSnapshot = await LoadTokenSnapshotByHashAsync(tokenHash, cancellationToken);
            if (tokenSnapshot is null)
            {
                return RefreshRotateResult.Locked();
            }

            var idempotentResult = await TryBuildIdempotentLockContentionResultAsync(
                tokenSnapshot,
                normalizedIdempotencyKey,
                newRawToken,
                cancellationToken);
            if (idempotentResult is not null)
            {
                return idempotentResult;
            }

            if (ShouldStopLockContentionPolling(tokenSnapshot))
            {
                return RefreshRotateResult.Locked();
            }

            await DelayNextLockContentionAttemptAsync(attempt, maxAttempts, cancellationToken);
        }

        return RefreshRotateResult.Locked();
    }

    private async Task<RefreshRotateResult?> TryBuildIdempotentLockContentionResultAsync(
        RefreshToken tokenSnapshot,
        string normalizedIdempotencyKey,
        string newRawToken,
        CancellationToken cancellationToken)
    {
        var sameIdempotency = string.Equals(
            tokenSnapshot.LastRotateIdempotencyKey,
            normalizedIdempotencyKey,
            StringComparison.Ordinal);
        if (!sameIdempotency || !tokenSnapshot.ReplacedByTokenId.HasValue)
        {
            return null;
        }

        var replacementToken = await TryLoadReplacementTokenAsync(tokenSnapshot.ReplacedByTokenId.Value, cancellationToken);
        if (replacementToken is null || !replacementToken.MatchesToken(newRawToken))
        {
            return null;
        }

        return RefreshRotateResult.Idempotent(tokenSnapshot, newRawToken, replacementToken.ExpiresAt);
    }

    private static bool ShouldStopLockContentionPolling(RefreshToken tokenSnapshot)
    {
        return !tokenSnapshot.IsActive;
    }

    private static Task DelayNextLockContentionAttemptAsync(
        int attempt,
        int maxAttempts,
        CancellationToken cancellationToken)
    {
        if (attempt >= maxAttempts)
        {
            return Task.CompletedTask;
        }

        return Task.Delay(TimeSpan.FromMilliseconds(35 * attempt), cancellationToken);
    }
}
