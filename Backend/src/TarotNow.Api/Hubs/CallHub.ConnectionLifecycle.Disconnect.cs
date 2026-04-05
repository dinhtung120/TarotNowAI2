using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Call.Commands.UpdateCallStatus;
using TarotNow.Application.Features.Call.Queries.GetActiveCallsByConversationIds;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    private async Task HandleDisconnectedUserCleanupAsync(string userId)
    {
        try
        {
            var conversationIds = await GetActiveConversationIdsAsync(userId);
            if (conversationIds.Count == 0)
            {
                return;
            }

            var activeCalls = await _mediator.Send(new GetActiveCallsByConversationIdsQuery
            {
                ConversationIds = conversationIds
            });

            foreach (var activeCall in activeCalls)
            {
                await EndCallOnDisconnectAsync(activeCall);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi cleanup active call khi disconnect user {UserId}", userId);
        }
    }

    private async Task EndCallOnDisconnectAsync(CallSessionDto activeCall)
    {
        var endedAt = DateTime.UtcNow;
        var updated = await _mediator.Send(new UpdateCallStatusCommand
        {
            CallSessionId = activeCall.Id,
            NewStatus = "ended",
            EndedAt = endedAt,
            EndReason = "disconnected",
            ExpectedPreviousStatus = activeCall.Status.ToString().ToLowerInvariant()
        });

        if (!updated)
        {
            return;
        }

        await BroadcastDisconnectedEndedEventAsync(activeCall);
        await TryWriteDisconnectedCallLogAsync(activeCall, endedAt);
    }

    private async Task BroadcastDisconnectedEndedEventAsync(CallSessionDto activeCall)
    {
        await Clients.Group(ConversationGroup(activeCall.ConversationId)).SendAsync("call.ended", new
        {
            session = new { id = activeCall.Id, conversationId = activeCall.ConversationId },
            reason = "disconnected"
        });
    }
}
