using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Call.Commands.UpdateCallStatus;
using TarotNow.Application.Features.Call.Queries.GetActiveCallsByConversationIds;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Cleanup cuộc gọi active khi user disconnect hoàn toàn.
    /// Luồng xử lý: lấy conversation active, truy vấn call đang mở, kết thúc từng call.
    /// </summary>
    /// <param name="userId">User id bị ngắt kết nối.</param>
    private async Task HandleDisconnectedUserCleanupAsync(string userId)
    {
        try
        {
            var conversationIds = await GetActiveConversationIdsAsync(userId);
            if (conversationIds.Count == 0)
            {
                // Không còn conversation active thì không cần cleanup call session.
                return;
            }

            var activeCalls = await _mediator.Send(new GetActiveCallsByConversationIdsQuery
            {
                ConversationIds = conversationIds
            });

            foreach (var activeCall in activeCalls)
            {
                // Kết thúc từng call theo cùng một luồng để bảo toàn dữ liệu call log.
                await EndCallOnDisconnectAsync(activeCall);
            }
        }
        catch (Exception ex)
        {
            // Cleanup lỗi không được ném ngược để tránh làm đứt luồng disconnect của SignalR.
            _logger.LogWarning(ex, "Lỗi cleanup active call khi disconnect user {UserId}", userId);
        }
    }

    /// <summary>
    /// Kết thúc một call session do ngắt kết nối.
    /// Luồng xử lý: cập nhật trạng thái call, broadcast call.ended, ghi call log hệ thống.
    /// </summary>
    /// <param name="activeCall">Call session đang active cần kết thúc.</param>
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
            // Trạng thái đã đổi bởi luồng khác thì bỏ qua để tránh thao tác lặp.
            return;
        }

        await BroadcastDisconnectedEndedEventAsync(activeCall);
        await TryWriteDisconnectedCallLogAsync(activeCall, endedAt);
    }

    /// <summary>
    /// Broadcast sự kiện kết thúc cuộc gọi do disconnect tới group conversation.
    /// </summary>
    /// <param name="activeCall">Call session vừa được kết thúc.</param>
    private async Task BroadcastDisconnectedEndedEventAsync(CallSessionDto activeCall)
    {
        await Clients.Group(ConversationGroup(activeCall.ConversationId)).SendAsync("call.ended", new
        {
            session = new { id = activeCall.Id, conversationId = activeCall.ConversationId },
            reason = "disconnected"
        });
    }
}
