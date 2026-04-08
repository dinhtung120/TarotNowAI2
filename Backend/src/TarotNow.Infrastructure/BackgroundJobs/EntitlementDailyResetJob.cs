

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.BackgroundJobs;

// Job nền reset quota entitlement theo ngày nghiệp vụ để đảm bảo hạn mức ngày mới được làm mới đúng thời điểm.
public class EntitlementDailyResetJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EntitlementDailyResetJob> _logger;

    /// <summary>
    /// Khởi tạo job reset entitlement quota.
    /// Luồng xử lý: nhận service provider để tạo scope theo từng batch và logger cho vận hành.
    /// </summary>
    public EntitlementDailyResetJob(IServiceProvider serviceProvider, ILogger<EntitlementDailyResetJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp nền định kỳ chạy reset quota.
    /// Luồng xử lý: chạy ProcessResetBatchAsync, bắt lỗi cục bộ để job không dừng, ngủ 15 phút giữa các lần quét.
    /// </summary>
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
                    // Bỏ qua lỗi dispose khi host đang shutdown có chủ đích.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Có Bug Tung Nồi Rớt Mạng Đứt Bóng Khi Reset Entitlement Quota.");
                    // Bắt lỗi để vòng lặp tiếp tục, tránh mất cơ chế reset quota toàn hệ thống.
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

    /// <summary>
    /// Xử lý một batch reset quota theo ngày UTC hiện tại.
    /// Luồng xử lý: lấy bucket stale cần reset, reset từng bucket, rồi commit SaveChanges một lần.
    /// </summary>
    private async Task ProcessResetBatchAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();

        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        var staleBuckets = await repo.GetAllBucketsForResetAsync(todayUtc, cancellationToken);

        if (staleBuckets.Count == 0)
        {
            // Không có bucket cũ cần reset thì dừng batch sớm để tiết kiệm tài nguyên.
            return;
        }

        int resetCount = 0;
        foreach (var bucket in staleBuckets)
        {
            bucket.ResetForNewDay(todayUtc);
            resetCount++;
            // Áp reset theo từng bucket để đưa UsedToday về 0 cho ngày mới.
        }

        await repo.SaveChangesAsync(cancellationToken);
        // Commit một lần sau batch để giảm số lượng transaction ghi database.

        _logger.LogInformation("Thành Công Reset Rổ Trái Cũ Chuyển Kéo Quota Lại Mốc 0 Cho {Count} Nhóm Giỏ Ngày {Date}", resetCount, todayUtc);
    }
}
