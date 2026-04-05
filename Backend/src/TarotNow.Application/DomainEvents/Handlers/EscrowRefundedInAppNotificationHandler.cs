using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowRefundedInAppNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _pushService;
    private readonly IWalletPushService _walletPushService;

    public EscrowRefundedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        INotificationPushService pushService,
        IWalletPushService walletPushService)
    {
        _notificationRepository = notificationRepository;
        _pushService = pushService;
        _walletPushService = walletPushService;
    }

    public async Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
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

        await _notificationRepository.CreateAsync(dto, cancellationToken);
        await _pushService.PushNewNotificationAsync(dto, cancellationToken);
        await _walletPushService.PushBalanceChangedAsync(domainEvent.UserId, cancellationToken);
    }
}
