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

    /// <summary>
    /// Khởi tạo background service quét timeout cuộc gọi.
    /// Luồng xử lý: nhận scope factory để resolve scoped dependencies theo từng vòng quét và logger vận hành.
    /// </summary>
    public CallTimeoutBackgroundService(IServiceScopeFactory scopeFactory, ILogger<CallTimeoutBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp chạy nền định kỳ để phát hiện và xử lý cuộc gọi treo.
    /// Luồng xử lý: lặp theo _checkInterval, gọi ProcessTimeoutsAsync, bắt lỗi cục bộ để service tiếp tục chạy ổn định.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[CallTimeoutService] Bắt đầu tác vụ chạy nền dọn dẹp cuộc gọi bị treo.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessTimeoutsAsync(stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Bỏ qua lỗi dispose khi ứng dụng đang shutdown có chủ đích.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[CallTimeoutService] Lỗi trong quá trình quét timeout cuộc gọi.");
                    // Bắt lỗi vòng lặp để job không chết hẳn do lỗi một lần quét.
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[CallTimeoutService] Dịch vụ đang tắt dần...");
        }

        _logger.LogInformation("[CallTimeoutService] Tác vụ đã dừng.");
    }

    /// <summary>
    /// Xử lý một vòng quét timeout: lấy stale call và xử lý từng cuộc gọi.
    /// Luồng xử lý: tạo scope mới, resolve dependencies scoped, truy vấn stale calls rồi tuần tự handle từng call.
    /// </summary>
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
            // Xử lý tuần tự để giảm rủi ro race khi nhiều call cùng conversation.
        }
    }
}
