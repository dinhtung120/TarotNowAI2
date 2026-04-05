using Microsoft.AspNetCore.SignalR;
using TarotNow.Application.Features.Call.Commands.EndCall;
using TarotNow.Application.Features.Call.Commands.InitiateCall;
using TarotNow.Application.Features.Call.Commands.RespondCall;
using TarotNow.Domain.Enums;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Bắt đầu một cuộc gọi mới vào Conversation.
    /// Gửi tới counterpart sự kiện ngõ gọi "call.incoming".
    /// Đồng thời GỬI LẠI session cho caller qua "call.initiated" để caller biết session ID.
    /// </summary>
    public async Task InitiateCall(string conversationId, string callType)
    {
        if (!TryGetUserGuid(out var userId)) return;

        // FIX #18 (Abuse Protection): Chống Spam DDoS InitiateCall.
        // Cùng một User không được phép gọi InitiateCall (kể cả với conversation khác nhau) nhanh hơn 5 giây 1 lần.
        var rateLimitKey = $"ratelimit:call_initiate:{userId}";
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, TimeSpan.FromSeconds(5));
        if (!isAllowed)
        {
            await SendClientErrorAsync("too_many_requests", "Bạn đang thao tác quá nhanh. Vui lòng chờ 5 giây rồi thử lại.");
            return;
        }

        try
        {
            var type = callType.Equals("video", StringComparison.OrdinalIgnoreCase) 
                ? CallType.Video 
                : CallType.Audio;

            var command = new InitiateCallCommand
            {
                ConversationId = conversationId,
                InitiatorId = userId,
                Type = type
            };

            var session = await _mediator.Send(command);

            // FIX #4: Gửi session trả lại cho CALLER để caller biết session.id
            // Nếu không gửi, caller sẽ có session = null và không thể Cancel/End cuộc gọi.
            await Clients.Caller.SendAsync("call.initiated", session);

            // Gửi sự kiện incoming call tới TẤT CẢ client ngoại trừ Caller.
            await Clients.GroupExcept(ConversationGroup(conversationId), Context.ConnectionId)
                         .SendAsync("call.incoming", session);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi InitiateCall: {Msg}", ex.Message);
            await SendClientErrorAsync("initiate_failed", ex.Message);
        }
    }

    /// <summary>
    /// Phản hồi đồng ý hay từ chối từ người nhận (Callee).
    /// </summary>
    public async Task RespondCall(string callSessionId, bool accept)
    {
        if (!TryGetUserGuid(out var userId)) return;

        // FIX #6: Cùng một User không được phản hồi liên tiếp quá nhanh.
        var rateLimitKey = $"ratelimit:call_respond:{userId}";
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, TimeSpan.FromSeconds(2));
        if (!isAllowed)
        {
            await SendClientErrorAsync("too_many_requests", "Thao tác quá nhanh.");
            return;
        }

        try
        {
            var command = new RespondCallCommand
            {
                CallSessionId = callSessionId,
                ResponderId = userId,
                Accept = accept
            };

            var session = await _mediator.Send(command);

            // Broadcast kết quả Respond tới cả Caller và Callee
            var evtName = accept ? "call.accepted" : "call.rejected";
            await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync(evtName, session);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi RespondCall: {Msg}", ex.Message);
            await SendClientErrorAsync("respond_failed", ex.Message);
        }
    }

    /// <summary>
    /// Kết thúc cuộc gọi hoặc huỷ (Cancel, Timeout, End).
    /// </summary>
    public async Task EndCall(string callSessionId, string reason = "normal")
    {
        if (!TryGetUserGuid(out var userId)) return;

        // FIX #6: Cùng một User không được EndCall liên tiếp quá nhanh.
        var rateLimitKey = $"ratelimit:call_end:{userId}";
        var isAllowed = await _cacheService.CheckRateLimitAsync(rateLimitKey, TimeSpan.FromSeconds(2));
        if (!isAllowed)
        {
            await SendClientErrorAsync("too_many_requests", "Thao tác quá nhanh.");
            return;
        }

        try
        {
            var command = new EndCallCommand
            {
                CallSessionId = callSessionId,
                UserId = userId,
                Reason = reason
            };

            var session = await _mediator.Send(command);

            await Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("call.ended", new
            {
                session,
                reason
            });

            // Sau khi phát tín hiệu EndCall, ghi thêm một dòng Log vào cuộc trò chuyện
            try
            {
                // FIX #5 & #10: Không dùng JSON Serialize thô nữa, đổi sang DTO mạnh.
                var logCmd = new TarotNow.Application.Features.Chat.Commands.SendMessage.SendMessageCommand
                {
                    ConversationId = session.ConversationId,
                    SenderId = Guid.Parse(session.InitiatorId),
                    Type = ChatMessageType.CallLog,
                    Content = string.Empty, // Bỏ trường lưu Text
                    CallPayload = session   // Dùng Payload rõ ràng.
                };

                // Gọi tới hệ thống Message để ghi Log mà lại tự động bỏ qua tính năng trừ tiền theo luồng đã lập trình
                var messageDto = await _mediator.Send(logCmd);

                // Giống như việc chat bình thường, ta broadcast message tới cùng nhóm trò chuyện để frontend ChatHub Load ngay
                await _chatHubContext.Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("message.created", messageDto);
                
                // Đồng thời báo hiệu conversation updated nếu cần
                await _chatHubContext.Clients.Group(ConversationGroup(session.ConversationId)).SendAsync("conversation.updated", new
                {
                    conversationId = session.ConversationId,
                    type = "message_created"
                });
            }
            catch (Exception chatEx)
            {
                _logger.LogWarning(chatEx, "Không tạo được dòng Log cho cuộc gọi {SessionId}: {Msg}", session.Id, chatEx.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Lỗi EndCall: {Msg}", ex.Message);
            await SendClientErrorAsync("end_failed", ex.Message);
        }
    }
}
