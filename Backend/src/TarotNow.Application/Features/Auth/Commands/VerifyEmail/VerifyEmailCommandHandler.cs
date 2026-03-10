using MediatR;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Interfaces;

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
        
        if (latestOtp == null || latestOtp.OtpCode != request.OtpCode)
        {
            throw new DomainException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Đánh dấu OTP đã sử dụng
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // Kích hoạt User (Đổi trạng thái từ Pending -> Active)
        user.Activate();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // TODO: (P1-AUTH-BE-2.3) Cộng +5 Gold cho user khi kích hoạt thành công. (Thực hiện qua Database Trigger hoặc Handler sau)
        // Hiện tại tính năng Ví/Gold chưa có, sẽ bổ sung trong Phase kế tiếp

        return true;
    }
}
