using MediatR;
using System;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

/// <summary>
/// COMMAND: MarkAllNotificationsReadCommand
/// Yêu cầu đánh dấu tất cả thông báo của một người dùng thành trạng thái đã đọc.
/// </summary>
public class MarkAllNotificationsReadCommand : IRequest<bool>
{
    /// <summary>Id của User sở hữu các thông báo (lấy từ JWT qua lớp bảo mật, không cho FE truyền).</summary>
    public Guid UserId { get; set; }
}
