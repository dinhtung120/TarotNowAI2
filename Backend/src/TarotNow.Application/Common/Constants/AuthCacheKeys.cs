namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập trung định danh cache keys cho luồng auth/session để tránh magic string.
/// </summary>
public static class AuthCacheKeys
{
    public const string AuthCleanupLockKey = "auth:cleanup:lock";

    public static string BuildSessionSnapshotKey(Guid sessionId) => $"auth:session:{sessionId}";

    public static string BuildSessionRevokedKey(Guid sessionId) => $"auth:session-revoked:{sessionId}";

    public static string BuildAccessBlacklistKey(string jti) => $"auth:access-blacklist:{jti}";

    public static string BuildUserSessionsIndexKey(Guid userId) => $"auth:user-sessions:{userId}";

    public static string BuildReplaySecurityKey(Guid sessionId) => $"auth:security:replay:{sessionId}";

    public static string BuildRefreshLockKey(string tokenHash) => $"auth:refresh-lock:{tokenHash}";

    public static string BuildRefreshSessionIdempotencyKey(Guid sessionId, string idempotencyKey)
    {
        var sessionPart = sessionId == Guid.Empty ? "legacy" : sessionId.ToString("N");
        return $"auth:refresh-idem:{sessionPart}:{idempotencyKey}";
    }

    public static string BuildRefreshTokenIdempotencyKey(string tokenHash, string idempotencyKey)
        => $"auth:refresh-idem-token:{tokenHash}:{idempotencyKey}";

    public static string BuildLoginFailureByIdentityKey(string identityHash)
        => $"auth:login-fail:identity:{identityHash}";

    public static string BuildLoginFailureByIpKey(string ipHash)
        => $"auth:login-fail:ip:{ipHash}";
}
