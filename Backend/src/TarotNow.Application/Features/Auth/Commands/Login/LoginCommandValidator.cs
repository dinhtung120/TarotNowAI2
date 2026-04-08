using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Login;

// Validator đầu vào cho command đăng nhập.
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho LoginCommand.
    /// Luồng xử lý: kiểm tra identity và password đều bắt buộc có giá trị.
    /// </summary>
    public LoginCommandValidator()
    {
        // EmailOrUsername bắt buộc để xác định account đăng nhập.
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or Username is required.");

        // Password bắt buộc để thực hiện xác thực.
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
