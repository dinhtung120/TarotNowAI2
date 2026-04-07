

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository, IEmailOtpRepository emailOtpRepository)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        
        if (user.Status == UserStatus.Active)
        {
            throw new BusinessRuleException("EMAIL_ALREADY_VERIFIED", "This email address is already verified.");
        }

        
        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, cancellationToken);
        
        
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        
        user.Activate();

        
        
        
        
        
        user.Wallet.Credit(CurrencyType.Gold, 5, TransactionType.RegisterBonus);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        return true;
    }
}
