using FluentValidation;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

// Validator đầu vào cho command khóa/mở khóa user.
public class ToggleUserLockCommandValidator : AbstractValidator<ToggleUserLockCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho ToggleUserLockCommand.
    /// Luồng xử lý: đảm bảo UserId bắt buộc có giá trị hợp lệ.
    /// </summary>
    public ToggleUserLockCommandValidator()
    {
        // UserId bắt buộc để xác định đúng tài khoản cần thao tác.
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
