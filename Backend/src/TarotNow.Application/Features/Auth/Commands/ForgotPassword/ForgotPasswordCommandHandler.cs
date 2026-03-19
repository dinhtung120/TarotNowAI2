/*
 * ===================================================================
 * FILE: ForgotPasswordCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.ForgotPassword
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý business logic cho tính năng Quên Mật Khẩu.
 *
 * LUỒNG HOẠT ĐỘNG (WORKFLOW):
 *   1. Tìm user theo Email.
 *   2. Nếu user KHÔNG tồn tại → VẪN trả về true (chống dò email).
 *   3. Nếu tồn tại → Tạo số OTP ngẫu nhiên (6 chữ số).
 *   4. Lưu OTP vào database (EmailOtp table) với thời hạn (VD: 15 phút).
 *   5. Gửi email chứa mã OTP này cho user.
 *   6. Trả về true cho Controller.
 * ===================================================================
 */

using MediatR;
using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

/// <summary>
/// Handler thực thi logic của ForgotPasswordCommand.
/// </summary>
public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender; // Dịch vụ gửi email (vd: SMTP, SendGrid...)

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository, 
        IEmailOtpRepository emailOtpRepository, 
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _emailSender = emailSender;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm user trong database bằng Email
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        // 2. BẢO MẬT: Chống User Enumeration Attack
        // Thay vì throw Exception ("Email không tồn tại"), ta im lặng trả về true.
        // Hacker không thể phân biệt được email này có ở hệ thống hay không.
        if (user == null)
        {
            return true;
        }

        /* 
         * 3. Tạo mã OTP (One-Time Password)
         * Sử dụng RandomNumberGenerator.GetInt32 thay vì Random() cấp mầm (seed) 
         * để đảm bảo tính NGẪU NHIÊN BẢO MẬT MẬT MÃ (Cryptographically Secure), 
         * hacker không thể đoán được chuỗi OTP.
         */
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        // 4. Khởi tạo Entity EmailOtp để lưu vào database
        var otpEntity = new TarotNow.Domain.Entities.EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.ResetPassword, // Phân biệt với OTP xác nhận đăng ký
            expiryMinutes: 15 // Hết hạn trong 15 phút
        );

        // Lưu bản ghi OTP vào PostgreSQL
        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        // 5. Build nội dung và gửi Email
        var subject = "TarotNow - Reset Your Password";
        var body = $"Hello {user.Username},\n\nWe received a request to reset your password. Your reset OTP code is: {otpCode}\n\nThis code will expire in 15 minutes. If you did not request this change, please safely ignore this email.";
        
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        // 6. Hoàn thành
        return true;
    }
}
