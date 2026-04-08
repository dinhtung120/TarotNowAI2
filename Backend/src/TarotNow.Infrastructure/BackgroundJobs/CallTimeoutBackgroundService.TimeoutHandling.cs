using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class CallTimeoutBackgroundService
{
    /// <summary>
    /// Lấy danh sách cuộc gọi requested đã quá thời gian chờ để xử lý timeout.
    /// Luồng xử lý: tính cutoff theo _timeoutThreshold, lọc trên Mongo theo status + createdAt, trả danh sách stale call.
    /// </summary>
    private async Task<List<CallSessionDocument>> GetStaleCallsAsync(MongoDbContext mongoContext, CancellationToken cancellationToken)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(_timeoutThreshold);
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.Status, "requested")
            & Builders<CallSessionDocument>.Filter.Lt(x => x.CreatedAt, cutoffTime);
        // Chỉ chọn call requested đã quá hạn để tránh kết thúc nhầm các phiên đang xử lý hợp lệ.
        return await mongoContext.CallSessions.Find(filter).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Xử lý một cuộc gọi treo: cập nhật trạng thái ended và phát log hệ thống liên quan.
    /// Luồng xử lý: thử update trạng thái có điều kiện expectedPreviousStatus, nếu thành công thì publish call log timeout.
    /// </summary>
    private async Task HandleStaleCallAsync(
        CallSessionDocument callDoc,
        ICallSessionRepository callRepo,
        IMediator mediator,
        IChatPushService? chatPushService,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("[CallTimeoutService] Phát hiện cuộc gọi treo: {Id}.", callDoc.Id);
        var updated = await callRepo.UpdateStatusAsync(
            callDoc.Id,
            CallSessionStatus.Ended,
            startedAt: null,
            endedAt: DateTime.UtcNow,
            endReason: "timeout_server",
            expectedPreviousStatus: CallSessionStatus.Requested,
            ct: cancellationToken);
        if (!updated)
        {
            // Edge case: call đã được xử lý bởi luồng khác, bỏ qua để tránh phát event trùng.
            return;
        }

        await TryPublishTimeoutCallLogAsync(callDoc, mediator, chatPushService, cancellationToken);
    }

    /// <summary>
    /// Tạo và phát message log timeout cuộc gọi để đồng bộ UI chat và sự kiện kết thúc call.
    /// Luồng xử lý: gửi SendMessage command tạo call log, sau đó broadcast các sự kiện realtime nếu có push service.
    /// </summary>
    private async Task TryPublishTimeoutCallLogAsync(
        CallSessionDocument callDoc,
        IMediator mediator,
        IChatPushService? chatPushService,
        CancellationToken cancellationToken)
    {
        try
        {
            var dto = BuildTimeoutCallDto(callDoc);
            var messageDto = await mediator.Send(new SendMessageCommand
            {
                ConversationId = callDoc.ConversationId,
                SenderId = Guid.Parse(callDoc.InitiatorId),
                Type = ChatMessageType.CallLog,
                Content = string.Empty,
                CallPayload = dto
            }, cancellationToken);

            if (chatPushService == null)
            {
                // Edge case: không có push service thì chỉ lưu log message, bỏ qua broadcast realtime.
                return;
            }

            await chatPushService.BroadcastMessageAsync(callDoc.ConversationId, messageDto, cancellationToken);
            await chatPushService.BroadcastConversationUpdatedAsync(callDoc.ConversationId, "message_created", cancellationToken);
            await chatPushService.BroadcastCallEndedAsync(callDoc.ConversationId, dto, "timeout_server", cancellationToken);
            // Đồng bộ đủ 3 kênh để client cập nhật danh sách chat và trạng thái call ngay lập tức.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi tạo log cho cuộc gọi timeout {Id}", callDoc.Id);
            // Bắt lỗi cục bộ để một call lỗi không làm dừng toàn bộ vòng quét timeout.
        }
    }

    /// <summary>
    /// Dựng DTO call ended cho trường hợp timeout server.
    /// Luồng xử lý: map dữ liệu call document sang CallSessionDto chuẩn để dùng cho message và broadcast.
    /// </summary>
    private static CallSessionDto BuildTimeoutCallDto(CallSessionDocument callDoc)
    {
        return new CallSessionDto
        {
            Id = callDoc.Id,
            ConversationId = callDoc.ConversationId,
            InitiatorId = callDoc.InitiatorId,
            Type = callDoc.Type == "video" ? CallType.Video : CallType.Audio,
            Status = CallSessionStatus.Ended,
            EndReason = "timeout_server",
            DurationSeconds = 0
        };
    }
}
