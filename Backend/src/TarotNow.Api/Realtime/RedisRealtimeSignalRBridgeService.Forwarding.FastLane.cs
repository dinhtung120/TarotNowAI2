using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common.Realtime;

namespace TarotNow.Api.Realtime;

public sealed partial class RedisRealtimeSignalRBridgeService
{
    private async Task ForwardChatFastLaneEventAsync(string eventName, JsonElement payload)
    {
        var eventId = GetStringProperty(payload, "eventId");
        if (ShouldSkipDuplicatedFastLaneEvent(eventId))
        {
            return;
        }

        if (eventName == RealtimeEventNames.ChatMessageCreatedFast)
        {
            await ForwardFastMessageCreatedAsync(payload);
            return;
        }

        if (eventName == RealtimeEventNames.ConversationUpdatedDelta)
        {
            await ForwardFastConversationUpdatedAsync(payload);
            return;
        }

        if (eventName == RealtimeEventNames.ChatUnreadDelta)
        {
            await ForwardFastUnreadDeltaAsync(payload);
            return;
        }

        if (eventName == RealtimeEventNames.ChatMessageReadDelta)
        {
            await ForwardFastMessageReadDeltaAsync(payload);
        }
    }

    private async Task ForwardFastMessageCreatedAsync(JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        if (!TryGetProperty(payload, "payload", out var innerPayload))
        {
            return;
        }

        var messagePayload = TryGetProperty(innerPayload, "message", out var messageElement)
            ? messageElement
            : innerPayload;

        await _chatHubContext.Clients.Group($"conversation:{conversationId}")
            .SendAsync(RealtimeEventNames.ChatMessageCreatedFast, messagePayload);
        await _chatHubContext.Clients.Group($"conversation:{conversationId}")
            .SendAsync(RealtimeEventNames.ChatMessageCreated, messagePayload);
    }

    private async Task ForwardFastConversationUpdatedAsync(JsonElement payload)
    {
        if (!TryGetProperty(payload, "payload", out var innerPayload))
        {
            return;
        }

        await ForwardConversationUpdatedAsync(innerPayload);
        await BroadcastConversationDeltaEventAsync(RealtimeEventNames.ConversationUpdatedDelta, innerPayload);
    }

    private async Task ForwardFastUnreadDeltaAsync(JsonElement payload)
    {
        if (!TryGetProperty(payload, "payload", out var innerPayload))
        {
            return;
        }

        await ForwardUnreadChangedAsync(innerPayload);
        await BroadcastConversationDeltaEventAsync(RealtimeEventNames.ChatUnreadDelta, innerPayload);
    }

    private async Task ForwardFastMessageReadDeltaAsync(JsonElement payload)
    {
        if (!TryGetProperty(payload, "payload", out var innerPayload))
        {
            return;
        }

        await ForwardConversationGroupEventAsync(RealtimeEventNames.ChatMessageRead, innerPayload);
        await ForwardConversationGroupEventAsync(RealtimeEventNames.ChatMessageReadDelta, innerPayload);
    }

    private async Task BroadcastConversationDeltaEventAsync(string eventName, JsonElement payload)
    {
        var conversationId = GetStringProperty(payload, "conversationId");
        if (string.IsNullOrWhiteSpace(conversationId) == false)
        {
            await _chatHubContext.Clients.Group($"conversation:{conversationId}")
                .SendAsync(eventName, payload);
        }

        var groups = GetUserGroups(payload);
        if (groups.Length > 0)
        {
            await _presenceHubContext.Clients.Groups(groups)
                .SendAsync(eventName, payload);
        }
    }
}
