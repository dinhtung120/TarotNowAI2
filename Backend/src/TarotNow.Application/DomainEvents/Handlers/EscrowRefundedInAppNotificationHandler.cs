using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

// Tạo thông báo in-app và push realtime khi escrow hoàn tiền cho người dùng.
public sealed class EscrowRefundedInAppNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _pushService;
    private readonly IWalletPushService _walletPushService;

    /// <summary>
    /// Khởi tạo handler thông báo hoàn tiền in-app.
    /// Luồng xử lý: nhận repository lưu thông báo, push service cho realtime và wallet push để cập nhật số dư.
    /// </summary>
    public EscrowRefundedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        INotificationPushService pushService,
        IWalletPushService walletPushService)
    {
        _notificationRepository = notificationRepository;
        _pushService = pushService;
        _walletPushService = walletPushService;
    }

    /// <summary>
    /// Xử lý notification hoàn tiền và phát đầy đủ tín hiệu cho client.
    /// Luồng xử lý: dựng DTO thông báo, lưu DB, push notification realtime, rồi push tín hiệu đổi số dư ví.
    /// </summary>
    public async Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        // Đóng gói metadata item/source để client có thể mở chi tiết giao dịch hoàn tiền.
        var dto = new NotificationCreateDto
        {
            UserId = domainEvent.UserId,
            Type = "escrow_refunded",
            TitleVi = "Hoàn tiền escrow thành công",
            TitleEn = "Escrow refunded",
            TitleZh = "托管退款成功",
            BodyVi = $"Đã hoàn {domainEvent.AmountDiamond} kim cương về ví của bạn.",
            BodyEn = $"Refunded {domainEvent.AmountDiamond} diamonds to your wallet.",
            BodyZh = $"已向你的钱包退还 {domainEvent.AmountDiamond} 钻石。",
            Metadata = new Dictionary<string, string>
            {
                ["itemId"] = domainEvent.ItemId.ToString(),
                ["source"] = domainEvent.RefundSource
            }
        };

        // Bước 1: lưu notification vào kho dữ liệu để truy vết lịch sử.
        await _notificationRepository.CreateAsync(dto, cancellationToken);
        // Bước 2: push notification mới để UI cập nhật tức thời.
        await _pushService.PushNewNotificationAsync(dto, cancellationToken);
        // Bước 3: push tín hiệu ví để client chủ động reload số dư.
        await _walletPushService.PushBalanceChangedAsync(domainEvent.UserId, cancellationToken);
    }
}
