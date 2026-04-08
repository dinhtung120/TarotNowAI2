

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

// Job nền đánh dấu subscription hết hạn theo thời gian thực tế.
public class SubscriptionExpiryJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SubscriptionExpiryJob> _logger;

    /// <summary>
    /// Khởi tạo job xử lý hết hạn subscription.
    /// Luồng xử lý: nhận service provider để tạo scope repository và logger vận hành.
    /// </summary>
    public SubscriptionExpiryJob(IServiceProvider serviceProvider, ILogger<SubscriptionExpiryJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp nền quét subscription hết hạn theo chu kỳ 1 giờ.
    /// Luồng xử lý: gọi ProcessExpirationsAsync, bắt lỗi cục bộ để job tiếp tục và delay định kỳ.
    /// </summary>
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
                    // Bỏ qua lỗi dispose khi host đang dừng có chủ đích.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Lỗi Nghiêm Trọng Xảy Ra Khi Lấy Đồ Quá Hạn Máy Chém.");
                    // Bắt lỗi để vòng lặp tiếp tục ở chu kỳ kế tiếp.
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("SubscriptionExpiryJob đang dừng do yêu cầu từ hệ thống.");
        }

        _logger.LogInformation("SubscriptionExpiryJob đã dừng.");
    }

    /// <summary>
    /// Xử lý batch subscription đã quá hạn tại thời điểm hiện tại.
    /// Luồng xử lý: lấy danh sách quá hạn, expire từng subscription, publish event, rồi commit SaveChanges.
    /// </summary>
    private async Task ProcessExpirationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        var domainEventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();

        var cutoff = DateTime.UtcNow;

        var expiredSubs = await repo.GetExpiredSubscriptionsToProcessAsync(cutoff, cancellationToken);
        if (expiredSubs.Count == 0)
        {
            // Không có subscription quá hạn thì kết thúc batch sớm.
            return;
        }

        foreach (var sub in expiredSubs)
        {
            try
            {
                sub.Expire();
                // Đổi state domain sang Expired theo rule lifecycle subscription.

                await domainEventPublisher.PublishAsync(new SubscriptionExpiredDomainEvent(sub.UserId, sub.Id), cancellationToken);
                // Publish event để các subscriber thu hồi entitlement/hậu xử lý đồng bộ.
            }
            catch (Exception itemEx)
            {
                _logger.LogError(itemEx, "Lôi Ngang Mắc Xương Tại Gói {SubId} Của Khách {UserId}. Tạm Qua Trảm Đứa Sau Mất Lệ.", sub.Id, sub.UserId);
                // Bỏ qua item lỗi để vẫn xử lý các subscription còn lại trong cùng batch.
            }
        }

        await repo.SaveChangesAsync(cancellationToken);
        // Commit một lần sau batch để giảm số transaction ghi lặp.

        _logger.LogInformation("Tìm Vào Sát Cửa Tử {Count} Hồ Sơ Đăng Ký Đã Hết Giờ Linh Thiêng Phù Hộ (Gắn Expired).", expiredSubs.Count);
    }
}
