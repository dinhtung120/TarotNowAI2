using Microsoft.AspNetCore.SignalR;
using TarotNow.Api.Hubs;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Realtime;

// Push tín hiệu thay đổi số dư ví cho client theo thời gian thực.
public class SignalRWalletPushService : IWalletPushService
{
    private readonly IHubContext<PresenceHub> _hubContext;
    private readonly ILogger<SignalRWalletPushService> _logger;

    /// <summary>
    /// Khởi tạo dịch vụ push sự kiện ví qua SignalR.
    /// Luồng xử lý: dùng PresenceHub để gửi theo user group và logger để theo dõi lỗi delivery.
    /// </summary>
    public SignalRWalletPushService(
        IHubContext<PresenceHub> hubContext,
        ILogger<SignalRWalletPushService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Gửi tín hiệu thay đổi số dư tới tất cả kết nối của người dùng.
    /// Luồng xử lý: phát event `wallet.balance_changed` vào group user, log kết quả thành công/thất bại.
    /// </summary>
    public async Task PushBalanceChangedAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var groupName = $"user:{userId}";

        try
        {
            // Chỉ gửi tín hiệu, không gửi số dư trực tiếp để client luôn fetch từ nguồn dữ liệu chuẩn.
            await _hubContext.Clients.Group(groupName).SendAsync("wallet.balance_changed", cancellationToken);

            _logger.LogInformation("[WalletPush] Đã push tín hiệu balance_changed cho user {UserId}", userId);
        }
        catch (Exception ex)
        {
            // Lỗi realtime không được làm fail luồng nghiệp vụ tài chính đã hoàn tất ở backend.
            _logger.LogError(ex, "[WalletPush] Lỗi khi push balance_changed cho user {UserId}", userId);
        }
    }
}
