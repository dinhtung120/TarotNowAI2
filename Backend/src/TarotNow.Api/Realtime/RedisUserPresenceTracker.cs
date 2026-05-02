using System.Collections.Generic;
using StackExchange.Redis;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Presence tracker dùng Redis để đồng bộ trạng thái online khi chạy nhiều instance.
public sealed partial class RedisUserPresenceTracker : IUserPresenceTracker
{
    private const string ConnectionsKeyPrefix = "presence:user:";
    private const string ConnectionsKeySuffix = ":connections";
    private const string LastActivityKey = "presence:last-activity";
    private const int MinPresenceLeaseSeconds = 30;
    private const int MaxPresenceLeaseSeconds = 86_400;

    private readonly IConnectionMultiplexer _multiplexer;
    private readonly ISystemConfigSettings _systemConfigSettings;
    private readonly ILogger<RedisUserPresenceTracker> _logger;

    public RedisUserPresenceTracker(
        IConnectionMultiplexer multiplexer,
        ISystemConfigSettings systemConfigSettings,
        ILogger<RedisUserPresenceTracker> logger)
    {
        _multiplexer = multiplexer;
        _systemConfigSettings = systemConfigSettings;
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
            var connectionsKey = GetConnectionsKey(userId);
            db.SetAdd(connectionsKey, connectionId);
            db.KeyExpire(connectionsKey, ResolveConnectionLease());
            RecordHeartbeat(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to mark connected. UserId={UserId}", userId);
        }
    }

    /// <summary>
    /// Đánh dấu một connection bị ngắt.
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
            var connectionsKey = GetConnectionsKey(userId);
            db.SetRemove(connectionsKey, connectionId);
            if (db.SetLength(connectionsKey) <= 0)
            {
                db.KeyDelete(connectionsKey);
                db.SortedSetRemove(LastActivityKey, userId);
            }
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
            var connectionsKey = GetConnectionsKey(userId);
            EnsureConnectionLease(db, connectionsKey);
            if (db.SetLength(connectionsKey) > 0)
            {
                return true;
            }

            var lastActivity = GetLastActivity(userId);
            var onlineWindow = TimeSpan.FromMinutes(
                Math.Clamp(_systemConfigSettings.PresenceTimeoutMinutes, 1, 240));
            return lastActivity is not null && (DateTime.UtcNow - lastActivity.Value) <= onlineWindow;
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
            var connectionsKey = GetConnectionsKey(userId);
            // Gia hạn lease theo heartbeat để session dài không bị rơi active connection do hết TTL.
            if (db.KeyExists(connectionsKey))
            {
                db.KeyExpire(connectionsKey, ResolveConnectionLease());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceRedis] Failed to record heartbeat. UserId={UserId}", userId);
        }
    }

}
