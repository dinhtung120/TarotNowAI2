using FluentValidation;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.CreateUser;

// Validator bảo vệ dữ liệu đầu vào khi admin tạo user.
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    // Danh sách role hợp lệ cho thao tác tạo user từ admin.
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.Ordinal)
    {
        UserRole.User,
        UserRole.TarotReader,
        UserRole.Admin,
    };

    /// <summary>
    /// Khởi tạo bộ rule validation cho CreateUserCommand.
    /// Luồng xử lý: ràng buộc định dạng email, username, display name, password và role hợp lệ.
    /// </summary>
    public CreateUserCommandValidator()
    {
        // Email bắt buộc, đúng định dạng và giới hạn độ dài phù hợp lưu trữ.
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        // Username phải đủ dài, giới hạn ký tự an toàn và có độ dài tối đa hợp lý.
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain alphanumeric characters and underscores.");

        // DisplayName là tùy chọn nhưng cần giới hạn độ dài để bảo vệ dữ liệu trình bày.
        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("DisplayName cannot exceed 100 characters.");

        // Rule mật khẩu tối thiểu nhằm đáp ứng tiêu chuẩn bảo mật cơ bản.
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        // Role bắt buộc thuộc danh sách hợp lệ để tránh gán quyền ngoài chính sách.
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => AllowedRoles.Contains((role ?? string.Empty).Trim().ToLowerInvariant()))
            .WithMessage("Role must be one of: user, tarot_reader, admin.");
    }
}
