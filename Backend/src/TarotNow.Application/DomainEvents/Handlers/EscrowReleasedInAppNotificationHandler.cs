using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

// Tạo và phát thông báo in-app cho cả payer/receiver khi giao dịch escrow được giải ngân.
public sealed class EscrowReleasedInAppNotificationHandler
    : IdempotentDomainEventNotificationHandler<EscrowReleasedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler thông báo in-app khi escrow release.
    /// Luồng xử lý: nhận repository lưu thông báo, push service và wallet push service.
    /// </summary>
    public EscrowReleasedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
        _redisPublisher = redisPublisher;
    }

    /// <summary>
    /// Xử lý notification giải ngân và phát đầy đủ thông báo liên quan.
    /// Luồng xử lý: dựng dto cho payer/receiver rồi lưu + push từng dto.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        EscrowReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var payerDto = BuildPayerNotification(domainEvent);
        var receiverDto = BuildReceiverNotification(domainEvent);

        await PersistAndPublishRealtimeNotificationAsync(payerDto, cancellationToken);
        await PersistAndPublishRealtimeNotificationAsync(receiverDto, cancellationToken);
    }

    /// <summary>
    /// Lưu notification vào kho dữ liệu rồi publish realtime cho người dùng đích.
    /// Luồng xử lý: persist trước để đảm bảo lịch sử, sau đó publish event qua Redis.
    /// </summary>
    private async Task PersistAndPublishRealtimeNotificationAsync(NotificationCreateDto dto, CancellationToken cancellationToken)
    {
        await _notificationRepository.CreateAsync(dto, cancellationToken);
        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Notifications,
            RealtimeEventNames.NotificationNew,
            new
            {
                userId = dto.UserId.ToString(),
                dto.TitleVi,
                dto.TitleEn,
                dto.BodyVi,
                dto.BodyEn,
                dto.Type,
                createdAt = DateTime.UtcNow
            },
            cancellationToken);
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

// Tạo và phát thông báo in-app theo session khi giao dịch escrow được giải ngân gộp.
public sealed class EscrowSessionReleasedInAppNotificationHandler
    : IdempotentDomainEventNotificationHandler<EscrowSessionReleasedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler thông báo in-app khi escrow session release.
    /// Luồng xử lý: nhận repository lưu thông báo và redis publisher để phát realtime.
    /// </summary>
    public EscrowSessionReleasedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
        _redisPublisher = redisPublisher;
    }

    /// <summary>
    /// Xử lý notification giải ngân gộp theo session.
    /// Luồng xử lý: dựng dto cho payer/receiver rồi lưu + push từng dto đúng 1 lần cho mỗi bên.
    /// </summary>
    protected override async Task HandleDomainEventAsync(
        EscrowSessionReleasedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var payerDto = BuildPayerNotification(domainEvent);
        var receiverDto = BuildReceiverNotification(domainEvent);

        await PersistAndPublishRealtimeNotificationAsync(payerDto, cancellationToken);
        await PersistAndPublishRealtimeNotificationAsync(receiverDto, cancellationToken);
    }

    /// <summary>
    /// Lưu notification rồi publish realtime event cho user đích.
    /// </summary>
    private async Task PersistAndPublishRealtimeNotificationAsync(NotificationCreateDto dto, CancellationToken cancellationToken)
    {
        await _notificationRepository.CreateAsync(dto, cancellationToken);
        await _redisPublisher.PublishAsync(
            RealtimeChannelNames.Notifications,
            RealtimeEventNames.NotificationNew,
            new
            {
                userId = dto.UserId.ToString(),
                dto.TitleVi,
                dto.TitleEn,
                dto.BodyVi,
                dto.BodyEn,
                dto.Type,
                createdAt = DateTime.UtcNow
            },
            cancellationToken);
    }

    /// <summary>
    /// Dựng notification dành cho người trả phí sau khi session escrow được release.
    /// </summary>
    private static NotificationCreateDto BuildPayerNotification(EscrowSessionReleasedDomainEvent domainEvent)
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
                ["financeSessionId"] = domainEvent.FinanceSessionId.ToString(),
                ["receiverId"] = domainEvent.ReceiverId.ToString(),
                ["releasedItemCount"] = domainEvent.ReleasedItemCount.ToString()
            }
        };
    }

    /// <summary>
    /// Dựng notification dành cho reader nhận tiền sau khi session escrow được release.
    /// </summary>
    private static NotificationCreateDto BuildReceiverNotification(EscrowSessionReleasedDomainEvent domainEvent)
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
                ["financeSessionId"] = domainEvent.FinanceSessionId.ToString(),
                ["payerId"] = domainEvent.PayerId.ToString(),
                ["releasedItemCount"] = domainEvent.ReleasedItemCount.ToString()
            }
        };
    }
}
