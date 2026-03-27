using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Api.Services;

/// <summary>
/// Background Service quét định kỳ (mỗi 60s) để dọn dẹp các user đã mất kết nối quá 15 phút.
/// </summary>
public class PresenceTimeoutBackgroundService : BackgroundService
{
    private readonly ILogger<PresenceTimeoutBackgroundService> _logger;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;

    // Timeout 15 phút theo yêu cầu
    private readonly TimeSpan _timeoutPeriod = TimeSpan.FromMinutes(15);
    
    // Quét mỗi 60 giây
    private readonly TimeSpan _scanInterval = TimeSpan.FromSeconds(60);

    public PresenceTimeoutBackgroundService(
        ILogger<PresenceTimeoutBackgroundService> logger,
        IUserPresenceTracker presenceTracker,
        IHubContext<PresenceHub> hubContext,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _presenceTracker = presenceTracker;
        _hubContext = hubContext;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[PresenceTimeout] Service started. Scan interval: {Interval}s, Timeout: {Timeout}m", 
            _scanInterval.TotalSeconds, _timeoutPeriod.TotalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessTimeoutsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PresenceTimeout] Error occurred while processing timeouts.");
            }

            // Đợi đến chu kỳ quét tiếp theo
            await Task.Delay(_scanInterval, stoppingToken);
        }
    }

    private async Task ProcessTimeoutsAsync(CancellationToken cancellationToken)
    {
        var timedOutUsers = _presenceTracker.GetTimedOutUsers(_timeoutPeriod);

        if (timedOutUsers.Count == 0)
        {
            return;
        }

        _logger.LogInformation("[PresenceTimeout] Found {Count} users timed out: {Users}", 
            timedOutUsers.Count, string.Join(", ", timedOutUsers));

        foreach (var userId in timedOutUsers)
        {
            // 1. Xóa khỏi In-Memory Tracker
            _presenceTracker.RemoveUser(userId);

            // 2. Broadcast sự kiện Offline cho tất cả client đang online
            await _hubContext.Clients.All.SendAsync("UserStatusChanged", userId, "offline", cancellationToken);
        }

        // 3. Nếu user đó là Reader, cần cập nhật DB (Online -> Offline)
        // Vì BackgroundService là Singleton, ta cần tạo Scope để resolve mảng Scoped Services (Repository)
        using var scope = _scopeFactory.CreateScope();
        var profileRepo = scope.ServiceProvider.GetRequiredService<IReaderProfileRepository>();

        // Cập nhật Database cho Reader
        await UpdateReaderProfilesToOfflineAsync(profileRepo, timedOutUsers, cancellationToken);
    }

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
                // Chỉ cập nhật nếu trạng thái hiện tại đang là Online.
                // Nếu Reader đang "Busy" (Busy) thì KHÔNG tự động chuyển sang Offline,
                // tôn trọng lựa chọn thủ công của Reader.
                if (string.Equals(profile.Status, ReaderOnlineStatus.Online, StringComparison.OrdinalIgnoreCase))
                {
                    profile.Status = ReaderOnlineStatus.Offline;
                    await profileRepo.UpdateAsync(profile, cancellationToken);
                    
                    _logger.LogInformation("[PresenceTimeout] Updated Reader DB status to Offline for user {UserId}", profile.UserId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceTimeout] Failed to update reader profiles to offline in DB.");
        }
    }
}
