using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

// Handler đánh dấu một thông báo đã đọc theo user.
public class MarkNotificationReadCommandExecutor : ICommandExecutionExecutor<MarkNotificationReadCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler mark-as-read.
    /// Luồng xử lý: nhận notification repository để cập nhật trạng thái read của bản ghi mục tiêu.
    /// </summary>
    public MarkNotificationReadCommandExecutor(INotificationRepository notificationRepository)
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
}
