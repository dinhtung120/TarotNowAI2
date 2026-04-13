using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisRealtimeSignalRBridgeService
{
    private async Task ForwardChatEventAsync(string eventName, JsonElement payload)
    {
        if (eventName == "message.created")
        {
            var conversationId = GetStringProperty(payload, "conversationId");
            if (string.IsNullOrWhiteSpace(conversationId))
            {
                return;
            }

            var chatPayload = TryGetProperty(payload, "message", out var messageElement) ? messageElement : payload;
            await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync(eventName, chatPayload);
            return;
        }

        if (eventName == "conversation.updated")
        {
            await ForwardConversationUpdatedAsync(payload);
            return;
        }

        if (eventName == "chat.unread_changed")
        {
            await ForwardUnreadChangedAsync(payload);
        }
    }

    private async Task ForwardPresenceEventAsync(string eventName, JsonElement payload)
    {
        var userId = GetStringProperty(payload, "userId");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        await _presenceHubContext.Clients.Group($"user:{userId}").SendAsync(eventName, payload);
    }

    private async Task ForwardConversationUpdatedAsync(JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("conversation.updated", payload);

        var groups = GetUserGroups(payload);
        if (groups.Length > 0)
        {
            await _presenceHubContext.Clients.Groups(groups).SendAsync("conversation.updated", payload);
        }
    }

    private async Task ForwardUnreadChangedAsync(JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        var unreadPayload = new
        {
            conversationId,
            type = "unread_changed",
            at = DateTime.UtcNow
        };

        if (string.IsNullOrWhiteSpace(conversationId) == false)
        {
            await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("conversation.updated", unreadPayload);
        }

        var groups = GetUserGroups(payload);
        if (groups.Length > 0)
        {
            await _presenceHubContext.Clients.Groups(groups).SendAsync("conversation.updated", unreadPayload);
        }
    }

    private static string[] GetUserGroups(JsonElement payload)
    {
        var groups = new List<string>(2);
        AddUserGroup(groups, GetStringProperty(payload, "userId"));
        AddUserGroup(groups, GetStringProperty(payload, "readerId"));
        return groups.ToArray();
    }

    private static void AddUserGroup(List<string> groups, string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var groupName = $"user:{userId}";
        if (groups.Contains(groupName, StringComparer.Ordinal))
        {
            return;
        }

        groups.Add(groupName);
    }

    private static string? GetStringProperty(JsonElement payload, string propertyName)
    {
        if (!TryGetProperty(payload, propertyName, out var value))
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.GetRawText(),
            _ => null
        };
    }

    private static bool TryGetProperty(JsonElement payload, string propertyName, out JsonElement value)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            value = default;
            return false;
        }

        if (payload.TryGetProperty(propertyName, out value))
        {
            return true;
        }

        foreach (var property in payload.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }
}
