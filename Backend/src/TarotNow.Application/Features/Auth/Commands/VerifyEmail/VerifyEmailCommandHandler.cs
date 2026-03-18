using MediatR;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
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
            throw new DomainException("INVALID_OTP", "Invalid email or OTP code.");
        }

        if (user.Status == UserStatus.Active)
        {
            throw new DomainException("EMAIL_ALREADY_VERIFIED", "This email address is already verified.");
        }

        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, cancellationToken);
        
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            throw new DomainException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Đánh dấu OTP đã sử dụng
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // Kích hoạt User (Đổi trạng thái từ Pending -> Active)
        user.Activate();

        // Cộng +5 Gold cho user khi kích hoạt thành công (Phase 1.1 Test 2.2).
        // Đây là phần thưởng khuyến khích user verify email.
        user.Wallet.Credit(CurrencyType.Gold, 5, TransactionType.RegisterBonus);
        
        await _userRepository.UpdateAsync(user, cancellationToken);

        return true;
    }
}
