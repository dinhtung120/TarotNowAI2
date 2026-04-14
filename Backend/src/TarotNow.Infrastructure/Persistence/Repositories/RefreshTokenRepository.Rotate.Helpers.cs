using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class RefreshTokenRepository
{
    private async Task<RefreshRotateResult> RotateAndPersistAsync(
        RotatePersistContext context,
        IDbContextTransaction? localTransaction,
        CancellationToken cancellationToken)
    {
        context.CurrentToken.MarkUsed(DateTime.UtcNow, context.IdempotencyKey);
        var nextToken = CreateNextToken(context.CurrentToken, context.Request);
        context.CurrentToken.LinkReplacement(nextToken.Id);

        await SaveRotationAsync(context.CurrentToken, nextToken, cancellationToken);
        if (localTransaction is not null)
        {
            await localTransaction.CommitAsync(cancellationToken);
        }

        await CacheIdempotentResultAsync(context.IdempotencyCacheKey, context.Request.NewRawToken, cancellationToken);
        return RefreshRotateResult.Success(context.CurrentToken, nextToken, context.Request.NewRawToken);
    }

    private static RefreshToken CreateNextToken(RefreshToken current, RefreshRotateRequest request)
    {
        var familyId = current.FamilyId == Guid.Empty ? current.Id : current.FamilyId;
        return new RefreshToken(
            userId: current.UserId,
            token: request.NewRawToken,
            expiresAt: request.NewExpiresAtUtc,
            createdByIp: request.IpAddress,
            sessionId: current.SessionId,
            familyId: familyId,
            parentTokenId: current.Id,
            createdDeviceId: request.DeviceId,
            createdUserAgentHash: request.UserAgentHash);
    }

    private async Task SaveRotationAsync(RefreshToken current, RefreshToken nextToken, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.AddAsync(nextToken, cancellationToken);
        _dbContext.RefreshTokens.Update(current);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CacheIdempotentResultAsync(
        string idemCacheKey,
        string newRefreshTokenRaw,
        CancellationToken cancellationToken)
    {
        var window = TimeSpan.FromSeconds(Math.Max(10, _authSecurityOptions.RefreshIdempotencyWindowSeconds));
        var cacheItem = new RefreshRotateCacheItem { NewRefreshTokenRaw = newRefreshTokenRaw };
        await _cacheService.SetAsync(idemCacheKey, cacheItem, window, cancellationToken);
    }

    private async Task<RefreshToken?> LoadForUpdateAsync(string normalizedToken, string hashedToken, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens
            .FromSqlInterpolated($"""
                SELECT * 
                FROM refresh_tokens
                WHERE token = {hashedToken} OR token = {normalizedToken}
                ORDER BY created_at DESC
                LIMIT 1
                FOR UPDATE
                """)
            .Include(x => x.User)
            .FirstOrDefaultAsync(ct);
    }

    private static string NormalizeIdempotencyKey(string rawKey)
    {
        if (string.IsNullOrWhiteSpace(rawKey))
        {
            return Guid.NewGuid().ToString("N");
        }

        var trimmed = rawKey.Trim();
        return trimmed.Length <= 128 ? trimmed : trimmed[..128];
    }

    private static string BuildRefreshLockKey(string tokenHash)
    {
        return $"auth:refresh-lock:{tokenHash}";
    }

    private static string BuildRefreshIdempotencyKey(Guid sessionId, string idempotencyKey)
    {
        var sessionPart = sessionId == Guid.Empty ? "legacy" : sessionId.ToString("N");
        return $"auth:refresh-idem:{sessionPart}:{idempotencyKey}";
    }

    private sealed class RefreshRotateCacheItem
    {
        public string NewRefreshTokenRaw { get; set; } = string.Empty;
    }

    private sealed record RotatePersistContext(
        RefreshRotateRequest Request,
        RefreshToken CurrentToken,
        string IdempotencyKey,
        string IdempotencyCacheKey);
}
