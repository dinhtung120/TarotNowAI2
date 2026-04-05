using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowReleasedInAppNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _pushService;
    private readonly IWalletPushService _walletPushService;

    public EscrowReleasedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        INotificationPushService pushService,
        IWalletPushService walletPushService)
    {
        _notificationRepository = notificationRepository;
        _pushService = pushService;
        _walletPushService = walletPushService;
    }

    public async Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var payerDto = BuildPayerNotification(domainEvent);
        var receiverDto = BuildReceiverNotification(domainEvent);

        await PersistAndPushAsync(payerDto, cancellationToken);
        await PersistAndPushAsync(receiverDto, cancellationToken);
        await PushWalletUpdatesAsync(domainEvent.PayerId, domainEvent.ReceiverId, cancellationToken);
    }

    private async Task PersistAndPushAsync(NotificationCreateDto dto, CancellationToken cancellationToken)
    {
        await _notificationRepository.CreateAsync(dto, cancellationToken);
        await _pushService.PushNewNotificationAsync(dto, cancellationToken);
    }

    private async Task PushWalletUpdatesAsync(Guid payerId, Guid receiverId, CancellationToken cancellationToken)
    {
        await _walletPushService.PushBalanceChangedAsync(payerId, cancellationToken);
        await _walletPushService.PushBalanceChangedAsync(receiverId, cancellationToken);
    }

    private static NotificationCreateDto BuildPayerNotification(Domain.Events.EscrowReleasedDomainEvent domainEvent)
    {
        return new NotificationCreateDto
        {
            UserId = domainEvent.PayerId,
            Type = "escrow_released",
            TitleVi = "Phiên chat đã hoàn tất",
            TitleEn = "Chat escrow settled",
            TitleZh = "聊天托管已结算",
            BodyVi = $"Đã giải ngân {domainEvent.ReleasedAmountDiamond} kim cương cho Reader.",
            BodyEn = $"Released {domainEvent.ReleasedAmountDiamond} diamonds to the reader.",
            BodyZh = $"已向占卜师释放 {domainEvent.ReleasedAmountDiamond} 钻石。",
            Metadata = new Dictionary<string, string>
            {
                ["itemId"] = domainEvent.ItemId.ToString(),
                ["receiverId"] = domainEvent.ReceiverId.ToString()
            }
        };
    }

    private static NotificationCreateDto BuildReceiverNotification(Domain.Events.EscrowReleasedDomainEvent domainEvent)
    {
        return new NotificationCreateDto
        {
            UserId = domainEvent.ReceiverId,
            Type = "escrow_income",
            TitleVi = "Bạn vừa nhận kim cương",
            TitleEn = "You received diamonds",
            TitleZh = "你已收到钻石",
            BodyVi = $"Bạn nhận được {domainEvent.ReleasedAmountDiamond} kim cương từ phiên chat.",
            BodyEn = $"You received {domainEvent.ReleasedAmountDiamond} diamonds from chat escrow.",
            BodyZh = $"你从聊天托管中收到 {domainEvent.ReleasedAmountDiamond} 钻石。",
            Metadata = new Dictionary<string, string>
            {
                ["itemId"] = domainEvent.ItemId.ToString(),
                ["payerId"] = domainEvent.PayerId.ToString()
            }
        };
    }
}
