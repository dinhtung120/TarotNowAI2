using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed class CallV2TimeoutBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CallV2TimeoutBackgroundService> _logger;
    private readonly TimeSpan _sweepInterval;

    public CallV2TimeoutBackgroundService(
        IServiceScopeFactory scopeFactory,
        IOptions<CallV2Options> options,
        ILogger<CallV2TimeoutBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        var seconds = Math.Max(5, options.Value.TimeoutSweepIntervalSeconds);
        _sweepInterval = TimeSpan.FromSeconds(seconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var maintenanceService = scope.ServiceProvider.GetRequiredService<ICallV2MaintenanceService>();
                await maintenanceService.ProcessTimeoutsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CallV2 timeout worker gặp lỗi trong quá trình quét.");
            }

            await Task.Delay(_sweepInterval, stoppingToken);
        }
    }
}
