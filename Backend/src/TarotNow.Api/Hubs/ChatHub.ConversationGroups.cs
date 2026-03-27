using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task JoinConversation(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        var validationError = await ValidateJoinConversationAsync(conversationId, userGuid);
        if (validationError != null)
        {
            await SendClientErrorAsync(validationError);
            return;
        }

        await AddConnectionToConversationGroupsAsync(conversationId);
        await BroadcastConversationJoinedAsync(conversationId, userGuid);
        LogConversationJoined(conversationId, userGuid);
    }

    public async Task LeaveConversation(string conversationId)
    {
        var userId = GetUserId();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConversationGroup(conversationId));

        _logger.LogInformation(
            "[ChatHub] User {UserId} left conversation {ConversationId}",
            userId,
            conversationId);
    }
}
