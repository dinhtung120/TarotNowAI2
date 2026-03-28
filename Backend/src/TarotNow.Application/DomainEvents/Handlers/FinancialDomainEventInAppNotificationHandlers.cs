/*
 * ===================================================================
 * FILE: FinancialDomainEventInAppNotificationHandlers.cs
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý DomainEvent tài chính (EscrowReleased, EscrowRefunded)
 *   → Tạo thông báo in-app (lưu vào MongoDB)
 *   → Push real-time xuống client qua SignalR (PresenceHub)
 *
 * LUỒNG HOẠT ĐỘNG:
 *   1. DomainEvent được publish từ lớp Domain/Application
 *   2. MediatR route event đến handler tương ứng (INotificationHandler)
 *   3. Handler tạo NotificationCreateDto → lưu vào MongoDB
 *   4. Handler gọi INotificationPushService → push event xuống FE
 *   5. FE nhận "notification.new" → invalidate cache → UI cập nhật
 *
 * TẠI SAO PUSH SAU KHI LƯU?
 *   - Đảm bảo dữ liệu đã PERSISTENT trước khi gửi signal.
 *   - Nếu push lỗi → user vẫn thấy notification khi refresh.
 *   - Push là "best effort" → lỗi push KHÔNG rollback tạo notification.
 * ===================================================================
 */

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

        /*
         * Tạo 2 thông báo:
         *   1. Cho PAYER (người trả tiền) — "Đã giải ngân X kim cương"
         *   2. Cho RECEIVER (reader) — "Bạn nhận được X kim cương"
         * Sau mỗi lần lưu vào DB, push signal realtime xuống client tương ứng.
         */

        // === Thông báo cho Payer ===
        var payerDto = new NotificationCreateDto
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
        await _notificationRepository.CreateAsync(payerDto, cancellationToken);
        await _pushService.PushNewNotificationAsync(payerDto, cancellationToken);

        // === Thông báo cho Receiver (Reader) ===
        var receiverDto = new NotificationCreateDto
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
        await _notificationRepository.CreateAsync(receiverDto, cancellationToken);
        await _pushService.PushNewNotificationAsync(receiverDto, cancellationToken);

        // [Real-time Push] Báo cho 2 bên biết ví vừa được cộng/trừ 
        await _walletPushService.PushBalanceChangedAsync(domainEvent.PayerId, cancellationToken);
        await _walletPushService.PushBalanceChangedAsync(domainEvent.ReceiverId, cancellationToken);
    }
}

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

        // [Real-time Push] Báo cho user (payer) biết ví vừa được hoàn tiền
        await _walletPushService.PushBalanceChangedAsync(domainEvent.UserId, cancellationToken);
    }
}
