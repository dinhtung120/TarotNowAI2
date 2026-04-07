

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

        public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.MarkAsReadAsync(
            request.NotificationId,
            request.UserId,
            cancellationToken);
    }
}
