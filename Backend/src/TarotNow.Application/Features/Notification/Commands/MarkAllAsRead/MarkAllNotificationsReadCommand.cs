using MediatR;
using System;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

// Command đánh dấu toàn bộ thông báo của user là đã đọc.
public class MarkAllNotificationsReadCommand : IRequest<bool>
{
    // Định danh user cần đánh dấu tất cả thông báo đã đọc.
    public Guid UserId { get; set; }
}
