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
            var score = db.SortedSetScore(LastActivityKey, userId);
            if (!score.HasValue)
            {
                return null;
            }

            return DateTimeOffset.FromUnixTimeSeconds((long)score.Value).UtcDateTime;
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

                if (db.SetLength(GetConnectionsKey(userId!)) == 0)
                {
                    result.Add(userId!);
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
}
