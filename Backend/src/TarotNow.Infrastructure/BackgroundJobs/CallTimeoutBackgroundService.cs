using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Application.Features.Chat.Commands.SendMessage;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence;
using TarotNow.Infrastructure.Persistence.MongoDocuments;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs; // We can't reference TarotNow.Api from Infrastructure!!

namespace TarotNow.Infrastructure.BackgroundJobs;

public class CallTimeoutBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CallTimeoutBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromSeconds(90);

    public CallTimeoutBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CallTimeoutBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[CallTimeoutService] Bắt đầu tác vụ chạy nền dọn dẹp cuộc gọi bị treo.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessTimeoutsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CallTimeoutService] Lỗi trong quá trình quét timeout cuộc gọi.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task ProcessTimeoutsAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
        var callRepo = scope.ServiceProvider.GetRequiredService<ICallSessionRepository>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Tìm các cuộc gọi Requested mà bị treo quá 90s
        var cutoffTime = DateTime.UtcNow.Subtract(_timeoutThreshold);
        var filter = Builders<CallSessionDocument>.Filter.Eq(x => x.Status, "requested") &
                     Builders<CallSessionDocument>.Filter.Lt(x => x.CreatedAt, cutoffTime);

        var staleCalls = await mongoContext.CallSessions.Find(filter).ToListAsync(stoppingToken);

        foreach (var callDoc in staleCalls)
        {
            _logger.LogInformation("[CallTimeoutService] Phát hiện cuộc gọi treo: {Id}. Tiến hành Ended due to timeout.", callDoc.Id);

            var updated = await callRepo.UpdateStatusAsync(
                callDoc.Id,
                CallSessionStatus.Ended,
                startedAt: null,
                endedAt: DateTime.UtcNow,
                endReason: "timeout_server",
                expectedPreviousStatus: CallSessionStatus.Requested,
                ct: stoppingToken);

            if (updated)
            {
                // Push CallLog using SendMessage
                try
                {
                    var dto = new CallSessionDto
                    {
                        Id = callDoc.Id,
                        ConversationId = callDoc.ConversationId,
                        InitiatorId = callDoc.InitiatorId,
                        Type = callDoc.Type == "video" ? CallType.Video : CallType.Audio,
                        Status = CallSessionStatus.Ended,
                        EndReason = "timeout_server",
                        DurationSeconds = 0
                    };

                    var logCmd = new SendMessageCommand
                    {
                        ConversationId = callDoc.ConversationId,
                        SenderId = Guid.Parse(callDoc.InitiatorId),
                        Type = TarotNow.Domain.Enums.ChatMessageType.CallLog,
                        Content = string.Empty,
                        CallPayload = dto
                    };

                    var messageDto = await mediator.Send(logCmd, stoppingToken);

                    var chatPushService = scope.ServiceProvider.GetService<IChatPushService>();
                    if (chatPushService != null)
                    {
                        await chatPushService.BroadcastMessageAsync(callDoc.ConversationId, messageDto, stoppingToken);
                        await chatPushService.BroadcastConversationUpdatedAsync(callDoc.ConversationId, "message_created", stoppingToken);
                        await chatPushService.BroadcastCallEndedAsync(callDoc.ConversationId, dto, "timeout_server", stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi tạo log cho cuộc gọi timeout {Id}", callDoc.Id);
                }
            }
        }
    }
}
