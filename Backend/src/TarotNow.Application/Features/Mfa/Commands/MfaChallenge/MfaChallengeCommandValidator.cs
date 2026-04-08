using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaChallenge;

// Validator cho command challenge MFA.
public class MfaChallengeCommandValidator : AbstractValidator<MfaChallengeCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu challenge MFA.
    /// Luồng xử lý: bắt buộc UserId hợp lệ và giới hạn độ dài Code để chặn input rỗng hoặc bất thường.
    /// </summary>
    public MfaChallengeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để truy xuất đúng tài khoản cần xác thực.

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6, 64);
        // Cho phép cả TOTP ngắn và backup code dài hơn nhưng vẫn đặt trần an toàn cho input.
    }
}
