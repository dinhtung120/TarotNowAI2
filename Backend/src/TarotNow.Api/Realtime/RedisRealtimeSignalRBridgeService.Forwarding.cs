using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisRealtimeSignalRBridgeService
{
    private async Task ForwardChatEventAsync(string eventName, JsonElement payload)
    {
        if (eventName == RealtimeEventNames.ChatMessageCreated)
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

        if (eventName == RealtimeEventNames.ConversationUpdated)
        {
            await ForwardConversationUpdatedAsync(payload);
            return;
        }

        if (eventName == RealtimeEventNames.ChatUnreadChanged)
        {
            await ForwardUnreadChangedAsync(payload);
            return;
        }

        if (eventName == RealtimeEventNames.ChatMessageRead
            || eventName == RealtimeEventNames.TypingStarted
            || eventName == RealtimeEventNames.TypingStopped)
        {
            await ForwardConversationGroupEventAsync(eventName, payload);
        }
    }

    private async Task ForwardPresenceEventAsync(string eventName, JsonElement payload)
    {
        if (eventName == RealtimeEventNames.UserStatusChanged)
        {
            var broadcastUserId = GetStringProperty(payload, "userId");
            if (string.IsNullOrWhiteSpace(broadcastUserId))
            {
                return;
            }

            var status = GetStringProperty(payload, "status") ?? "offline";
            await _presenceHubContext.Clients.Groups(
                    PresenceGroupNames.User(broadcastUserId),
                    PresenceGroupNames.UserStatusObservers(broadcastUserId))
                .SendAsync(eventName, broadcastUserId, status);
            return;
        }

        var userId = GetStringProperty(payload, "userId");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        await _presenceHubContext.Clients.Group(PresenceGroupNames.User(userId))
            .SendAsync(eventName, payload);
    }

    private async Task ForwardConversationUpdatedAsync(JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        await _chatHubContext.Clients.Group($"conversation:{conversationId}")
            .SendAsync(RealtimeEventNames.ConversationUpdated, payload);

        var groups = GetUserGroups(payload);
        if (groups.Length > 0)
        {
            await _presenceHubContext.Clients.Groups(groups)
                .SendAsync(RealtimeEventNames.ConversationUpdated, payload);
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
            await _chatHubContext.Clients.Group($"conversation:{conversationId}")
                .SendAsync(RealtimeEventNames.ConversationUpdated, unreadPayload);
        }

        var groups = GetUserGroups(payload);
        if (groups.Length > 0)
        {
            await _presenceHubContext.Clients.Groups(groups)
                .SendAsync(RealtimeEventNames.ConversationUpdated, unreadPayload);
        }
    }

    private async Task ForwardConversationGroupEventAsync(string eventName, JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        await _chatHubContext.Clients.Group($"conversation:{conversationId}")
            .SendAsync(eventName, payload);
    }

}
