using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Register;

// Validator đầu vào cho command đăng ký.
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Khởi tạo bộ rule validation cho RegisterCommand.
    /// Luồng xử lý: kiểm tra email/username/password/displayname/date of birth/consent theo chính sách đăng ký.
    /// </summary>
    public RegisterCommandValidator()
    {
        // Email bắt buộc và đúng định dạng.
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        // Username bắt buộc, đủ dài và chỉ chứa ký tự an toàn.
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain alphanumeric characters and underscores.");

        // Password tối thiểu theo rule bảo mật cơ bản.
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        // DisplayName tùy chọn nhưng cần giới hạn độ dài hiển thị.
        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("DisplayName cannot exceed 100 characters.");

        // Người dùng phải đủ 18 tuổi theo quy định sử dụng dịch vụ.
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old to register.");

        // Bắt buộc đồng ý điều khoản trước khi tạo tài khoản.
        RuleFor(x => x.HasConsented)
            .Equal(true).WithMessage("You must consent to the Terms of Service and Privacy Policy.");
    }

    /// <summary>
    /// Kiểm tra tuổi người dùng có tối thiểu 18 tại thời điểm hiện tại hay không.
    /// Luồng xử lý: tính chênh lệch năm, điều chỉnh theo mốc sinh nhật, so sánh với tuổi tối thiểu.
    /// </summary>
    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var minAge = 18;
        var today = DateTime.UtcNow.Date;
        var diff = today.Year - dateOfBirth.Year;

        // Nếu chưa qua sinh nhật năm nay thì giảm tuổi thực xuống 1.
        if (dateOfBirth.Date > today.AddYears(-diff))
        {
            diff--;
        }

        return diff >= minAge;
    }
}
