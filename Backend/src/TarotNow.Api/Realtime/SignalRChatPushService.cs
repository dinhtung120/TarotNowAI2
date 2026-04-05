using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

public class SignalRChatPushService : IChatPushService
{
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IHubContext<CallHub> _callHubContext;

    public SignalRChatPushService(IHubContext<ChatHub> chatHubContext, IHubContext<CallHub> callHubContext)
    {
        _chatHubContext = chatHubContext;
        _callHubContext = callHubContext;
    }

    public async Task BroadcastMessageAsync(string conversationId, ChatMessageDto message, CancellationToken ct = default)
    {
        await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("message.created", message, ct);
    }

    public async Task BroadcastConversationUpdatedAsync(string conversationId, string updateType, CancellationToken ct = default)
    {
        await _chatHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("conversation.updated", new
        {
            conversationId = conversationId,
            type = updateType
        }, ct);
    }

    public async Task BroadcastCallEndedAsync(string conversationId, CallSessionDto session, string reason, CancellationToken ct = default)
    {
        await _callHubContext.Clients.Group($"conversation:{conversationId}").SendAsync("call.ended", new
        {
            session = session,
            reason = reason
        }, ct);
    }
}
