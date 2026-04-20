using MediatR;
using TarotNow.Api.Constants;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Features.Presence.Commands.PublishUserStatusChanged;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Services;

// Worker nền dọn trạng thái hiện diện quá hạn để đồng bộ SignalR và trạng thái reader trong database.
public class PresenceTimeoutBackgroundService : BackgroundService
{
    private readonly ILogger<PresenceTimeoutBackgroundService> _logger;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly IMediator _mediator;
    private readonly IServiceScopeFactory _scopeFactory;

    // Sau 15 phút không heartbeat thì xem user đã timeout hiện diện.
    private readonly TimeSpan _timeoutPeriod = TimeSpan.FromMinutes(15);

    // Quét timeout mỗi 60 giây để cân bằng độ trễ cập nhật và chi phí hệ thống.
    private readonly TimeSpan _scanInterval = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Khởi tạo worker dọn trạng thái presence quá hạn.
    /// Luồng xử lý: nhận tracker, hub context và scope factory để vừa phát realtime vừa cập nhật DB an toàn scope.
    /// </summary>
    public PresenceTimeoutBackgroundService(
        ILogger<PresenceTimeoutBackgroundService> logger,
        IUserPresenceTracker presenceTracker,
        IMediator mediator,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _presenceTracker = presenceTracker;
        _mediator = mediator;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Vòng lặp nền quét user timeout theo chu kỳ cho đến khi dịch vụ dừng.
    /// Luồng xử lý: lặp theo scan interval, xử lý timeout, bắt lỗi từng lượt quét để service không chết toàn cục.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("[PresenceTimeout] Service started. Scan interval: {Interval}s, Timeout: {Timeout}m",
            _scanInterval.TotalSeconds, _timeoutPeriod.TotalMinutes);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Mỗi vòng quét xử lý đầy đủ nhóm user đã quá hạn hoạt động.
                    await ProcessTimeoutsAsync(stoppingToken);
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
                await Task.Delay(_scanInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[PresenceTimeout] Dịch vụ dọn dẹp hiện diện đang dừng...");
        }

        _logger.LogDebug("[PresenceTimeout] Service stopped.");
    }

    /// <summary>
    /// Xử lý danh sách user đã timeout: cập nhật tracker, broadcast offline và đồng bộ trạng thái reader trong DB.
    /// Luồng xử lý: lấy danh sách timeout, xử lý từng user realtime, sau đó cập nhật batch profile qua repository.
    /// </summary>
    private async Task ProcessTimeoutsAsync(CancellationToken cancellationToken)
    {
        var timedOutUsers = _presenceTracker.GetTimedOutUsers(_timeoutPeriod);

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

        // Tạo scope mới cho repository để tránh giữ DbContext ngoài vòng đời phù hợp.
        using var scope = _scopeFactory.CreateScope();
        var profileRepo = scope.ServiceProvider.GetRequiredService<IReaderProfileRepository>();

        // Bước 3: đồng bộ trạng thái reader online/offline trong DB cho các luồng truy vấn không qua SignalR.
        await UpdateReaderProfilesToOfflineAsync(profileRepo, timedOutUsers, cancellationToken);
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

    /// <summary>
    /// Cập nhật trạng thái các reader đã timeout về Offline trong database.
    /// Luồng xử lý: lấy profile theo danh sách user id, chỉ cập nhật record đang Online để tránh ghi thừa.
    /// </summary>
    private async Task UpdateReaderProfilesToOfflineAsync(
        IReaderProfileRepository profileRepo,
        IReadOnlyList<string> userIds,
        CancellationToken cancellationToken)
    {
        try
        {
            var profiles = await profileRepo.GetByUserIdsAsync(userIds, cancellationToken);

            foreach (var profile in profiles)
            {
                if (string.Equals(profile.Status, ApiReaderStatusConstants.Online, StringComparison.OrdinalIgnoreCase))
                {
                    // Rule nghiệp vụ: chỉ chuyển Online -> Offline, giữ nguyên các trạng thái đặc thù khác.
                    profile.Status = ApiReaderStatusConstants.Offline;
                    await profileRepo.UpdateAsync(profile, cancellationToken);

                    // Ghi log tại điểm đổi state DB để truy vết sai lệch giữa presence memory và persistent store.
                    _logger.LogDebug("[PresenceTimeout] Updated Reader DB status to Offline for user {UserId}", profile.UserId);
                }
            }
        }
        catch (Exception ex)
        {
            // Không chặn worker khi lỗi DB tạm thời; lượt quét sau sẽ thử lại.
            _logger.LogError(ex, "[PresenceTimeout] Failed to update reader profiles to offline in DB.");
        }
    }
}
