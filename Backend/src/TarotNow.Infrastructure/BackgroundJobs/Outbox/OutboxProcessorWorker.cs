using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

/// <summary>
/// Background worker quét transactional outbox và dispatch domain events qua MediatR.
/// </summary>
public sealed class OutboxProcessorWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessorWorker> _logger;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo outbox processor worker.
    /// </summary>
    public OutboxProcessorWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessorWorker> logger,
        ISystemConfigSettings systemConfigSettings)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started.");
        var fastLoopCooldown = TimeSpan.FromMilliseconds(50);

        while (stoppingToken.IsCancellationRequested == false)
        {
            var processedCount = 0;
            try
            {
                processedCount = await ProcessBatchOnceAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Outbox processor loop failed.");
            }

            if (processedCount > 0)
            {
                await Task.Delay(fastLoopCooldown, stoppingToken);
                continue;
            }

            await Task.Delay(ResolveIdlePollInterval(), stoppingToken);
        }

        _logger.LogInformation("Outbox processor stopped.");
    }

    private async Task<int> ProcessBatchOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxBatchProcessor>();
        return await processor.ProcessOnceAsync(cancellationToken);
    }

    private TimeSpan ResolveIdlePollInterval()
    {
        var seconds = _systemConfigSettings.OperationalOutboxPollIntervalSeconds;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, 1, 300));
    }
}
