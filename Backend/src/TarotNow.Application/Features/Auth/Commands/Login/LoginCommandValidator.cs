/*
 * ===================================================================
 * FILE: LoginCommandValidator.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Login
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lớp kiểm tra (Validation) đầu vào cho LoginCommand.
 *   Sử dụng thư viện FluentValidation để chặn các Request thiếu thông tin ngay từ "vòng gửi xe", 
 *   tránh lãng phí tài nguyên gọi Database vô ích.
 *
 * LUỒNG HOẠT ĐỘNG PIPELINE MEDIATR:
 *   Client gọi API -> Controller -> MediatR -> ValidationBehavior -> [LoginCommandValidator] 
 *   -> Kiểm tra (nếu có lỗi thì quăng ValidationException HTTP 400) -> [LoginCommandHandler]
 * ===================================================================
 */

using FluentValidation;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Chứa bộ rule (quy định) kiểm soát dữ liệu của LoginCommand.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        // 1. Kiểm tra trường Tên đăng nhập / Email
        // Yêu cầu (Rule): Không được để trống.
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("Email or Username is required.");

        // 2. Kiểm tra trường Mật khẩu
        // Yêu cầu (Rule): Không được để trống.
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        // (Lưu ý: Không kiểm tra độ mạnh yếu password ở khâu Login. 
        // Logic kiểm tra độ phức tạp mật khẩu chỉ nằm ở khâu Đăng Ký (Register) 
        // hoặc Đổi Mật Khẩu (ChangePassword)).
    }
}
