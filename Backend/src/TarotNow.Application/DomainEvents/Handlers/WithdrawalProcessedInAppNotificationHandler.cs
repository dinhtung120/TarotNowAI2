using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Common.Realtime;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Gửi thông báo in-app cho user sau khi request được duyệt hoặc từ chối.
/// </summary>
public sealed class WithdrawalProcessedInAppNotificationHandler
    : IdempotentDomainEventNotificationHandler<WithdrawalProcessedDomainEvent>
{
    private const string WithdrawalApprovedType = "withdrawal_approved";
    private const string WithdrawalRejectedType = "withdrawal_rejected";

    private readonly INotificationRepository _notificationRepository;
    private readonly IRedisPublisher _redisPublisher;

    /// <summary>
    /// Khởi tạo handler in-app notification cho withdrawal processed.
    /// </summary>
    public WithdrawalProcessedInAppNotificationHandler(
        INotificationRepository notificationRepository,
        IRedisPublisher redisPublisher,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
        _redisPublisher = redisPublisher;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        WithdrawalProcessedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var dto = BuildNotification(domainEvent);
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

    private static NotificationCreateDto BuildNotification(WithdrawalProcessedDomainEvent domainEvent)
    {
        if (string.Equals(domainEvent.Action, WithdrawalProcessAction.Approve, StringComparison.OrdinalIgnoreCase))
        {
            return new NotificationCreateDto
            {
                UserId = domainEvent.UserId,
                Type = WithdrawalApprovedType,
                TitleVi = "Yeu cau rut tien da duoc duyet",
                TitleEn = "Withdrawal request approved",
                TitleZh = "提现申请已通过",
                BodyVi = "Yeu cau rut tien cua ban da duoc phe duyet. Admin se chuyen khoan thu cong som nhat.",
                BodyEn = "Your withdrawal request is approved. Admin will transfer funds manually shortly.",
                BodyZh = "你的提现申请已批准，管理员将尽快手动转账。",
                Metadata = BuildMetadata(domainEvent)
            };
        }

        var rejectReason = string.IsNullOrWhiteSpace(domainEvent.AdminNote) ? "Khong co" : domainEvent.AdminNote;
        return new NotificationCreateDto
        {
            UserId = domainEvent.UserId,
            Type = WithdrawalRejectedType,
            TitleVi = "Yeu cau rut tien bi tu choi",
            TitleEn = "Withdrawal request rejected",
            TitleZh = "提现申请被拒绝",
            BodyVi = $"Yeu cau rut tien bi tu choi. Ly do: {rejectReason}.",
            BodyEn = $"Your withdrawal request was rejected. Reason: {rejectReason}.",
            BodyZh = $"你的提现申请被拒绝。原因：{rejectReason}。",
            Metadata = BuildMetadata(domainEvent)
        };
    }

    private static Dictionary<string, string> BuildMetadata(WithdrawalProcessedDomainEvent domainEvent)
    {
        return new Dictionary<string, string>
        {
            ["requestId"] = domainEvent.RequestId.ToString(),
            ["status"] = domainEvent.Status,
            ["action"] = domainEvent.Action
        };
    }
}
