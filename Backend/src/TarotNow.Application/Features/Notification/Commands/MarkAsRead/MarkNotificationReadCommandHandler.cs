using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

// Handler đánh dấu một thông báo đã đọc theo user.
public class MarkNotificationReadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MarkNotificationReadCommandHandlerRequestedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler mark-as-read.
    /// Luồng xử lý: nhận notification repository để cập nhật trạng thái read của bản ghi mục tiêu.
    /// </summary>
    public MarkNotificationReadCommandHandlerRequestedDomainEventHandler(
        INotificationRepository notificationRepository,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý command đánh dấu một thông báo đã đọc.
    /// Luồng xử lý: gọi repository theo NotificationId + UserId để đảm bảo chỉ user sở hữu mới cập nhật được bản ghi.
    /// </summary>
    public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.MarkAsReadAsync(
            request.NotificationId,
            request.UserId,
            cancellationToken);
    }

    protected override async Task HandleDomainEventAsync(
        MarkNotificationReadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
