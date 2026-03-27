using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

public sealed class EscrowReleasedInAppNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly INotificationRepository _notificationRepository;

    public EscrowReleasedInAppNotificationHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _notificationRepository.CreateAsync(new NotificationCreateDto
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
        }, cancellationToken);

        await _notificationRepository.CreateAsync(new NotificationCreateDto
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
        }, cancellationToken);
    }
}

public sealed class EscrowRefundedInAppNotificationHandler : INotificationHandler<EscrowRefundedNotification>
{
    private readonly INotificationRepository _notificationRepository;

    public EscrowRefundedInAppNotificationHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(EscrowRefundedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _notificationRepository.CreateAsync(new NotificationCreateDto
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
        }, cancellationToken);
    }
}
