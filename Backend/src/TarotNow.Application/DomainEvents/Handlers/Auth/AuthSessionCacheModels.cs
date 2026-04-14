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
    public static string BuildSessionKey(Guid sessionId) => $"auth:session:{sessionId}";

    public static string BuildSessionRevokedKey(Guid sessionId) => $"auth:session-revoked:{sessionId}";

    public static string BuildAccessBlacklistKey(string jti) => $"auth:access-blacklist:{jti}";

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
        CancellationToken cancellationToken)
    {
        var key = BuildUserSessionIndexKey(userId);
        var sessions = await cacheService.GetAsync<List<Guid>>(key, cancellationToken) ?? new List<Guid>();
        if (sessions.Contains(sessionId))
        {
            return;
        }

        sessions.Add(sessionId);
        await cacheService.SetAsync(key, sessions, TimeSpan.FromDays(30), cancellationToken);
    }

    public static async Task RemoveSessionFromUserIndexAsync(
        ICacheService cacheService,
        Guid userId,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var key = BuildUserSessionIndexKey(userId);
        var sessions = await cacheService.GetAsync<List<Guid>>(key, cancellationToken);
        if (sessions is null || sessions.Count == 0)
        {
            return;
        }

        sessions.RemoveAll(x => x == sessionId);
        if (sessions.Count == 0)
        {
            await cacheService.RemoveAsync(key, cancellationToken);
            return;
        }

        await cacheService.SetAsync(key, sessions, TimeSpan.FromDays(30), cancellationToken);
    }

    public static string BuildUserSessionIndexKey(Guid userId) => $"auth:user-sessions:{userId}";
}
