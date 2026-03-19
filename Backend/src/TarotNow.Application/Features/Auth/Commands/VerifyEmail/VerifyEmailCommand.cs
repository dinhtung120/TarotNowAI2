/*
 * ===================================================================
 * FILE: VerifyEmailCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.VerifyEmail
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh Nhận diện Yêu cầu Xác Thực Email.
 *   Khi User đăng ký xong, hệ thống gửi dãy 6 số OTP vào Email của họ.
 *   User nhập mã OTP đó lên màn hình Frontend -> Kích hoạt API này.
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

/// <summary>
/// Chứa thông tin OTP để hệ thống xác thực người dùng sở hữu địa chỉ Email thật.
/// </summary>
public class VerifyEmailCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Mã Code 6 chữ số gửi qua thư. Hiệu lực ngắn hạn (thường là 15 phút).
    /// </summary>
    public string OtpCode { get; set; } = string.Empty;
}
