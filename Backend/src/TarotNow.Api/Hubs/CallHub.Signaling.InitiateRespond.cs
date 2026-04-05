using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Call.Commands.InitiateCall;
using TarotNow.Application.Features.Call.Commands.RespondCall;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Bắt đầu một cuộc gọi mới vào conversation.
    /// </summary>
    public async Task InitiateCall(string conversationId, string callType)
    {
        if (!TryGetUserGuid(out var userId))
        {
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_initiate",
            userId,
            TimeSpan.FromSeconds(5),
            "Bạn đang thao tác quá nhanh. Vui lòng chờ 5 giây rồi thử lại.");

        if (!allowed)
        {
            return;
        }

        await ExecuteInitiateCallAsync(conversationId, callType, userId);
    }

    /// <summary>
    /// Phản hồi đồng ý hoặc từ chối một cuộc gọi đến.
    /// </summary>
    public async Task RespondCall(string callSessionId, bool accept)
    {
        if (!TryGetUserGuid(out var userId))
        {
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_respond",
            userId,
            TimeSpan.FromSeconds(2),
            "Thao tác quá nhanh.");

        if (!allowed)
        {
            return;
        }

        await ExecuteRespondCallAsync(callSessionId, accept, userId);
    }

    private async Task ExecuteInitiateCallAsync(string conversationId, string callType, Guid userId)
    {
        try
        {
            var session = await _mediator.Send(new InitiateCallCommand
            {
                ConversationId = conversationId,
                InitiatorId = userId,
                Type = callType
            });

            await Clients.Caller.SendAsync("call.initiated", session);
            await Clients.GroupExcept(ConversationGroup(conversationId), [Context.ConnectionId])
                .SendAsync("call.incoming", session);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi InitiateCall: {Msg}", ex.Message);
            await SendClientErrorAsync("initiate_failed", ex.Message);
        }
    }

    private async Task ExecuteRespondCallAsync(string callSessionId, bool accept, Guid userId)
    {
        try
        {
            var session = await _mediator.Send(new RespondCallCommand
            {
                CallSessionId = callSessionId,
                ResponderId = userId,
                Accept = accept
            });

            var eventName = accept ? "call.accepted" : "call.rejected";
            await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync(eventName, session);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi RespondCall: {Msg}", ex.Message);
            await SendClientErrorAsync("respond_failed", ex.Message);
        }
    }
}
