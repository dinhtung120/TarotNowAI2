using TarotNow.Application.Exceptions;
using TarotNow.Application.Common.Mappings;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public partial class RefreshTokenCommandHandler
{
    /// <summary>
    /// Lấy refresh token entity theo chuỗi token hoặc ném lỗi nếu không tồn tại.
    /// Luồng xử lý: truy vấn repository theo token, fallback ném BusinessRuleException INVALID_TOKEN.
    /// </summary>
    private async Task<RefreshTokenEntity> GetRefreshTokenOrThrowAsync(string token, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepository.GetByTokenAsync(token, cancellationToken)
            ?? throw new BusinessRuleException("INVALID_TOKEN", "Refresh token does not exist.");
    }

    /// <summary>
    /// Kiểm tra chuỗi token gửi lên có khớp entity đã tải hay không.
    /// Luồng xử lý: gọi MatchesToken và ném INVALID_TOKEN nếu không khớp.
    /// </summary>
    private static void EnsureTokenMatches(RefreshTokenEntity token, string rawToken)
    {
        if (!token.MatchesToken(rawToken))
        {
            // Chặn token giả mạo hoặc token cũ không còn khớp.
            throw new BusinessRuleException("INVALID_TOKEN", "Refresh token is invalid.");
        }
    }

    /// <summary>
    /// Kiểm tra token có dấu hiệu bị tái sử dụng sau khi revoke hay không.
    /// Luồng xử lý: nếu token revoked thì revoke toàn bộ token user và ném TOKEN_COMPROMISED.
    /// </summary>
    private async Task EnsureTokenNotCompromisedAsync(RefreshTokenEntity token, CancellationToken cancellationToken)
    {
        if (!token.IsRevoked)
        {
            // Token chưa bị revoke thì tiếp tục luồng refresh bình thường.
            return;
        }

        // Rule bảo mật: phát hiện reuse token revoked thì thu hồi toàn bộ phiên của user.
        await _refreshTokenRepository.RevokeAllByUserIdAsync(token.UserId, cancellationToken);
        throw new BusinessRuleException(
            "TOKEN_COMPROMISED",
            "Token reuse detected. All sessions have been revoked for security reasons. Please log in again.");
    }

    /// <summary>
    /// Kiểm tra refresh token còn hạn sử dụng.
    /// Luồng xử lý: ném TOKEN_EXPIRED nếu token đã hết hạn.
    /// </summary>
    private static void EnsureTokenNotExpired(RefreshTokenEntity token)
    {
        if (token.IsExpired)
        {
            throw new BusinessRuleException("TOKEN_EXPIRED", "Refresh token has expired. Please log in again.");
        }
    }

    /// <summary>
    /// Thu hồi refresh token cũ trong cơ chế rotate token.
    /// Luồng xử lý: mark token revoked và cập nhật entity vào repository.
    /// </summary>
    private async Task RotateTokenAsync(RefreshTokenEntity token, CancellationToken cancellationToken)
    {
        token.Revoke();
        await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
    }

    /// <summary>
    /// Đảm bảo user gắn với token vẫn ở trạng thái hoạt động.
    /// Luồng xử lý: chặn null/banned/locked và trả user hợp lệ cho bước cấp token mới.
    /// </summary>
    private static User EnsureUserIsActive(User? user)
    {
        if (user == null || user.Status == Domain.Enums.UserStatus.Banned || user.Status == Domain.Enums.UserStatus.Locked)
        {
            // Chặn cấp token mới cho tài khoản đã bị vô hiệu hóa.
            throw new BusinessRuleException("USER_BLOCKED", "User account is no longer active.");
        }

        return user;
    }

    /// <summary>
    /// Cấp refresh token mới và lưu vào repository.
    /// Luồng xử lý: sinh token ngẫu nhiên, tạo entity với expiry/ip, persist và trả chuỗi token.
    /// </summary>
    private async Task<string> IssueRefreshTokenAsync(User user, string? clientIpAddress, CancellationToken cancellationToken)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        var entity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshToken,
            expiresAt: DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
            createdByIp: clientIpAddress ?? "unknown");

        // Lưu token mới để phục vụ kiểm tra vòng refresh tiếp theo.
        await _refreshTokenRepository.AddAsync(entity, cancellationToken);
        return refreshToken;
    }

    /// <summary>
    /// Dựng phản hồi auth từ user và access token mới.
    /// Luồng xử lý: map user sang profile DTO và gắn thời gian hết hạn access token.
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
