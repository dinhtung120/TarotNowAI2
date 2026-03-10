using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IEmailSender _emailSender;

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
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả về true để ngăn tấn công dò tìm theo chuẩn bảo mật, không báo người dùng nếu email tồn tại hay không
            return true;
        }

        var random = new Random();
        var otpCode = random.Next(100000, 999999).ToString();

        var otpEntity = new TarotNow.Domain.Entities.EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.ResetPassword,
            expiryMinutes: 15
        );

        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        var subject = "TarotNow - Reset Your Password";
        var body = $"Hello {user.Username},\n\nWe received a request to reset your password. Your reset OTP code is: {otpCode}\n\nThis code will expire in 15 minutes. If you did not request this change, please safely ignore this email.";
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
