using MediatR;
using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

// Handler khởi tạo luồng quên mật khẩu bằng OTP qua email.
public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender;

    /// <summary>
    /// Khởi tạo handler forgot password.
    /// Luồng xử lý: nhận user repo, OTP repo và email sender để tạo OTP và gửi email.
    /// </summary>
    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IEmailOtpRepository emailOtpRepository,
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Xử lý yêu cầu quên mật khẩu.
    /// Luồng xử lý: tìm user theo email, nếu tồn tại thì tạo OTP + lưu DB + gửi email, luôn trả true để tránh lộ thông tin account.
    /// </summary>
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả true ngay cả khi email không tồn tại để tránh lộ thông tin account (email enumeration).
            return true;
        }

        // Sinh OTP 6 chữ số ngẫu nhiên cho luồng reset password.
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        var otpEntity = new TarotNow.Domain.Entities.EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.ResetPassword,
            expiryMinutes: 15);

        // Lưu OTP trước khi gửi email để bảo đảm mã trong email luôn có bản ghi hợp lệ.
        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        var subject = "TarotNow - Reset Your Password";
        var body = $"Hello {user.Username},\n\nWe received a request to reset your password. Your reset OTP code is: {otpCode}\n\nThis code will expire in 15 minutes. If you did not request this change, please safely ignore this email.";

        // Gửi OTP đến email đã đăng ký để hoàn tất bước xác thực reset.
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
