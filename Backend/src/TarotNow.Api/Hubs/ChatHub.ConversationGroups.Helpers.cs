using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    private async Task<string?> ValidateJoinConversationAsync(string conversationId, Guid userGuid)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return "ConversationId is required";
        }

        var accessStatus = await _mediator.Send(new ValidateConversationAccessQuery
        {
            ConversationId = conversationId,
            RequesterId = userGuid
        });
        return MapConversationAccessError(accessStatus);
    }

    private static string? MapConversationAccessError(ConversationAccessStatus accessStatus)
    {
        if (accessStatus == ConversationAccessStatus.NotFound)
        {
            return "Conversation not found";
        }

        return accessStatus == ConversationAccessStatus.Forbidden ? "Forbidden" : null;
    }

    private async Task AddConnectionToConversationGroupsAsync(string conversationId)
    {
        var groupKey = ConversationGroup(conversationId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupKey);
    }

    private async Task BroadcastConversationJoinedAsync(string conversationId, Guid userGuid)
    {
        var now = DateTime.UtcNow;
        var groupKey = ConversationGroup(conversationId);

        await Clients.Group(groupKey).SendAsync("conversation.updated", new
        {
            conversationId,
            type = "member_joined",
            userId = userGuid,
            at = now
        });
    }

    private void LogConversationJoined(string conversationId, Guid userGuid)
    {
        _logger.LogInformation(
            "[ChatHub] User {UserId} joined conversation {ConversationId}",
            userGuid,
            conversationId);
    }
}
