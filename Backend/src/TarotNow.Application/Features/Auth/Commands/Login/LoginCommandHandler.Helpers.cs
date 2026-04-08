using TarotNow.Application.Exceptions;
using TarotNow.Application.Common.Mappings;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
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
    /// Kiểm tra credential hợp lệ (user tồn tại và mật khẩu đúng).
    /// Luồng xử lý: verify password hash, ném BusinessRuleException nếu không hợp lệ.
    /// </summary>
    private User EnsureValidCredentials(User? user, string rawPassword)
    {
        var validPassword = user != null && _passwordHasher.VerifyPassword(user.PasswordHash, rawPassword);
        if (!validPassword)
        {
            // Trả lỗi chung để không lộ thông tin email/username có tồn tại hay không.
            throw new BusinessRuleException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        return user!;
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
            throw new BusinessRuleException("USER_PENDING", "Please verify your email address to log in.");
        }

        if (user.Status == UserStatus.Banned || user.Status == UserStatus.Locked)
        {
            // Banned/Locked bị chặn đăng nhập để bảo vệ hệ thống và người dùng.
            throw new BusinessRuleException("USER_BLOCKED", "Your account is temporarily locked or banned.");
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
    private async Task<string> CreateRefreshTokenAsync(User user, string? clientIpAddress, CancellationToken cancellationToken)
    {
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
            createdByIp: clientIpAddress ?? "unknown");

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
            ExpiresIn = _jwtTokenSettings.AccessTokenExpiryMinutes,
            User = user.ToUserProfileDto()
        };
    }
}
