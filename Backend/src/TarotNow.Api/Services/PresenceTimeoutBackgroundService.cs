using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Constants;
using TarotNow.Api.Hubs;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Services;

public class PresenceTimeoutBackgroundService : BackgroundService
{
    private readonly ILogger<PresenceTimeoutBackgroundService> _logger;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;

    
    private readonly TimeSpan _timeoutPeriod = TimeSpan.FromMinutes(15);
    
    
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
        _logger.LogDebug("[PresenceTimeout] Service started. Scan interval: {Interval}s, Timeout: {Timeout}m", 
            _scanInterval.TotalSeconds, _timeoutPeriod.TotalMinutes);

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
                    
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "[PresenceTimeout] Error occurred while processing timeouts.");
                }

                
                await Task.Delay(_scanInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[PresenceTimeout] Dịch vụ dọn dẹp hiện diện đang dừng...");
        }

        _logger.LogDebug("[PresenceTimeout] Service stopped.");
    }

    private async Task ProcessTimeoutsAsync(CancellationToken cancellationToken)
    {
        var timedOutUsers = _presenceTracker.GetTimedOutUsers(_timeoutPeriod);

        if (timedOutUsers.Count == 0)
        {
            return;
        }

        _logger.LogDebug("[PresenceTimeout] Found {Count} users timed out: {Users}", 
            timedOutUsers.Count, string.Join(", ", timedOutUsers));

        foreach (var userId in timedOutUsers)
        {
            
            _presenceTracker.RemoveUser(userId);

            
            await _hubContext.Clients.All.SendAsync("UserStatusChanged", userId, "offline", cancellationToken);
        }

        
        
        using var scope = _scopeFactory.CreateScope();
        var profileRepo = scope.ServiceProvider.GetRequiredService<IReaderProfileRepository>();

        
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
                
                
                
                if (string.Equals(profile.Status, ApiReaderStatusConstants.Online, StringComparison.OrdinalIgnoreCase))
                {
                    profile.Status = ApiReaderStatusConstants.Offline;
                    await profileRepo.UpdateAsync(profile, cancellationToken);
                    
                    _logger.LogDebug("[PresenceTimeout] Updated Reader DB status to Offline for user {UserId}", profile.UserId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[PresenceTimeout] Failed to update reader profiles to offline in DB.");
        }
    }
}
