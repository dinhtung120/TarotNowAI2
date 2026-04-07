

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository, 
        IEmailOtpRepository emailOtpRepository, 
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        
        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, cancellationToken);
        
        
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        
        var newHash = _passwordHasher.HashPassword(request.NewPassword);
        user.UpdatePassword(newHash);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        
        
        
        
        await _refreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);

        return true;
    }
}
