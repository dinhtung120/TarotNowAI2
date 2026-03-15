using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

public class SendEmailVerificationOtpCommandHandler : IRequestHandler<SendEmailVerificationOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender;

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

        // Tạo OTP mới
        var random = new Random();
        var otpCode = random.Next(100000, 999999).ToString();

        var otpEntity = new EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.VerifyEmail,
            expiryMinutes: 15
        );

        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        // Gửi tới Mock Email
        var subject = "TarotNow - Validate your target email address";
        var body = $"Hello {user.Username},\n\nYour Verification Code is: {otpCode}\n\nThis code will expire in 15 minutes. Please do not share this code.";
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
