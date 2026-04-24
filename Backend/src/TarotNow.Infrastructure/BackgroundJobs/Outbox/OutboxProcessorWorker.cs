using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs.Outbox;

/// <summary>
/// Background worker quét transactional outbox và dispatch domain events qua MediatR.
/// </summary>
public sealed class OutboxProcessorWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessorWorker> _logger;
    private readonly TimeSpan _pollInterval;

    /// <summary>
    /// Khởi tạo outbox processor worker.
    /// </summary>
    public OutboxProcessorWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessorWorker> logger,
        IOptions<OutboxOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _pollInterval = ResolvePollInterval(options.Value);
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started.");

        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                await ProcessBatchOnceAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Outbox processor loop failed.");
            }

            await Task.Delay(_pollInterval, stoppingToken);
        }

        _logger.LogInformation("Outbox processor stopped.");
    }

    private async Task ProcessBatchOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxBatchProcessor>();
        await processor.ProcessOnceAsync(cancellationToken);
    }

    private static TimeSpan ResolvePollInterval(OutboxOptions options)
    {
        var seconds = options.PollIntervalSeconds <= 0 ? 5 : options.PollIntervalSeconds;
        return TimeSpan.FromSeconds(Math.Clamp(seconds, 1, 300));
    }
}
