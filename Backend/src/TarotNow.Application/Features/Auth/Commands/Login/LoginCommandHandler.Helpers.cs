using TarotNow.Application.Exceptions;
using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.Mappings;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public partial class LoginCommandHandler
{
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
    private async Task<string> CreateRefreshTokenAsync(
        User user,
        Guid sessionId,
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
            createdByIp: command.ClientIpAddress,
            sessionId: sessionId,
            familyId: Guid.NewGuid(),
            parentTokenId: null,
            createdDeviceId: command.DeviceId,
            createdUserAgentHash: command.UserAgentHash);

        // Lưu refresh token để phục vụ revoke/rotate trong các vòng refresh kế tiếp.
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        return refreshTokenString;
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
            User = user.ToUserProfileDto()
        };
    }

    private static LoginFailedDomainEvent BuildLoginFailedEvent(LoginCommand request, string reasonCode)
    {
        return new LoginFailedDomainEvent
        {
            IdentityHash = HashValue(request.EmailOrUsername),
            IpHash = HashValue(request.ClientIpAddress),
            ReasonCode = reasonCode
        };
    }

    private async Task EnsureLoginThrottleNotExceededAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        var identityHash = HashValue(request.EmailOrUsername);
        var ipHash = HashValue(request.ClientIpAddress);
        var identityCount = await _cacheService.GetAsync<long>(BuildLoginIdentityFailureKey(identityHash), cancellationToken);
        if (identityCount >= AuthSecurityPolicyConstants.LoginIdentityFailureLimit)
        {
            throw new BusinessRuleException(AuthErrorCodes.RateLimited, "Too many failed login attempts. Please try again later.");
        }

        var ipCount = await _cacheService.GetAsync<long>(BuildLoginIpFailureKey(ipHash), cancellationToken);
        if (ipCount >= AuthSecurityPolicyConstants.LoginIpFailureLimit)
        {
            throw new BusinessRuleException(AuthErrorCodes.RateLimited, "Too many failed login attempts. Please try again later.");
        }
    }

    private async Task ClearLoginFailureCountersAsync(LoginCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync(BuildLoginIdentityFailureKey(HashValue(request.EmailOrUsername)), cancellationToken);
        await _cacheService.RemoveAsync(BuildLoginIpFailureKey(HashValue(request.ClientIpAddress)), cancellationToken);
    }

    private static string BuildLoginIdentityFailureKey(string identityHash)
    {
        return $"auth:login-fail:identity:{identityHash}";
    }

    private static string BuildLoginIpFailureKey(string ipHash)
    {
        return $"auth:login-fail:ip:{ipHash}";
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
