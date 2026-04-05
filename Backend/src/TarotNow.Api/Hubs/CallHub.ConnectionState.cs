using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private static readonly TimeSpan DisconnectGracePeriod = TimeSpan.FromSeconds(12);
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> UserConnections =
        new(StringComparer.Ordinal);
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> UserConversationAccess =
        new(StringComparer.Ordinal);

    private static void RegisterConnection(string userId, string connectionId)
    {
        var connectionMap = UserConnections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        connectionMap[connectionId] = 0;
    }

    private static void UnregisterConnection(string userId, string connectionId)
    {
        if (!UserConnections.TryGetValue(userId, out var connectionMap))
        {
            return;
        }

        connectionMap.TryRemove(connectionId, out _);
        if (connectionMap.IsEmpty)
        {
            UserConnections.TryRemove(userId, out _);
            UserConversationAccess.TryRemove(userId, out _);
        }
    }

    private static bool HasAnyConnection(string userId)
    {
        return UserConnections.TryGetValue(userId, out var connectionMap) && !connectionMap.IsEmpty;
    }

    private static void RememberConversationAccess(string userId, string conversationId)
    {
        var accessMap = UserConversationAccess.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        accessMap[conversationId] = 0;
    }

    private static void RememberConversationAccess(string userId, IEnumerable<string> conversationIds)
    {
        var accessMap = UserConversationAccess.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        foreach (var conversationId in conversationIds.Where(id => !string.IsNullOrWhiteSpace(id)))
        {
            accessMap[conversationId] = 0;
        }
    }

    private static bool HasConversationAccessCached(string userId, string conversationId)
    {
        return UserConversationAccess.TryGetValue(userId, out var accessMap) && accessMap.ContainsKey(conversationId);
    }

    private async Task DelayCleanupForTransientDisconnectAsync(string userId)
    {
        await Task.Delay(DisconnectGracePeriod);
        if (HasAnyConnection(userId))
        {
            return;
        }

        await HandleDisconnectedUserCleanupAsync(userId);
    }
}
