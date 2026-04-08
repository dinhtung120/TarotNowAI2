using MediatR;
using TarotNow.Application.DomainEvents.Notifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.DomainEvents.Handlers;

// Tạo và phát thông báo in-app cho cả payer/receiver khi giao dịch escrow được giải ngân.
public sealed class EscrowReleasedInAppNotificationHandler : INotificationHandler<EscrowReleasedNotification>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPushService _pushService;
    private readonly IWalletPushService _walletPushService;

    /// <summary>
    /// Khởi tạo handler thông báo in-app khi escrow release.
    /// Luồng xử lý: nhận repository lưu thông báo, push service và wallet push service.
    /// </summary>
    public EscrowReleasedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        INotificationPushService pushService,
        IWalletPushService walletPushService)
    {
        _notificationRepository = notificationRepository;
        _pushService = pushService;
        _walletPushService = walletPushService;
    }

    /// <summary>
    /// Xử lý notification giải ngân và phát đầy đủ thông báo liên quan.
    /// Luồng xử lý: dựng dto cho payer/receiver, lưu + push từng dto, rồi push cập nhật ví cho cả hai bên.
    /// </summary>
    public async Task Handle(EscrowReleasedNotification notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var payerDto = BuildPayerNotification(domainEvent);
        var receiverDto = BuildReceiverNotification(domainEvent);

        // Ghi và push hai notification tách biệt để nội dung đúng theo từng vai trò.
        await PersistAndPushAsync(payerDto, cancellationToken);
        await PersistAndPushAsync(receiverDto, cancellationToken);
        // Đồng bộ tín hiệu thay đổi số dư ví cho cả bên trả và bên nhận.
        await PushWalletUpdatesAsync(domainEvent.PayerId, domainEvent.ReceiverId, cancellationToken);
    }

    /// <summary>
    /// Lưu notification vào kho dữ liệu rồi push realtime cho người dùng đích.
    /// Luồng xử lý: persist trước để đảm bảo lịch sử, sau đó gửi push event mới.
    /// </summary>
    private async Task PersistAndPushAsync(NotificationCreateDto dto, CancellationToken cancellationToken)
    {
        await _notificationRepository.CreateAsync(dto, cancellationToken);
        await _pushService.PushNewNotificationAsync(dto, cancellationToken);
    }

    /// <summary>
    /// Push sự kiện thay đổi số dư ví cho hai user liên quan giao dịch.
    /// Luồng xử lý: gọi wallet push lần lượt cho payer và receiver.
    /// </summary>
    private async Task PushWalletUpdatesAsync(Guid payerId, Guid receiverId, CancellationToken cancellationToken)
    {
        await _walletPushService.PushBalanceChangedAsync(payerId, cancellationToken);
        await _walletPushService.PushBalanceChangedAsync(receiverId, cancellationToken);
    }

    /// <summary>
    /// Dựng notification dành cho người trả phí (payer) sau khi escrow được release.
    /// Luồng xử lý: đóng gói nội dung đa ngôn ngữ và metadata item/receiver cho client.
    /// </summary>
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

    /// <summary>
    /// Dựng notification dành cho reader nhận tiền (receiver) sau khi escrow được release.
    /// Luồng xử lý: đóng gói nội dung đa ngôn ngữ và metadata item/payer cho client.
    /// </summary>
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
