using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

public sealed class SignalRCallRealtimePushService : ICallRealtimePushService
{
    private readonly IHubContext<CallHub> _callHubContext;

    public SignalRCallRealtimePushService(IHubContext<CallHub> callHubContext)
    {
        _callHubContext = callHubContext;
    }

    public Task BroadcastIncomingAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default)
    {
        var payload = BuildPayload(session, CallSessionV2Statuses.Requested, null, timeouts);
        return _callHubContext.Clients.Group(UserGroup(session.CalleeId)).SendAsync("call.incoming", payload, ct);
    }

    public Task BroadcastAcceptedAsync(CallSessionV2Dto session, CallTimeoutsDto? timeouts = null, CancellationToken ct = default)
    {
        var payload = BuildPayload(session, CallSessionV2Statuses.Accepted, null, timeouts);
        return _callHubContext.Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("call.accepted", payload, ct);
    }

    public Task BroadcastEndedAsync(CallSessionV2Dto session, string reason, CancellationToken ct = default)
    {
        var payload = BuildPayload(session, session.Status, reason);
        return _callHubContext.Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("call.ended", payload, ct);
    }

    private static object BuildPayload(
        CallSessionV2Dto session,
        string status,
        string? reason = null,
        CallTimeoutsDto? timeouts = null)
    {
        return new
        {
            sessionId = session.Id,
            conversationId = session.ConversationId,
            status,
            reason,
            timeouts,
            session,
        };
    }

    private static string ConversationGroup(string conversationId)
    {
        return $"conversation:{conversationId}";
    }

    private static string UserGroup(string userId)
    {
        return $"user:{userId}";
    }
}
