using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.ResetPassword;

// Handler xử lý đặt lại mật khẩu bằng OTP email.
public class ResetPasswordCommandExecutor : ICommandExecutionExecutor<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailOtpRepository _emailOtpRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IAuthSessionRepository _authSessionRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler reset password.
    /// Luồng xử lý: nhận user repo, OTP repo, password hasher và refresh token repo.
    /// </summary>
    public ResetPasswordCommandExecutor(
        IUserRepository userRepository,
        IEmailOtpRepository emailOtpRepository,
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,
        IAuthSessionRepository authSessionRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _userRepository = userRepository;
        _emailOtpRepository = emailOtpRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _authSessionRepository = authSessionRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <summary>
    /// Xử lý command đặt lại mật khẩu.
    /// Luồng xử lý: kiểm tra user + OTP hợp lệ, cập nhật mật khẩu mới, đánh dấu OTP đã dùng, revoke toàn bộ refresh token.
    /// </summary>
    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            // Trả lỗi chung cho email/OTP để tránh lộ tài khoản có tồn tại hay không.
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        var latestOtp = await _emailOtpRepository.GetLatestActiveOtpAsync(user.Id, OtpType.ResetPassword, cancellationToken);
        if (latestOtp == null || !latestOtp.VerifyCode(request.OtpCode))
        {
            // OTP sai hoặc hết hiệu lực: dừng luồng reset ngay.
            throw new BusinessRuleException("INVALID_OTP", "Invalid email or OTP code.");
        }

        // Băm và cập nhật mật khẩu mới để không lưu dữ liệu nhạy cảm dạng thô.
        var newHash = _passwordHasher.HashPassword(request.NewPassword);
        user.UpdatePassword(newHash);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Đánh dấu OTP đã dùng để ngăn tái sử dụng.
        latestOtp.MarkAsUsed();
        await _emailOtpRepository.UpdateAsync(latestOtp, cancellationToken);

        // Rule bảo mật: revoke toàn bộ refresh token hiện có sau khi đổi mật khẩu.
        var activeSessionIds = await _authSessionRepository.GetActiveSessionIdsByUserAsync(user.Id, cancellationToken);
        await _refreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await _authSessionRepository.RevokeAllByUserAsync(user.Id, cancellationToken);

        await _domainEventPublisher.PublishAsync(
            new UserLoggedOutDomainEvent
            {
                UserId = user.Id,
                RevokeAll = true,
                SessionIds = activeSessionIds,
                Reason = RefreshRevocationReasons.ManualRevoke
            },
            cancellationToken);

        return true;
    }
}
