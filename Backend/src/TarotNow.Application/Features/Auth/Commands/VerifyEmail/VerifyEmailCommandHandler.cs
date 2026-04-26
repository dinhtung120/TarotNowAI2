using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.VerifyEmail;

// Handler xác minh email và kích hoạt tài khoản.
public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler verify email.
    /// Luồng xử lý: nhận user repository và OTP repository để xác minh mã và cập nhật trạng thái account.
    /// </summary>
    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        IEmailOtpRepository emailOtpRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command xác minh email.
    /// Luồng xử lý: kiểm tra user tồn tại/chưa active, kiểm tra OTP hợp lệ, đánh dấu OTP đã dùng, kích hoạt user và cộng thưởng.
    /// </summary>
    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả lỗi chung để tránh lộ thông tin account tồn tại.
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        if (user.Status == UserStatus.Active)
        {
            // Edge case xác minh lặp cho account đã active.
            throw new BusinessRuleException("EMAIL_ALREADY_VERIFIED", "This email address is already verified.");
        }

        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.VerifyEmail, cancellationToken);
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            // OTP sai/hết hạn thì chặn kích hoạt tài khoản.
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Đánh dấu OTP đã dùng để chống replay.
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // Kích hoạt account sau khi OTP hợp lệ.
        user.Activate();

        // Thưởng vàng chào mừng khi user xác minh email thành công.
        user.Wallet.Credit(CurrencyType.Gold, 5, TransactionType.RegisterBonus);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _domainEventPublisher.PublishAsync(
            new Domain.Events.MoneyChangedDomainEvent
            {
                UserId = user.Id,
                Currency = CurrencyType.Gold,
                ChangeType = TransactionType.RegisterBonus,
                DeltaAmount = 5,
                ReferenceId = $"verify_email_{user.Id:N}"
            },
            cancellationToken);

        return true;
    }
}
