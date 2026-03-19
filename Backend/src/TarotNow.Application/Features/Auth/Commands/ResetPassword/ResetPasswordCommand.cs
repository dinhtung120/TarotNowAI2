/*
 * ===================================================================
 * FILE: ResetPasswordCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.ResetPassword
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh thực hiện hành vi Đặt Lại Mật Khẩu (Bước 2 của Quên Mật Khẩu).
 *   Nhận Email, Mã xác nhận OTP (đã gửi qua mail trước đó) và Mật Khẩu Mới.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

/// <summary>
/// Nơi chứa dữ liệu cho yêu cầu cấp tài khoản mật mã mới qua luồng Forgot Password Email OTP.
/// </summary>
public class ResetPasswordCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
