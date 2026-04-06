/*
 * ===================================================================
 * FILE: EntitlementDailyResetJob.cs
 * NAMESPACE: TarotNow.Infrastructure.BackgroundJobs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Công Cụ Quét Dọn (Background Worker) Đổ Đầy Lại Các Rổ Trống Mỗi Ngày Lúc Nửa Đêm UTC.
 *   
 *   CHI TIẾT:
 *   - Nó càn quét bảng Bucket, tìm những rổ nào có BusinessDate < Hôm nay.
 *   - Lôi về và Reset UsedToday = 0, đồng thời Nâng BusinessDate lên bằng Hôm nay.
 *   - Chạy với Tần Suất Xoay Vòng Mỗi 15 Phút để Xử Lý Nhanh Nhất Lượng Rớt Lại (Nhà Thiết Kế Quartz/Hangfire Sẽ Cấu Hình Bật Timer Này).
 * ===================================================================
 */

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
                    // Shutdown phase
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Có Bug Tung Nồi Rớt Mạng Đứt Bóng Khi Reset Entitlement Quota.");
                }

                // Chờ Mỗi 15 Phút Kéo Lượt 1000 Chú Dư Date Cũ Vào Xóa Về 0.
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
        // Vì BackgroundService là Singleton Cắm Vĩnh Viễn Suốt Vòng Đời Sever, Không Thể Bắt Tiêm Cứng Transient/Scoped Của EF Core Vô Được (Chết Memory Lick).
        // Phải Phóng Factory ServiceScope Sinh Ra Rác Dùng Rùi Vứt Scope Đi Mới Không Vớ DbContext Lỗi Cũ Của Luồng Khác.
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();

        var todayUtc = DateOnly.FromDateTime(DateTime.UtcNow);
        
        // Càn Quét Lấy 1 Nhúm 1000 Chú Ra (Pagination)
        var staleBuckets = await repo.GetAllBucketsForResetAsync(todayUtc, cancellationToken);
        
        if (staleBuckets.Count == 0)
        {
            return; // Đã Tĩnh Lặng Trọng Hoàn Mỹ, Không Thừa Đứa Nào Cả.
        }

        int resetCount = 0;
        foreach (var bucket in staleBuckets)
        {
            bucket.ResetForNewDay(todayUtc);
            resetCount++;
        }

        // Táng Áp Lực Dữ Liệu SQL
        await repo.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Thành Công Reset Rổ Trái Cũ Chuyển Kéo Quota Lại Mốc 0 Cho {Count} Nhóm Giỏ Ngày {Date}", resetCount, todayUtc);
    }
}
