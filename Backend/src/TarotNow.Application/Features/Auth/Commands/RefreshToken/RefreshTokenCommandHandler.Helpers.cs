using TarotNow.Application.Common.Constants;
using TarotNow.Application.Common.Mappings;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public partial class RefreshTokenCommandHandler
{
    /// <summary>
    /// Đảm bảo user còn hoạt động.
    /// </summary>
    private static User EnsureUserIsActive(User? user)
    {
        if (user == null
            || user.Status == Domain.Enums.UserStatus.Banned
            || user.Status == Domain.Enums.UserStatus.Locked)
        {
            throw new BusinessRuleException(AuthErrorCodes.UserBlocked, "User account is no longer active.");
        }

        return user;
    }

    /// <summary>
    /// Dựng auth response từ access token mới.
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

    /// <summary>
    /// Xử lý nhánh replay detected: revoke family/session và publish security event.
    /// </summary>
    private async Task HandleReplayDetectedAsync(
        RefreshRotateResult rotateResult,
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = rotateResult.CurrentToken;
        if (token is null)
        {
            return;
        }

        if (token.FamilyId != Guid.Empty)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(
                token.FamilyId,
                RefreshRevocationReasons.ReplayDetected,
                cancellationToken);
        }

        if (token.SessionId != Guid.Empty)
        {
            await _refreshTokenRepository.RevokeSessionAsync(
                token.SessionId,
                RefreshRevocationReasons.ReplayDetected,
                cancellationToken);
            await _authSessionRepository.RevokeAsync(token.SessionId, cancellationToken);
        }

        await _domainEventPublisher.PublishAsync(
            new RefreshTokenReplayDetectedDomainEvent
            {
                UserId = token.UserId,
                SessionId = token.SessionId,
                FamilyId = token.FamilyId,
                SourceIpHash = HashValue(request.ClientIpAddress)
            },
            cancellationToken);
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
