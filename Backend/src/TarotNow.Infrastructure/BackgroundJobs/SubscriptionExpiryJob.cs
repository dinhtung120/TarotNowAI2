/*
 * ===================================================================
 * FILE: SubscriptionExpiryJob.cs
 * NAMESPACE: TarotNow.Infrastructure.BackgroundJobs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thằng Giám Thị Canh Giờ Trảm Hết Hồ Sơ Quá Hạn Cụt Kì Sống (Background Service Cắt Tiết Gói Active Hết Hạn).
 *   
 *   CHI TIẾT:
 *   - Lấy `Subscriptions WHERE Status="active" AND EndDate <= NOW`.
 *   - Sút vào trạng thái `Expired`.
 *   - Bắn Domain Event báo còi báo thức ra để Handler ngoài kia Tắt Cache Người Dùng Báo Đỏ Mobile Nạp Lại Thêm Đi.
 * ===================================================================
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public class SubscriptionExpiryJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SubscriptionExpiryJob> _logger;

    public SubscriptionExpiryJob(IServiceProvider serviceProvider, ILogger<SubscriptionExpiryJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SubscriptionExpiryJob bắt đầu công việc canh chừng dọn sạch bọn xài lố giờ ...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpirationsAsync(stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Graceful exit
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Lỗi Nghiêm Trọng Xảy Ra Khi Lấy Đồ Quá Hạn Máy Chém.");
                }

                // Dạo Quanh Phố Trảm 1 Tiếng Một Lần Đi Tuần Nhé.
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("SubscriptionExpiryJob đang dừng do yêu cầu từ hệ thống.");
        }

        _logger.LogInformation("SubscriptionExpiryJob đã dừng.");
    }

    private async Task ProcessExpirationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        var domainEventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

        var cutoff = DateTime.UtcNow;

        var expiredSubs = await repo.GetExpiredSubscriptionsToProcessAsync(cutoff, cancellationToken);
        if (expiredSubs.Count == 0) return;

        foreach (var sub in expiredSubs)
        {
            try
            {
                // Đóng Đập Xả (Kéo Lại Cờ Enum Expired).
                sub.Expire();
                
                // Mở Loa Gào Ra Domain Events Báo Mọi Người Xông Vào Xóa Cache Đít UI Chữ Vàng Cầm Đỏ.
                await domainEventPublisher.PublishAsync(new SubscriptionExpiredDomainEvent(sub.UserId, sub.Id), cancellationToken);
            }
            catch (Exception itemEx)
            {
                _logger.LogError(itemEx, "Lôi Ngang Mắc Xương Tại Gói {SubId} Của Khách {UserId}. Tạm Qua Trảm Đứa Sau Mất Lệ.", sub.Id, sub.UserId);
            }
        }

        // Dội Ụp Lưu Trút Đống Nhãn Active Sang Expired.
        await repo.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Tìm Vào Sát Cửa Tử {Count} Hồ Sơ Đăng Ký Đã Hết Giờ Linh Thiêng Phù Hộ (Gắn Expired).", expiredSubs.Count);
    }
}
