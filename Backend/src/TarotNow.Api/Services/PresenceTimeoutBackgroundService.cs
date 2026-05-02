using MediatR;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Features.Presence.Commands.PublishUserStatusChanged;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Services;

// Worker nền dọn trạng thái hiện diện quá hạn và phát domain event offline.
public class PresenceTimeoutBackgroundService : BackgroundService
{
    private readonly ILogger<PresenceTimeoutBackgroundService> _logger;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly IMediator _mediator;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo worker dọn trạng thái presence quá hạn.
    /// Luồng xử lý: nhận tracker + mediator để cleanup presence state và phát domain event offline.
    /// </summary>
    public PresenceTimeoutBackgroundService(
        ILogger<PresenceTimeoutBackgroundService> logger,
        IUserPresenceTracker presenceTracker,
        IMediator mediator,
        ISystemConfigSettings systemConfigSettings)
    {
        _logger = logger;
        _presenceTracker = presenceTracker;
        _mediator = mediator;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Vòng lặp nền quét user timeout theo chu kỳ cho đến khi dịch vụ dừng.
    /// Luồng xử lý: lặp theo scan interval, xử lý timeout, bắt lỗi từng lượt quét để service không chết toàn cục.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timeoutPeriod = ResolveTimeoutPeriod();
        var scanInterval = ResolveScanInterval();

        _logger.LogDebug("[PresenceTimeout] Service started. Scan interval: {Interval}s, Timeout: {Timeout}m",
            scanInterval.TotalSeconds, timeoutPeriod.TotalMinutes);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Mỗi vòng quét xử lý đầy đủ nhóm user đã quá hạn hoạt động.
                    await ProcessTimeoutsAsync(timeoutPeriod, stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Edge case khi shutdown: bỏ qua lỗi dispose phát sinh do hủy token trong lúc giải phóng tài nguyên.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    // Lỗi một lượt quét không được làm dừng worker; ghi log và tiếp tục vòng sau.
                    _logger.LogError(ex, "[PresenceTimeout] Error occurred while processing timeouts.");
                }

                // Chờ theo chu kỳ quét; token bị hủy sẽ nhảy ra ngoài qua OperationCanceledException.
                await Task.Delay(scanInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[PresenceTimeout] Dịch vụ dọn dẹp hiện diện đang dừng...");
        }

        _logger.LogDebug("[PresenceTimeout] Service stopped.");
    }

    /// <summary>
    /// Xử lý danh sách user đã timeout: cập nhật tracker và publish offline event.
    /// Luồng xử lý: lấy danh sách timeout, remove state và publish command cho từng user.
    /// </summary>
    private async Task ProcessTimeoutsAsync(TimeSpan timeoutPeriod, CancellationToken cancellationToken)
    {
        var timedOutUsers = _presenceTracker.GetTimedOutUsers(timeoutPeriod);

        if (timedOutUsers.Count == 0)
        {
            // Không có user quá hạn thì kết thúc sớm để giảm chi phí I/O không cần thiết.
            return;
        }

        _logger.LogDebug("[PresenceTimeout] Found {Count} users timed out: {Users}",
            timedOutUsers.Count, string.Join(", ", timedOutUsers));

        foreach (var userId in timedOutUsers)
        {
            // Bước 1: xóa trạng thái hiện diện khỏi tracker để lần kiểm tra online sau phản ánh đúng offline.
            _presenceTracker.RemoveUser(userId);
            await PublishOfflineStatusAsync(userId, cancellationToken);
        }

        // Side-effect cập nhật projection Reader status sẽ được thực hiện tại Domain Event handler
        // của UserStatusChangedDomainEvent để tuân thủ Rule 0 (không ghi trực tiếp repository tại service này).
    }

    private async Task PublishOfflineStatusAsync(string userId, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(
                new PublishUserStatusChangedCommand
                {
                    UserId = userId,
                    Status = "offline"
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceTimeout] Failed to publish offline status for user {UserId}.", userId);
        }
    }

    private TimeSpan ResolveTimeoutPeriod()
    {
        var minutes = Math.Clamp(_systemConfigSettings.PresenceTimeoutMinutes, 1, 240);
        return TimeSpan.FromMinutes(minutes);
    }

    private TimeSpan ResolveScanInterval()
    {
        var seconds = Math.Clamp(_systemConfigSettings.PresenceScanIntervalSeconds, 5, 600);
        return TimeSpan.FromSeconds(seconds);
    }
}
