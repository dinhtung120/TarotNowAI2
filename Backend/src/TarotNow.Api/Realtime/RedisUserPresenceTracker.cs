using System.Collections.Generic;
using StackExchange.Redis;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Realtime;

// Presence tracker dùng Redis để đồng bộ trạng thái online khi chạy nhiều instance.
public sealed class RedisUserPresenceTracker : IUserPresenceTracker
{
    private const string ConnectionsKeyPrefix = "presence:user:";
    private const string ConnectionsKeySuffix = ":connections";
    private const string LastActivityKey = "presence:last-activity";
    private static readonly TimeSpan OnlineWindow = TimeSpan.FromMinutes(15);

    private readonly IConnectionMultiplexer _multiplexer;
    private readonly ILogger<RedisUserPresenceTracker> _logger;

    public RedisUserPresenceTracker(
        IConnectionMultiplexer multiplexer,
        ILogger<RedisUserPresenceTracker> logger)
    {
        _multiplexer = multiplexer;
        _logger = logger;
    }

    /// <summary>
    /// Đánh dấu user connected và ghi heartbeat.
    /// </summary>
    public void MarkConnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            return;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            db.SetAdd(GetConnectionsKey(userId), connectionId);
            RecordHeartbeat(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to mark connected. UserId={UserId}", userId);
        }
    }

    /// <summary>
    /// Đánh dấu một connection bị ngắt và cập nhật heartbeat.
    /// </summary>
    public void MarkDisconnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            return;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            db.SetRemove(GetConnectionsKey(userId), connectionId);
            RecordHeartbeat(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to mark disconnected. UserId={UserId}", userId);
        }
    }

    /// <summary>
    /// Kiểm tra trạng thái online dựa trên connection set và heartbeat.
    /// </summary>
    public bool IsOnline(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            if (db.SetLength(GetConnectionsKey(userId)) > 0)
            {
                return true;
            }

            var lastActivity = GetLastActivity(userId);
            return lastActivity is not null && (DateTime.UtcNow - lastActivity.Value) <= OnlineWindow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to check online status. UserId={UserId}", userId);
            return false;
        }
    }

    /// <summary>
    /// Cập nhật heartbeat timestamp theo Unix seconds.
    /// </summary>
    public void RecordHeartbeat(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        try
        {
            var db = _multiplexer.GetDatabase();
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            db.SortedSetAdd(LastActivityKey, userId, nowUnix);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to record heartbeat. UserId={UserId}", userId);
        }
    }

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
}
