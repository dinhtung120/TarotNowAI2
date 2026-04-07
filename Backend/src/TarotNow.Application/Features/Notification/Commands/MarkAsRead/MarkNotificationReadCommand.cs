

using MediatR;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

public class MarkNotificationReadCommand : IRequest<bool>
{
        public string NotificationId { get; set; } = string.Empty;

        public Guid UserId { get; set; }
}
