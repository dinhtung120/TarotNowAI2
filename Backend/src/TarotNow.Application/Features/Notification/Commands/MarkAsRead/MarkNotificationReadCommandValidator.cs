using FluentValidation;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

// Validator cho command đánh dấu một thông báo đã đọc.
public class MarkNotificationReadCommandValidator : AbstractValidator<MarkNotificationReadCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho yêu cầu mark notification as read.
    /// Luồng xử lý: bắt buộc NotificationId và UserId để tránh cập nhật mơ hồ hoặc sai chủ sở hữu.
    /// </summary>
    public MarkNotificationReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty();
        // NotificationId bắt buộc để định vị đúng bản ghi cần đổi trạng thái.

        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để chặn truy cập chéo notification của user khác.
    }
}
