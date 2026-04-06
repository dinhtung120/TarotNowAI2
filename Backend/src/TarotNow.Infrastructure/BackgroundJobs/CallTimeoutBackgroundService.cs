using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence;
using MediatR;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class CallTimeoutBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CallTimeoutBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromSeconds(60);

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
        var chatPushService = scope.ServiceProvider.GetService<IChatPushService>();
        var staleCalls = await GetStaleCallsAsync(mongoContext, stoppingToken);

        foreach (var callDoc in staleCalls)
        {
            await HandleStaleCallAsync(callDoc, callRepo, mediator, chatPushService, stoppingToken);
        }
    }
}
