/*
 * ===================================================================
 * FILE: SendEmailVerificationOtpCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler thực thi logic tạo Mã OTP Số hoá ngẫu nhiên và châm ngòi gửi Email.
 *
 * TÍNH AN NINH (SECURITY THOUGHTS):
 *   Giống luồng Quên Mật Khẩu, cổng này trả về chữ OK (true) ngay lập tức 
 *   bất kể Email đúng hay sai, thậm chí đã xác minh xong vẫn báo OK.
 *   Lý do: Không để Hacker nhặt được thông điệp "Email này chưa gởi OTP bao giờ" 
 *   hoặc "Email không tồn tại".
 * ===================================================================
 */

using MediatR;
using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

/// <summary>
/// Động cơ tạo và phóng OTP Email.
/// </summary>
public class SendEmailVerificationOtpCommandHandler : IRequestHandler<SendEmailVerificationOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender; // Inteface SMTP (SendGrid, MailKit, v.v...)

    public SendEmailVerificationOtpCommandHandler(
        IUserRepository userRepository, 
        IEmailOtpRepository emailOtpRepository, 
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _emailSender = emailSender;
    }

    public async Task<bool> Handle(SendEmailVerificationOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả về true (ngay cả khi User không tồn tại) để ngăn chặn kẻ xấu dò tìm Email (Email enumeration)
            return true;
        }

        if (user.Status == UserStatus.Active)
        {
            // Tài khoản đã verify, trả về true luôn giả lập thành công để tránh lộ thông tin thiết kế
            return true;
        }

        // Tạo OTP mới (Luôn dùng RNG Cryptographic thay cho Math.Random)
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        // Đóng gói cấu trúc OTP (Id cha, Loại Validate, Thời gian sống)
        var otpEntity = new EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.VerifyEmail, // Cần đánh dấu khác với ResetPassword
            expiryMinutes: 15
        );

        // Lưu vào thùng chứa DB (PostgreSQL)
        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        // Gửi qua SMTP Relay (hoặc AWS SES, SendGrid, Resend)
        var subject = "TarotNow - Validate your target email address";
        var body = $"Hello {user.Username},\n\nYour Verification Code is: {otpCode}\n\nThis code will expire in 15 minutes. Please do not share this code.";
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
