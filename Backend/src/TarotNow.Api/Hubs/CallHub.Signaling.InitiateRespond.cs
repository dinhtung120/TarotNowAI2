using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Call.Commands.InitiateCall;
using TarotNow.Application.Features.Call.Commands.RespondCall;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Khởi tạo cuộc gọi mới trong conversation.
    /// Luồng xử lý: xác thực user, kiểm tra rate-limit, gọi nghiệp vụ initiate và broadcast call.incoming.
    /// </summary>
    /// <param name="conversationId">Id hội thoại cần gọi.</param>
    /// <param name="callType">Loại cuộc gọi (audio/video...).</param>
    public async Task InitiateCall(string conversationId, string callType)
    {
        if (!TryGetUserGuid(out var userId))
        {
            // Bỏ qua thao tác khi user id không hợp lệ.
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_initiate",
            userId,
            TimeSpan.FromSeconds(5),
            "Bạn đang thao tác quá nhanh. Vui lòng chờ 5 giây rồi thử lại.");

        if (!allowed)
        {
            // Rate-limit chặn thao tác initiate để tránh spam cuộc gọi.
            return;
        }

        await ExecuteInitiateCallAsync(conversationId, callType, userId);
    }

    /// <summary>
    /// Phản hồi cuộc gọi đến (accept/reject).
    /// Luồng xử lý: xác thực user, kiểm tra rate-limit, gọi nghiệp vụ respond và broadcast kết quả.
    /// </summary>
    /// <param name="callSessionId">Id call session cần phản hồi.</param>
    /// <param name="accept">Quyết định chấp nhận hay từ chối.</param>
    public async Task RespondCall(string callSessionId, bool accept)
    {
        if (!TryGetUserGuid(out var userId))
        {
            // Bỏ qua phản hồi khi không parse được user id.
            return;
        }

        var allowed = await TryAcquireSignalRateLimitAsync(
            "call_respond",
            userId,
            TimeSpan.FromSeconds(2),
            "Thao tác quá nhanh.");

        if (!allowed)
        {
            // Rate-limit chặn thao tác respond quá nhanh để giảm xung đột trạng thái.
            return;
        }

        await ExecuteRespondCallAsync(callSessionId, accept, userId);
    }

    /// <summary>
    /// Thực thi nghiệp vụ khởi tạo cuộc gọi và broadcast sự kiện liên quan.
    /// </summary>
    /// <param name="conversationId">Id conversation đích.</param>
    /// <param name="callType">Loại cuộc gọi.</param>
    /// <param name="userId">Người khởi tạo cuộc gọi.</param>
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
            // Gửi sự kiện incoming cho các kết nối khác trong cùng conversation (loại trừ caller).
            await Clients.GroupExcept(ConversationGroup(conversationId), [Context.ConnectionId])
                .SendAsync("call.incoming", session);
        }
        catch (Exception ex)
        {
            // Trả lỗi realtime cho caller để UI hiển thị thất bại initiate.
            _logger.LogWarning(ex, "Lỗi InitiateCall: {Msg}", ex.Message);
            await SendClientErrorAsync("initiate_failed", ex.Message);
        }
    }

    /// <summary>
    /// Thực thi nghiệp vụ phản hồi cuộc gọi.
    /// </summary>
    /// <param name="callSessionId">Id call session cần phản hồi.</param>
    /// <param name="accept">Quyết định accept/reject.</param>
    /// <param name="userId">Người phản hồi cuộc gọi.</param>
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

            // Chọn event name theo quyết định để client xử lý trạng thái cuộc gọi đúng nhánh.
            var eventName = accept ? "call.accepted" : "call.rejected";
            await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync(eventName, session);
        }
        catch (Exception ex)
        {
            // Bọc lỗi và phản hồi thống nhất để caller xử lý UX lỗi realtime.
            _logger.LogWarning(ex, "Lỗi RespondCall: {Msg}", ex.Message);
            await SendClientErrorAsync("respond_failed", ex.Message);
        }
    }
}
