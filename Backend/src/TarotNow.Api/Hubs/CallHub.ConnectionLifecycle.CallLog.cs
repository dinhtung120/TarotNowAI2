using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;

namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    /// <summary>
    /// Thử ghi message log cuộc gọi khi call kết thúc do disconnect.
    /// Luồng xử lý: dựng payload call log, tạo message hệ thống, broadcast message.created và conversation.updated.
    /// </summary>
    /// <param name="activeCall">Call session vừa bị ngắt.</param>
    /// <param name="endedAt">Thời điểm call bị kết thúc.</param>
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
            // Lỗi ghi call log không được chặn luồng cleanup disconnect chính.
            _logger.LogWarning(ex, "Không tạo được dòng Log khi disconnect {SessionId}", activeCall.Id);
        }
    }

    /// <summary>
    /// Dựng payload call log tiêu chuẩn cho nhánh disconnect.
    /// </summary>
    /// <param name="activeCall">Call session gốc trước khi kết thúc.</param>
    /// <param name="endedAt">Thời điểm kết thúc được ghi nhận.</param>
    /// <returns>Payload call session hoàn chỉnh để lưu/broadcast.</returns>
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
                // Duration được chặn >=0 để tránh dữ liệu âm khi lệch thời gian hiếm gặp.
                ? Math.Max(0, (int)(endedAt - activeCall.StartedAt.Value).TotalSeconds)
                : 0,
            CreatedAt = activeCall.CreatedAt,
            UpdatedAt = endedAt
        };
    }
}
