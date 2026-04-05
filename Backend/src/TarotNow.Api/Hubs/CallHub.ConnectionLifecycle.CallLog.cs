using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private async Task TryWriteDisconnectedCallLogAsync(CallSessionDto activeCall, DateTime endedAt)
    {
        try
        {
            var payload = BuildDisconnectedCallPayload(activeCall, endedAt);
            var message = await _mediator.Send(new SendMessageCommand
            {
                ConversationId = activeCall.ConversationId,
                SenderId = Guid.Parse(activeCall.InitiatorId),
                Type = ApiMessageTypes.CallLog,
                Content = string.Empty,
                CallPayload = payload
            });

            var group = ConversationGroup(activeCall.ConversationId);
            await _chatHubContext.Clients.Group(group).SendAsync("message.created", message);
            await _chatHubContext.Clients.Group(group).SendAsync("conversation.updated", new
            {
                conversationId = activeCall.ConversationId,
                type = "message_created"
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Không tạo được dòng Log khi disconnect {SessionId}", activeCall.Id);
        }
    }

    private static CallSessionDto BuildDisconnectedCallPayload(CallSessionDto activeCall, DateTime endedAt)
    {
        return new CallSessionDto
        {
            Id = activeCall.Id,
            ConversationId = activeCall.ConversationId,
            InitiatorId = activeCall.InitiatorId,
            Type = activeCall.Type,
            StartedAt = activeCall.StartedAt,
            EndedAt = endedAt,
            EndReason = "disconnected",
            DurationSeconds = activeCall.StartedAt.HasValue
                ? Math.Max(0, (int)(endedAt - activeCall.StartedAt.Value).TotalSeconds)
                : 0,
            CreatedAt = activeCall.CreatedAt,
            UpdatedAt = endedAt
        };
    }
}
