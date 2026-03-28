using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

public class MarkAllNotificationsReadCommandHandler : IRequestHandler<MarkAllNotificationsReadCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<MarkAllNotificationsReadCommandHandler> _logger;

    public MarkAllNotificationsReadCommandHandler(
        INotificationRepository notificationRepository,
        ILogger<MarkAllNotificationsReadCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(MarkAllNotificationsReadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marking all notifications as read for User {UserId}", request.UserId);

        /* 
         * Gọi lệnh repository: Set { IsRead = true } cho TẤT CẢ các records của UserId
         * Return bool indicating whether anything was actually modified
         */
        var success = await _notificationRepository.MarkAllAsReadAsync(request.UserId, cancellationToken);
        
        return success; // True nếu có ít nhất 1 thông báo bị thay đổi
    }
}
