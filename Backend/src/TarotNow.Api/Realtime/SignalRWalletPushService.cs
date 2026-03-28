/*
 * ===================================================================
 * FILE: SignalRWalletPushService.cs
 * NAMESPACE: TarotNow.Api.Realtime
 * ===================================================================
 * MỤC ĐÍCH:
 *   Implement cho IWalletPushService bằng SignalR.
 *   Gọi vào IHubContext<PresenceHub> để đẩy event 'wallet.balance_changed' 
 *   qua WebSocket tới đúng user tương ứng.
 * ===================================================================
 */

using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

/// <summary>
/// Service nhận tín hiệu Ví thay đổi từ Application Layer 
/// và đẩy real-time xuống Client qua PresenceHub.
/// </summary>
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

    /// <summary>
    /// Đẩy sự kiện 'wallet.balance_changed' tới group 'user:{userId}'.
    /// Payload hiện tại là rỗng, frontend sẽ tự động gọi API fetch số dư mới nhằm bảo mật dữ liệu.
    /// </summary>
    public async Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{userId}";

        try
        {
            // Push tín hiệu "wallet.balance_changed"
            await _hubContext.Clients.Group(groupName).SendAsync("wallet.balance_changed", cancellationToken);

            _logger.LogInformation("[WalletPush] Đã push tín hiệu balance_changed cho user {UserId}", userId);
        }
        catch (Exception ex)
        {
            // Best effort, lỗi không ảnh hưởng luồng chính
            _logger.LogError(ex, "[WalletPush] Lỗi khi push balance_changed cho user {UserId}", userId);
        }
    }
}
