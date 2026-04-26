using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

// Handler đánh dấu toàn bộ thông báo là đã đọc.
public class MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<MarkAllNotificationsReadCommandHandlerRequestedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler đánh dấu toàn bộ thông báo đã đọc.
    /// Luồng xử lý: nhận repository để cập nhật trạng thái thông báo và logger để ghi vết thao tác.
    /// </summary>
    public MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler(
        INotificationRepository notificationRepository,
        ILogger<MarkAllNotificationsReadCommandHandlerRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    /// <summary>
    /// Xử lý command đánh dấu tất cả thông báo đã đọc.
    /// Luồng xử lý: ghi log tác vụ theo user và gọi repository cập nhật trạng thái read cho toàn bộ thông báo.
    /// </summary>
    public async Task<bool> Handle(MarkAllNotificationsReadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marking all notifications as read for User {UserId}", request.UserId);
        // Ghi log audit thao tác để hỗ trợ truy vết khi có khiếu nại mất thông báo chưa đọc.

        var success = await _notificationRepository.MarkAllAsReadAsync(request.UserId, cancellationToken);
        // Đổi trạng thái hàng loạt ở tầng persistence; kết quả trả về phản ánh số bản ghi có cập nhật thành công.

        return success;
    }

    protected override async Task HandleDomainEventAsync(
        MarkAllNotificationsReadCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await Handle(domainEvent.Command, cancellationToken);
    }
}
