using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Realtime;

public class InMemoryUserPresenceTracker : IUserPresenceTracker
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _connectionsByUser = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, DateTime> _lastActivity = new(StringComparer.OrdinalIgnoreCase);

    public void MarkConnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            return;
        }

        var userConnections = _connectionsByUser.GetOrAdd(
            userId,
            _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        userConnections[connectionId] = 1;
        
        RecordHeartbeat(userId);
    }

    public void MarkDisconnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            return;
        }

        if (_connectionsByUser.TryGetValue(userId, out var userConnections))
        {
            userConnections.TryRemove(connectionId, out _);
            if (userConnections.IsEmpty)
            {
                _connectionsByUser.TryRemove(userId, out _);
            }
        }
        
        // Ngay cả khi disconnect, chúng ta vẫn record heartbeat một lần cuối
        // để bắt đầu đếm ngược timeout 15 phút
        RecordHeartbeat(userId);
    }

    public bool IsOnline(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        // Ưu tiên 1: Đang có connection mở
        if (_connectionsByUser.TryGetValue(userId, out var userConnections) && !userConnections.IsEmpty)
        {
            return true;
        }
        
        // Ưu tiên 2: Không có connection nhưng vẫn nằm trong khoảng timeout 15 phút
        if (_lastActivity.TryGetValue(userId, out var lastActivityTime))
        {
            // Timeout mặc định là 15 phút. Nếu vẫn trong khoảng thời gian này, coi như vẫn online
            return (DateTime.UtcNow - lastActivityTime).TotalMinutes <= 15;
        }

        return false;
    }

    public void RecordHeartbeat(string userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            _lastActivity[userId] = DateTime.UtcNow;
        }
    }

    public DateTime? GetLastActivity(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return _lastActivity.TryGetValue(userId, out var lastActivityTime) ? lastActivityTime : null;
    }

    public IReadOnlyList<string> GetTimedOutUsers(TimeSpan timeout)
    {
        var cutoffTime = DateTime.UtcNow - timeout;
        
        // Chỉ quét những user KHÔNG có connection nào đang mở VÀ lần cuối heartbeat đã quá timeout
        return _lastActivity
            .Where(kvp => kvp.Value <= cutoffTime && 
                          (!_connectionsByUser.TryGetValue(kvp.Key, out var userConnections) || userConnections.IsEmpty))
            .Select(kvp => kvp.Key)
            .ToList();
    }

    public void RemoveUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        _connectionsByUser.TryRemove(userId, out _);
        _lastActivity.TryRemove(userId, out _);
    }
}
