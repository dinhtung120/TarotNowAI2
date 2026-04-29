using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public sealed partial class MongoRefreshTokenRepository
{
    private async Task<bool> AcquireRotateLockAsync(string lockKey, string lockOwner, CancellationToken cancellationToken)
    {
        var lockWindow = TimeSpan.FromSeconds(Math.Max(3, _authSecurityOptions.RefreshLockSeconds));
        return await _cacheService.AcquireLockAsync(lockKey, lockOwner, lockWindow, cancellationToken);
    }

    private static string NormalizeIdempotencyKey(
        string rawKey,
        string tokenHash,
        string deviceId,
        string userAgentHash)
    {
        if (!string.IsNullOrWhiteSpace(rawKey))
        {
            var trimmed = rawKey.Trim();
            return trimmed.Length <= 128 ? trimmed : trimmed[..128];
        }

        var source = $"{tokenHash}|{NormalizeSegment(deviceId, "unknown-device")}|{NormalizeSegment(userAgentHash, "unknown-ua")}";
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(source));
        return $"auto-{Convert.ToHexString(hash).ToLowerInvariant()}";
    }

    private static string NormalizeSegment(string raw, string fallback)
    {
        return string.IsNullOrWhiteSpace(raw) ? fallback : raw.Trim();
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
}
