using System.Collections.Generic;
using StackExchange.Redis;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisUserPresenceTracker : IUserPresenceTracker
{
    /// <summary>
    /// Lấy timestamp hoạt động cuối cùng của user.
    /// </summary>
    public DateTime? GetLastActivity(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            return ReadLastActivity(db, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to read last activity. UserId={UserId}", userId);
            return null;
        }
    }

    /// <summary>
    /// Lấy danh sách user timeout (không còn connections và quá ngưỡng heartbeat).
    /// </summary>
    public IReadOnlyList<string> GetTimedOutUsers(TimeSpan timeout)
    {
        try
        {
            var db = _multiplexer.GetDatabase();
            var cutoff = DateTimeOffset.UtcNow.Subtract(timeout).ToUnixTimeSeconds();
            var timedOutCandidates = db.SortedSetRangeByScore(LastActivityKey, double.NegativeInfinity, cutoff);
            var result = new List<string>(timedOutCandidates.Length);

            foreach (var userId in timedOutCandidates)
            {
                if (userId.IsNullOrEmpty)
                {
                    continue;
                }

                var resolvedUserId = userId.ToString();
                if (string.IsNullOrWhiteSpace(resolvedUserId))
                {
                    continue;
                }

                var connectionsKey = GetConnectionsKey(resolvedUserId);
                var activeConnections = db.SetLength(connectionsKey);
                if (activeConnections <= 0)
                {
                    result.Add(resolvedUserId);
                    continue;
                }

                if (TryPruneStaleConnections(db, resolvedUserId, connectionsKey, activeConnections))
                {
                    result.Add(resolvedUserId);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to query timed out users.");
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Xóa toàn bộ state presence của user.
    /// </summary>
    public void RemoveUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            db.KeyDelete(GetConnectionsKey(userId));
            db.SortedSetRemove(LastActivityKey, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to remove user state. UserId={UserId}", userId);
        }
    }

    private static string GetConnectionsKey(string userId)
    {
        return $"{ConnectionsKeyPrefix}{userId}{ConnectionsKeySuffix}";
    }

    private TimeSpan ResolveConnectionLease()
    {
        var timeoutMinutes = Math.Clamp(_systemConfigSettings.PresenceTimeoutMinutes, 1, 240);
        var seconds = Math.Clamp(timeoutMinutes * 120, MinPresenceLeaseSeconds, MaxPresenceLeaseSeconds);
        return TimeSpan.FromSeconds(seconds);
    }

    private void EnsureConnectionLease(IDatabase db, string connectionsKey)
    {
        if (!db.KeyExists(connectionsKey))
        {
            return;
        }

        var ttl = db.KeyTimeToLive(connectionsKey);
        if (ttl.HasValue && ttl.Value > TimeSpan.Zero)
        {
            return;
        }

        db.KeyExpire(connectionsKey, ResolveConnectionLease());
    }

    private bool HasNonStaleActiveConnections(IDatabase db, string userId, string connectionsKey)
    {
        EnsureConnectionLease(db, connectionsKey);
        var activeConnections = db.SetLength(connectionsKey);
        if (activeConnections <= 0)
        {
            return false;
        }

        return !TryPruneStaleConnections(db, userId, connectionsKey, activeConnections);
    }

    private bool TryPruneStaleConnections(
        IDatabase db,
        string userId,
        string connectionsKey,
        long activeConnections)
    {
        if (activeConnections <= 0)
        {
            return false;
        }

        var lastActivity = ReadLastActivity(db, userId);
        if (lastActivity is null)
        {
            return false;
        }

        var now = DateTime.UtcNow;
        var idleDuration = now - lastActivity.Value;
        var staleWindow = ResolveStaleConnectionWindow();
        if (idleDuration <= staleWindow)
        {
            return false;
        }

        var deletedConnections = db.KeyDelete(connectionsKey);
        _logger.LogWarning(
            "[PresenceRedis] Pruned stale active connections. UserId={UserId}, ActiveConnections={ActiveConnections}, IdleSeconds={IdleSeconds}, StaleWindowSeconds={StaleWindowSeconds}, DeletedConnections={DeletedConnections}",
            userId,
            activeConnections,
            Math.Round(idleDuration.TotalSeconds),
            Math.Round(staleWindow.TotalSeconds),
            deletedConnections);

        return true;
    }

    private DateTime? ReadLastActivity(IDatabase db, string userId)
    {
        var score = db.SortedSetScore(LastActivityKey, userId);
        if (!score.HasValue)
        {
            return null;
        }

        return DateTimeOffset.FromUnixTimeSeconds((long)score.Value).UtcDateTime;
    }

    private TimeSpan ResolveStaleConnectionWindow()
    {
        var timeoutSeconds = Math.Clamp(_systemConfigSettings.PresenceTimeoutMinutes, 1, 240) * 60;
        var scanIntervalSeconds = Math.Clamp(_systemConfigSettings.PresenceScanIntervalSeconds, 5, 600);
        var leaseSeconds = (int)ResolveConnectionLease().TotalSeconds;
        var staleWindowSeconds = timeoutSeconds + Math.Max(scanIntervalSeconds, 60);
        var maxStaleWindowSeconds = Math.Max(timeoutSeconds, leaseSeconds - 10);
        var boundedWindowSeconds = Math.Clamp(staleWindowSeconds, timeoutSeconds, maxStaleWindowSeconds);
        return TimeSpan.FromSeconds(boundedWindowSeconds);
    }
}
