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
    private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessorWorker> _logger;

    /// <summary>
    /// Khởi tạo outbox processor worker.
    /// </summary>
    public OutboxProcessorWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessorWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
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

            await Task.Delay(PollInterval, stoppingToken);
        }

        _logger.LogInformation("Outbox processor stopped.");
    }

    private async Task ProcessBatchOnceAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IOutboxBatchProcessor>();
        await processor.ProcessOnceAsync(cancellationToken);
    }
}
