using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Chat.Queries.GetConversationParticipants;

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

        var participants = await _mediator.Send(new GetConversationParticipantsQuery
        {
            ConversationId = conversationId
        });

        if (participants == null)
        {
            return;
        }

        var userGroups = new[]
        {
            UserGroup(participants.UserId),
            UserGroup(participants.ReaderId)
        };

        await Clients.Groups(userGroups).SendAsync("conversation.updated", new
        {
            conversationId,
            type,
            at = atUtc
        });
    }
}
