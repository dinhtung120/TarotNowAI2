using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class MongoRefreshTokenRepository
{
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

        var tokenHash = RefreshToken.HashToken(request.RawToken.Trim());
        var normalizedIdempotencyKey = NormalizeIdempotencyKey(
            request.IdempotencyKey,
            tokenHash,
            request.DeviceId,
            request.UserAgentHash);

        var lockKey = AuthCacheKeys.BuildRefreshLockKey(tokenHash);
        var lockOwner = Guid.NewGuid().ToString("N");
        if (!await AcquireRotateLockAsync(lockKey, lockOwner, cancellationToken))
        {
            return await HandleLockContentionAsync(tokenHash, request.NewRawToken, normalizedIdempotencyKey, cancellationToken);
        }

        try
        {
            return await RotateWithinLockAsync(request, tokenHash, normalizedIdempotencyKey, cancellationToken);
        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockOwner, CancellationToken.None);
        }
    }

    private async Task<RefreshRotateResult> RotateWithinLockAsync(
        RefreshRotateRequest request,
        string tokenHash,
        string normalizedIdempotencyKey,
        CancellationToken cancellationToken)
    {
        var currentDocument = await FindByTokenHashAsync(tokenHash, cancellationToken);
        if (currentDocument is null)
        {
            return RefreshRotateResult.Invalid();
        }

        var currentToken = ToEntity(currentDocument);
        var earlyExit = await TryBuildEarlyExitResultAsync(
            currentToken,
            request.NewRawToken,
            normalizedIdempotencyKey,
            DateTime.UtcNow,
            cancellationToken);
        if (earlyExit is not null)
        {
            return earlyExit;
        }

        currentToken.MarkUsed(DateTime.UtcNow, normalizedIdempotencyKey);
        var nextToken = CreateNextToken(currentToken, request);
        currentToken.LinkReplacement(nextToken.Id);

        await UpdateAsync(currentToken, cancellationToken);
        await AddAsync(nextToken, cancellationToken);
        return RefreshRotateResult.Success(currentToken, nextToken, request.NewRawToken);
    }

    private static bool IsValidRotateRequest(RefreshRotateRequest request)
    {
        return !string.IsNullOrWhiteSpace(request.RawToken)
               && !string.IsNullOrWhiteSpace(request.NewRawToken);
    }
}
