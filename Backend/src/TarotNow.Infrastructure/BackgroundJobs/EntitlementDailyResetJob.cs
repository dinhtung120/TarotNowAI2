

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs;

public class EntitlementDailyResetJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EntitlementDailyResetJob> _logger;

    public EntitlementDailyResetJob(IServiceProvider serviceProvider, ILogger<EntitlementDailyResetJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EntitlementDailyResetJob đang khởi động vòng lặp canh gác rổ Quyền lợi...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessResetBatchAsync(stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Có Bug Tung Nồi Rớt Mạng Đứt Bóng Khi Reset Entitlement Quota.");
                }

                
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("EntitlementDailyResetJob đang dừng rổ Quyền lợi...");
        }

        _logger.LogInformation("EntitlementDailyResetJob đã dừng.");
    }

    private async Task ProcessResetBatchAsync(CancellationToken cancellationToken)
    {
        
        
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();

        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        
        
        var staleBuckets = await repo.GetAllBucketsForResetAsync(todayUtc, cancellationToken);
        
        if (staleBuckets.Count == 0)
        {
            return; 
        }

        int resetCount = 0;
        foreach (var bucket in staleBuckets)
        {
            bucket.ResetForNewDay(todayUtc);
            resetCount++;
        }

        
        await repo.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Thành Công Reset Rổ Trái Cũ Chuyển Kéo Quota Lại Mốc 0 Cho {Count} Nhóm Giỏ Ngày {Date}", resetCount, todayUtc);
    }
}
