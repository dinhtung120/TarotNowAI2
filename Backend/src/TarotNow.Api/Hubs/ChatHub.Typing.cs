using Microsoft.AspNetCore.SignalR;

namespace TarotNow.Api.Hubs;

public partial class ChatHub
{
    public async Task TypingStarted(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await Clients.Group(ConversationGroup(conversationId)).SendAsync("typing.started", new
        {
            conversationId,
            userId = userGuid,
            at = DateTime.UtcNow
        });
    }

    public async Task TypingStopped(string conversationId)
    {
        if (TryGetUserGuid(out var userGuid) == false)
        {
            await SendClientErrorAsync("Unauthorized");
            return;
        }

        await Clients.Group(ConversationGroup(conversationId)).SendAsync("typing.stopped", new
        {
            conversationId,
            userId = userGuid,
            at = DateTime.UtcNow
        });
    }
}
