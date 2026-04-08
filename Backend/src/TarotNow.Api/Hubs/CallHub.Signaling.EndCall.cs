using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Call.Commands.EndCall;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Kết thúc cuộc gọi hiện tại.
    /// Luồng xử lý: xác thực user, kiểm tra rate-limit signaling, thực thi end-call và broadcast kết quả.
    /// </summary>
    /// <param name="callSessionId">Id call session cần kết thúc.</param>
    /// <param name="reason">Lý do kết thúc call.</param>
    public async Task EndCall(string callSessionId, string reason = "normal")
    {
        if (!TryGetUserGuid(out var userId))
        {
            // Không có user hợp lệ thì bỏ qua thao tác để tránh call từ kết nối không xác thực.
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_end",
            userId,
            TimeSpan.FromSeconds(2),
            "Thao tác quá nhanh.");

        if (!allowed)
        {
            // Rate-limit từ chối thì không thực thi end-call để bảo vệ hệ thống realtime.
            return;
        }

        await ExecuteEndCallAsync(callSessionId, reason, userId);
    }

    /// <summary>
    /// Thực thi nghiệp vụ kết thúc call và xử lý side effects.
    /// </summary>
    /// <param name="callSessionId">Id call session cần kết thúc.</param>
    /// <param name="reason">Lý do kết thúc call.</param>
    /// <param name="userId">Người thực hiện thao tác end call.</param>
    private async Task ExecuteEndCallAsync(string callSessionId, string reason, Guid userId)
    {
        try
        {
            // Gọi command nghiệp vụ để cập nhật trạng thái call trong persistence trước khi broadcast.
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
            // Lỗi nghiệp vụ/hạ tầng được phản hồi về caller để client hiển thị trạng thái thất bại.
            _logger.LogWarning(ex, "Lỗi EndCall: {Msg}", ex.Message);
            await SendClientErrorAsync("end_failed", ex.Message);
        }
    }

    /// <summary>
    /// Broadcast sự kiện call đã kết thúc cho toàn bộ participant trong conversation.
    /// </summary>
    /// <param name="session">Call session sau khi kết thúc.</param>
    /// <param name="reason">Lý do kết thúc cuộc gọi.</param>
    private async Task BroadcastEndedAsync(CallSessionDto session, string reason)
    {
        await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("call.ended", new
        {
            session,
            reason
        });
    }

    /// <summary>
    /// Thử ghi message log cho cuộc gọi vừa kết thúc.
    /// </summary>
    /// <param name="session">Call session đã kết thúc.</param>
    private async Task TryWriteCallLogAsync(CallSessionDto session)
    {
        try
        {
            // Sender log dùng initiator để giữ nhất quán với timeline cuộc gọi trong hội thoại.
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
            // Ghi log thất bại không được chặn luồng kết thúc call đã hoàn tất.
            _logger.LogWarning(ex, "Không tạo được dòng Log cho cuộc gọi {SessionId}: {Msg}", session.Id, ex.Message);
        }
    }
}
