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
    private async Task<List<CallSessionDocument>> GetStaleCallsAsync(MongoDbContext mongoContext, CancellationToken cancellationToken)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(_timeoutThreshold);
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.Status, "requested")
            & Builders<CallSessionDocument>.Filter.Lt(x => x.CreatedAt, cutoffTime);
        return await mongoContext.CallSessions.Find(filter).ToListAsync(cancellationToken);
    }

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
            return;
        }

        await TryPublishTimeoutCallLogAsync(callDoc, mediator, chatPushService, cancellationToken);
    }

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
                return;
            }

            await chatPushService.BroadcastMessageAsync(callDoc.ConversationId, messageDto, cancellationToken);
            await chatPushService.BroadcastConversationUpdatedAsync(callDoc.ConversationId, "message_created", cancellationToken);
            await chatPushService.BroadcastCallEndedAsync(callDoc.ConversationId, dto, "timeout_server", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi tạo log cho cuộc gọi timeout {Id}", callDoc.Id);
        }
    }

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
