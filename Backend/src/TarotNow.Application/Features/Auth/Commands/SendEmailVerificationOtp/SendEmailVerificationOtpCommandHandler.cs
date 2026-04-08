using MediatR;
using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

// Handler gửi OTP xác minh email cho tài khoản chưa active.
public class SendEmailVerificationOtpCommandHandler : IRequestHandler<SendEmailVerificationOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender;

    /// <summary>
    /// Khởi tạo handler gửi OTP verify email.
    /// Luồng xử lý: nhận user repo, OTP repo và email sender để tạo/gửi mã xác thực.
    /// </summary>
    public SendEmailVerificationOtpCommandHandler(
        IUserRepository userRepository,
        IEmailOtpRepository emailOtpRepository,
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Xử lý command gửi OTP verify email.
    /// Luồng xử lý: tìm user theo email, bỏ qua user null/đã active, tạo OTP mới, lưu DB và gửi email.
    /// </summary>
    public async Task<bool> Handle(SendEmailVerificationOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Rule bảo mật: luôn trả true để không lộ email có tồn tại hay không.
            return true;
        }

        if (user.Status == UserStatus.Active)
        {
            // Account đã active thì không cần gửi lại OTP xác minh email.
            return true;
        }

        // Sinh OTP 6 chữ số cho mục đích xác minh email.
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        var otpEntity = new EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.VerifyEmail,
            expiryMinutes: 15);

        // Persist OTP trước để đảm bảo mã gửi đi luôn có bản ghi hợp lệ.
        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        var subject = "TarotNow - Validate your target email address";
        var body = $"Hello {user.Username},\n\nYour Verification Code is: {otpCode}\n\nThis code will expire in 15 minutes. Please do not share this code.";
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
