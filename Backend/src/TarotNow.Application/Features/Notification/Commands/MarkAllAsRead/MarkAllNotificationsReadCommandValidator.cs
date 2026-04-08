using FluentValidation;

namespace TarotNow.Application.Features.Notification.Commands.MarkAllAsRead;

// Validator cho command đánh dấu toàn bộ thông báo đã đọc.
public class MarkAllNotificationsReadCommandValidator : AbstractValidator<MarkAllNotificationsReadCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho yêu cầu mark-all-as-read.
    /// Luồng xử lý: bắt buộc UserId để bảo đảm cập nhật đúng ngữ cảnh người dùng.
    /// </summary>
    public MarkAllNotificationsReadCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId rỗng có thể làm sai lệch phạm vi cập nhật nên phải chặn sớm.
    }
}
