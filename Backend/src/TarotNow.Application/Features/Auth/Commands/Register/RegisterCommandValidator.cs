/*
 * ===================================================================
 * FILE: RegisterCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Register
 * ===================================================================
 * MỤC ĐÍCH:
 *   Bộ quy tắc (Rules) bảo vệ API Đăng Ký trước khi chạm vào Database.
 *   Xác minh tính hợp lệ, độ tuổi, và đồng thuận điều khoản.
 *
 * CHIẾN LƯỢC BẢO MẬT:
 *   1. Mật khẩu Mạnh (Strong Password): Ép buộc có chữ hoa, chữ thường và số 
 *      để chống tấn công rà quét từ điển.
 *   2. Hạn chế ký tự Username: Chỉ cho phép chữ, số và dấu gạch dưới 
 *      để tránh lỗi định dạng URL hoặc lỗi Injection.
 *   3. Chặn đăng ký người dưới 18 tuổi: Tuân theo điều luật dịch vụ tài chính 
 *      (vì TarowNow có nạp/rút tiền).
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Register;

/// <summary>
/// Validator cho RegisterCommand, chặn rác dữ liệu ngay lớp ngoài cùng.
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            // Ngăn chặn ký tự đặc biệt như <script> hay ' OR 1=1;
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain alphanumeric characters and underscores.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(100).WithMessage("DisplayName cannot exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .Must(BeAtLeast18YearsOld).WithMessage("You must be at least 18 years old to register."); // Cổng kiểm duyệt Tuổi (Age Gate)

        RuleFor(x => x.HasConsented)
            .Equal(true).WithMessage("You must consent to the Terms of Service and Privacy Policy.");
    }

    /// <summary>
    /// Thuật toán kiểm tra số tuổi hiện tại dựa theo ngày sinh và ngày chạy hệ thống.
    /// Tính chính xác đến từng ngày (chứ không chỉ trừ năm).
    /// </summary>
    private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
    {
        var minAge = 18;
        var today = DateTime.UtcNow.Date;
        var diff = today.Year - dateOfBirth.Year;
        
        // Nếu năm nay sinh nhật chưa tới thì trừ đi 1 tuổi (vd sinh tháng 12 mà nay mới tháng 11).
        if (dateOfBirth.Date > today.AddYears(-diff)) 
            diff--;

        return diff >= minAge;
    }
}
