using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    private async Task BroadcastConversationUpdatedToParticipantsAsync(
        string conversationId,
        string type,
        DateTime atUtc)
    {
        if (string.IsNullOrWhiteSpace(conversationId))
        {
            return;
        }

        var conversation = await _conversationRepository.GetByIdAsync(conversationId);
        if (conversation == null)
        {
            return;
        }

        var userGroups = new[]
        {
            UserGroup(conversation.UserId),
            UserGroup(conversation.ReaderId)
        };

        await Clients.Groups(userGroups).SendAsync("conversation.updated", new
        {
            conversationId,
            type,
            at = atUtc
        });
    }
}
