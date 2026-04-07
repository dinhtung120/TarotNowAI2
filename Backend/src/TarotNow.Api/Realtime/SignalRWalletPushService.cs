

using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

public class SignalRWalletPushService : IWalletPushService
{
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly ILogger<SignalRWalletPushService> _logger;

    public SignalRWalletPushService(
        IHubContext<PresenceHub> hubContext,
        ILogger<SignalRWalletPushService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

        public async Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{userId}";

        try
        {
            
            await _hubContext.Clients.Group(groupName).SendAsync("wallet.balance_changed", cancellationToken);

            _logger.LogInformation("[WalletPush] Đã push tín hiệu balance_changed cho user {UserId}", userId);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex, "[WalletPush] Lỗi khi push balance_changed cho user {UserId}", userId);
        }
    }
}
