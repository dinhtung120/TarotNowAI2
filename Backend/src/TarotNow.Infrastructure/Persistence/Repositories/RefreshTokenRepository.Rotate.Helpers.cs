using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.Constants;
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

    private async Task<RefreshToken?> LoadForUpdateAsync(string hashedToken, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens
            .FromSqlInterpolated($"""
                SELECT * 
                FROM refresh_tokens
                WHERE token = {hashedToken}
                ORDER BY created_at DESC
                LIMIT 1
                FOR UPDATE
                """)
            .Include(x => x.User)
            .FirstOrDefaultAsync(ct);
    }

    private async Task<RefreshToken?> LoadTokenSnapshotByHashAsync(string hashedToken, CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == hashedToken, cancellationToken);
    }

    private static string NormalizeIdempotencyKey(
        string rawKey,
        string tokenHash,
        string deviceId,
        string userAgentHash)
    {
        if (string.IsNullOrWhiteSpace(rawKey))
        {
            return BuildDeterministicIdempotencyKey(tokenHash, deviceId, userAgentHash);
        }

        var trimmed = rawKey.Trim();
        return trimmed.Length <= 128 ? trimmed : trimmed[..128];
    }

    private static string BuildDeterministicIdempotencyKey(
        string tokenHash,
        string deviceId,
        string userAgentHash)
    {
        var normalizedDeviceId = string.IsNullOrWhiteSpace(deviceId) ? "unknown-device" : deviceId.Trim();
        var normalizedUserAgentHash = string.IsNullOrWhiteSpace(userAgentHash) ? "unknown-ua" : userAgentHash.Trim();
        var source = $"{tokenHash}|{normalizedDeviceId}|{normalizedUserAgentHash}";
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(source));
        return $"auto-{Convert.ToHexString(hash).ToLowerInvariant()}";
    }

    private static string BuildRefreshLockKey(string tokenHash)
    {
        return AuthCacheKeys.BuildRefreshLockKey(tokenHash);
    }

    private sealed record RotatePersistContext(
        RefreshRotateRequest Request,
        RefreshToken CurrentToken,
        string IdempotencyKey);
}
