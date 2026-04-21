using TarotNow.Application.Common.Constants;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

internal sealed class AuthSessionCacheSnapshot
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string UserAgentHash { get; set; } = string.Empty;
    public string LastIpHash { get; set; } = string.Empty;
    public DateTime LastSeenAtUtc { get; set; } = DateTime.UtcNow;
    public string LastAccessJti { get; set; } = string.Empty;
}

internal static class AuthEventCacheHelpers
{
    public static string BuildSessionKey(Guid sessionId) => AuthCacheKeys.BuildSessionSnapshotKey(sessionId);

    public static string BuildSessionRevokedKey(Guid sessionId) => AuthCacheKeys.BuildSessionRevokedKey(sessionId);

    public static string BuildAccessBlacklistKey(string jti) => AuthCacheKeys.BuildAccessBlacklistKey(jti);

    public static async Task MarkSessionRevokedAsync(
        ICacheService cacheService,
        Guid sessionId,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        await cacheService.SetAsync(BuildSessionRevokedKey(sessionId), "1", ttl, cancellationToken);
    }

    public static async Task BlacklistAccessJtiAsync(
        ICacheService cacheService,
        string? jti,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(jti))
        {
            return;
        }

        await cacheService.SetAsync(BuildAccessBlacklistKey(jti.Trim()), "1", ttl, cancellationToken);
    }

    public static async Task AddSessionToUserIndexAsync(
        ICacheService cacheService,
        Guid userId,
        Guid sessionId,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        await cacheService.AddToSetAsync(
            BuildUserSessionIndexKey(userId),
            sessionId.ToString("N"),
            ttl,
            cancellationToken);
    }

    public static async Task RemoveSessionFromUserIndexAsync(
        ICacheService cacheService,
        Guid userId,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        await cacheService.RemoveFromSetAsync(
            BuildUserSessionIndexKey(userId),
            sessionId.ToString("N"),
            cancellationToken);
    }

    public static string BuildUserSessionIndexKey(Guid userId) => AuthCacheKeys.BuildUserSessionsIndexKey(userId);
}
