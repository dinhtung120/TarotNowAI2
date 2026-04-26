using TarotNow.Application.Exceptions;
using TarotNow.Application.Common.Constants;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public partial class LoginCommandHandlerRequestedDomainEventHandler
{
    private async Task<User> ValidateCredentialsAsync(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await GetUserByIdentityAsync(request.EmailOrUsername, cancellationToken);
        if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            await _domainEventPublisher.PublishAsync(
                BuildLoginFailedEvent(request, AuthErrorCodes.Unauthorized),
                cancellationToken);
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Invalid email/username or password.");
        }

        try
        {
            EnsureUserCanLogin(user);
        }
        catch (BusinessRuleException ex)
        {
            await _domainEventPublisher.PublishAsync(
                BuildLoginFailedEvent(request, ex.ErrorCode),
                cancellationToken);
            throw;
        }

        await RehashPasswordIfNeededAsync(user, request.Password, cancellationToken);
        return user;
    }

    private async Task<LoginSessionContext> CreateSessionContextAsync(
        User user,
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var ipHash = HashIpAddress(command.ClientIpAddress);
        var session = await _authSessionRepository.CreateAsync(
            user.Id,
            command.DeviceId,
            command.UserAgentHash,
            ipHash,
            cancellationToken);

        await _refreshTokenRepository.RevokeSessionAsync(
            session.Id,
            RefreshRevocationReasons.ManualRevoke,
            cancellationToken);

        return new LoginSessionContext(session.Id, ipHash);
    }

    private async Task<LoginResult> IssueLoginResultAsync(
        User user,
        LoginCommand request,
        LoginSessionContext sessionContext,
        CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GenerateAccessToken(user, sessionContext.SessionId, out _, out var accessTokenJti);
        var issuedRefreshToken = await CreateRefreshTokenAsync(user, sessionContext.SessionId, request, cancellationToken);
        var response = BuildAuthResponse(user, accessToken);

        await _domainEventPublisher.PublishAsync(
            new UserLoggedInDomainEvent
            {
                UserId = user.Id,
                SessionId = sessionContext.SessionId,
                DeviceId = request.DeviceId,
                UserAgentHash = request.UserAgentHash,
                IpHash = sessionContext.IpHash,
                AccessTokenJti = accessTokenJti
            },
            cancellationToken);

        return new LoginResult
        {
            Response = response,
            RefreshToken = issuedRefreshToken.RawToken,
            RefreshTokenExpiresAtUtc = issuedRefreshToken.ExpiresAtUtc
        };
    }

    /// <summary>
    /// Tìm user theo identity đầu vào (email hoặc username).
    /// Luồng xử lý: phân nhánh theo ký tự '@' để gọi repository tương ứng.
    /// </summary>
    private async Task<User?> GetUserByIdentityAsync(string emailOrUsername, CancellationToken cancellationToken)
    {
        return emailOrUsername.Contains('@')
            ? await _userRepository.GetByEmailAsync(emailOrUsername, cancellationToken)
            : await _userRepository.GetByUsernameAsync(emailOrUsername, cancellationToken);
    }

    /// <summary>
    /// Kiểm tra trạng thái tài khoản có được phép đăng nhập hay không.
    /// Luồng xử lý: chặn tài khoản pending và blocked theo chính sách bảo mật.
    /// </summary>
    private static void EnsureUserCanLogin(User user)
    {
        if (user.Status == UserStatus.Pending)
        {
            // Pending cần xác minh email trước khi cho phép đăng nhập.
            throw new BusinessRuleException(AuthErrorCodes.Unauthorized, "Please verify your email address to log in.");
        }

        if (user.Status == UserStatus.Banned || user.Status == UserStatus.Locked)
        {
            // Banned/Locked bị chặn đăng nhập để bảo vệ hệ thống và người dùng.
            throw new BusinessRuleException(AuthErrorCodes.UserBlocked, "Your account is temporarily locked or banned.");
        }
    }

    /// <summary>
    /// Rehash mật khẩu nếu hash hiện tại đã lỗi thời theo chính sách mới.
    /// Luồng xử lý: kiểm tra NeedsRehash, nếu true thì băm lại mật khẩu và cập nhật user.
    /// </summary>
    private async Task RehashPasswordIfNeededAsync(User user, string rawPassword, CancellationToken cancellationToken)
    {
        if (!_passwordHasher.NeedsRehash(user.PasswordHash))
        {
            // Hash hiện tại còn đạt chuẩn nên không cần cập nhật.
            return;
        }

        // Đổi state hash để tăng cường bảo mật mà không ép người dùng đổi mật khẩu thủ công.
        user.UpdatePassword(_passwordHasher.HashPassword(rawPassword));
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    /// <summary>
    /// Tạo refresh token mới và lưu vào repository.
    /// Luồng xử lý: sinh token ngẫu nhiên, tạo entity với hạn dùng, persist vào kho dữ liệu.
    /// </summary>
    private async Task<IssuedRefreshToken> CreateRefreshTokenAsync(
        User user,
        Guid sessionId,
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var expiresAtUtc = DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays);
        var refreshTokenEntity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: expiresAtUtc,
            createdByIp: command.ClientIpAddress,
            sessionId: sessionId,
            familyId: Guid.NewGuid(),
            parentTokenId: null,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);

        // Lưu refresh token để phục vụ revoke/rotate trong các vòng refresh kế tiếp.
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        return new IssuedRefreshToken(refreshTokenString, expiresAtUtc);
    }

    /// <summary>
    /// Dựng phản hồi auth chuẩn gồm access token và hồ sơ user.
    /// Luồng xử lý: gán token + expiry, map user entity sang profile DTO.
    /// </summary>
    private AuthResponse BuildAuthResponse(User user, string accessToken)
    {
        return new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresInSeconds = _jwtTokenSettings.AccessTokenExpiryMinutes * 60,
            User = _mapper.Map<UserProfileDto>(user)
        };
    }

    private static LoginFailedDomainEvent BuildLoginFailedEvent(LoginCommand request, string reasonCode)
    {
        return new LoginFailedDomainEvent
        {
            IdentityHash = HashIdentity(request.EmailOrUsername),
            IpHash = HashIpAddress(request.ClientIpAddress),
            ReasonCode = reasonCode
        };
    }

    private static string HashIdentity(string? identity)
    {
        var normalized = string.IsNullOrWhiteSpace(identity)
            ? "unknown"
            : identity.Trim().ToLowerInvariant();
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string HashIpAddress(string? ipAddress)
    {
        var normalized = string.IsNullOrWhiteSpace(ipAddress)
            ? "unknown"
            : ipAddress.Trim();
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private readonly record struct IssuedRefreshToken(string RawToken, DateTime ExpiresAtUtc);
    private readonly record struct LoginSessionContext(Guid SessionId, string IpHash);
}
