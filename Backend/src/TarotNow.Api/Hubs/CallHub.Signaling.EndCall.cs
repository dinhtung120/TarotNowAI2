using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Call.Commands.EndCall;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
        public async Task EndCall(string callSessionId, string reason = "normal")
    {
        if (!TryGetUserGuid(out var userId))
        {
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_end",
            userId,
            TimeSpan.FromSeconds(2),
            "Thao tác quá nhanh.");

        if (!allowed)
        {
            return;
        }

        await ExecuteEndCallAsync(callSessionId, reason, userId);
    }

    private async Task ExecuteEndCallAsync(string callSessionId, string reason, Guid userId)
    {
        try
        {
            var session = await _mediator.Send(new EndCallCommand
            {
                CallSessionId = callSessionId,
                UserId = userId,
                Reason = reason
            });

            await BroadcastEndedAsync(session, reason);
            await TryWriteCallLogAsync(session);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi EndCall: {Msg}", ex.Message);
            await SendClientErrorAsync("end_failed", ex.Message);
        }
    }

    private async Task BroadcastEndedAsync(CallSessionDto session, string reason)
    {
        await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("call.ended", new
        {
            session,
            reason
        });
    }

    private async Task TryWriteCallLogAsync(CallSessionDto session)
    {
        try
        {
            var senderId = Guid.Parse(session.InitiatorId);
            var message = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = session.ConversationId,
                SenderId = senderId,
                Type = ApiMessageTypes.CallLog,
                Content = string.Empty,
                CallPayload = session
            });

            var group = ConversationGroup(session.ConversationId);
            await _chatHubContext.Clients.Group(group).SendAsync("message.created", message);
            await _chatHubContext.Clients.Group(group).SendAsync("conversation.updated", new
            {
                conversationId = session.ConversationId,
                type = "message_created"
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không tạo được dòng Log cho cuộc gọi {SessionId}: {Msg}", session.Id, ex.Message);
        }
    }
}
