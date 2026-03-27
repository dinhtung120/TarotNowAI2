using System.Collections.Concurrent;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Realtime;

public class InMemoryUserPresenceTracker : IUserPresenceTracker
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _connectionsByUser = new(StringComparer.OrdinalIgnoreCase);

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
    }

    public void MarkDisconnected(string userId, string connectionId)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
        {
            return;
        }

        if (_connectionsByUser.TryGetValue(userId, out var userConnections) == false)
        {
            return;
        }

        userConnections.TryRemove(connectionId, out _);
        if (userConnections.IsEmpty)
        {
            _connectionsByUser.TryRemove(userId, out _);
        }
    }

    public bool IsOnline(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        return _connectionsByUser.TryGetValue(userId, out var userConnections)
            && userConnections.IsEmpty == false;
    }
}
