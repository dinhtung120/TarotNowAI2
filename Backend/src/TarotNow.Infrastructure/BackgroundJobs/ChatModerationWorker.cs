using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed partial class ChatModerationWorker : BackgroundService
{
    private readonly ChatModerationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptionsMonitor<ChatModerationOptions> _options;
    private readonly ILogger<ChatModerationWorker> _logger;

    public ChatModerationWorker(
        ChatModerationQueue queue,
        IServiceScopeFactory scopeFactory,
        IOptionsMonitor<ChatModerationOptions> options,
        ILogger<ChatModerationWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _options = options;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[ChatModerationWorker] Bắt đầu lắng nghe hàng đợi kiểm duyệt...");

        try
        {
            await foreach (var payload in _queue.ReadAllAsync(stoppingToken))
            {
                if (_options.CurrentValue.Enabled == false)
                {
                    continue;
                }

                try
                {
                    await ProcessPayloadAsync(payload, stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(
                        ex,
                        "[ChatModerationWorker] Failed to process payload. ConversationId={ConversationId}, MessageId={MessageId}",
                        payload.ConversationId,
                        payload.MessageId);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[ChatModerationWorker] Đang dừng công việc kiểm duyệt...");
        }

        _logger.LogInformation("[ChatModerationWorker] Tác vụ đã dừng.");
    }

    private async Task ProcessPayloadAsync(ChatModerationPayload payload, CancellationToken cancellationToken)
    {
        if (ShouldModerate(payload.Type) == false)
        {
            return;
        }

        var matchedKeyword = MatchKeyword(payload.Content, _options.CurrentValue.Keywords);
        if (matchedKeyword == null)
        {
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var reportRepo = scope.ServiceProvider.GetRequiredService<IReportRepository>();
        var chatMessageRepo = scope.ServiceProvider.GetRequiredService<IChatMessageRepository>();
        
        
        await chatMessageRepo.UpdateFlagAsync(payload.MessageId, true, cancellationToken);
        
        
        var report = BuildAutoModerationReport(payload, matchedKeyword);
        await reportRepo.AddAsync(report, cancellationToken);

        _logger.LogWarning(
            "[ChatModerationWorker] Auto-flag created. MessageId={MessageId}, Keyword={Keyword}",
            payload.MessageId,
            matchedKeyword);
    }
}
