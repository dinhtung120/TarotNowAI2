using MediatR;
using System;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

public class MarkAllNotificationsReadCommand : IRequest<bool>
{
        public Guid UserId { get; set; }
}
