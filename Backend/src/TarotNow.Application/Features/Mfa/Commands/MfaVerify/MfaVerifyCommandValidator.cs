using FluentValidation;

namespace TarotNow.Application.Features.Mfa.Commands.MfaVerify;

// Validator cho command verify MFA.
public class MfaVerifyCommandValidator : AbstractValidator<MfaVerifyCommand>
{
    /// <summary>
    /// Khởi tạo rule validation cho dữ liệu verify MFA.
    /// Luồng xử lý: bắt buộc UserId và giới hạn độ dài Code để chặn input rỗng/không hợp lệ sớm.
    /// </summary>
    public MfaVerifyCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        // UserId bắt buộc để xác định đúng tài khoản cần bật MFA.

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6, 64);
        // Khoảng độ dài hỗ trợ TOTP tiêu chuẩn và các biến thể mã xác thực có định dạng dài hơn.
    }
}
