using MediatR;
using System;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

// Command đánh dấu một thông báo cụ thể là đã đọc.
public class MarkNotificationReadCommand : IRequest<bool>
{
    // Định danh thông báo cần cập nhật trạng thái.
    public string NotificationId { get; set; } = string.Empty;

    // Định danh user sở hữu thông báo để kiểm tra quyền cập nhật.
    public Guid UserId { get; set; }
}
