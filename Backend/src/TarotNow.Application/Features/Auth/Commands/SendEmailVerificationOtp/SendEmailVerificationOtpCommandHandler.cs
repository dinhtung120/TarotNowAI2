

using MediatR;
using System.Security.Cryptography;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

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
            
            return true;
        }

        if (user.Status == UserStatus.Active)
        {
            
            return true;
        }

        
        var otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

        
        var otpEntity = new EmailOtp(
            userId: user.Id,
            otpCode: otpCode,
            type: OtpType.VerifyEmail, 
            expiryMinutes: 15
        );

        
        await _emailOtpRepository.AddAsync(otpEntity, cancellationToken);

        
        var subject = "TarotNow - Validate your target email address";
        var body = $"Hello {user.Username},\n\nYour Verification Code is: {otpCode}\n\nThis code will expire in 15 minutes. Please do not share this code.";
        await _emailSender.SendEmailAsync(user.Email, subject, body, cancellationToken);

        return true;
    }
}
