using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task JoinConversation(string conversationId)
    {
        if (!TryGetUserGuid(out var userGuid))
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        if (string.IsNullOrWhiteSpace(conversationId))
        {
            await SendClientErrorAsync("ConversationId is required");
            return;
        }

        var accessStatus = await _mediator.Send(new ValidateConversationAccessQuery
        {
            ConversationId = conversationId,
            RequesterId = userGuid
        });

        if (accessStatus == ConversationAccessStatus.NotFound)
        {
            await SendClientErrorAsync("Conversation not found");
            return;
        }

        if (accessStatus == ConversationAccessStatus.Forbidden)
        {
            await SendClientErrorAsync("Forbidden");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        await Clients.Group(conversationId).SendAsync("UserJoined", new
        {
            userId = userGuid,
            conversationId,
            joinedAt = DateTime.UtcNow
        });

        _logger.LogInformation(
            "[ChatHub] User {UserId} joined conversation {ConversationId}",
            userGuid,
            conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        var userId = GetUserId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);

        _logger.LogInformation(
            "[ChatHub] User {UserId} left conversation {ConversationId}",
            userId,
            conversationId);
    }
}
