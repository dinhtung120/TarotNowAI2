/*
 * ===================================================================
 * FILE: SendEmailVerificationOtpCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh yêu cầu "Gửi lại Mã OTP Mới".
 *   Sử dụng trong trường hợp User chưa nhận được email, hoặc Email gửi mã OTP 
 *   trước đó đã hết hạn (sau 15 phút).
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

/// <summary>
/// Yêu cầu gửi lại mã xác nhận cho Email tân binh.
/// Trả về Boolean (Luôn True để bưng bít thông tin hệ thống).
/// </summary>
public class SendEmailVerificationOtpCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
}
