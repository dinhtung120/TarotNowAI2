using TarotNow.Application.Common.Constants;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public partial class RefreshTokenCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Nâng cấp refresh token legacy chưa có session sang mô hình auth session theo thiết bị.
    /// </summary>
    private async Task EnsureLegacyTokenSessionBindingAsync(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (token is null || token.SessionId != Guid.Empty)
        {
            return;
        }

        var session = await _authSessionRepository.CreateAsync(
            token.UserId,
            request.DeviceId,
            request.UserAgentHash,
            HashValue(request.ClientIpAddress),
            cancellationToken);

        token.BindSession(session.Id);
        await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
    }

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
            User = _mapper.Map<UserProfileDto>(user)
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

        var effectiveSessionId = token.SessionId;
        if (effectiveSessionId == Guid.Empty)
        {
            var upgradedSession = await _authSessionRepository.CreateAsync(
                token.UserId,
                request.DeviceId,
                request.UserAgentHash,
                HashValue(request.ClientIpAddress),
                cancellationToken);
            token.BindSession(upgradedSession.Id);
            await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
            effectiveSessionId = upgradedSession.Id;
        }

        if (effectiveSessionId != Guid.Empty)
        {
            await _refreshTokenRepository.RevokeSessionAsync(
                effectiveSessionId,
                RefreshRevocationReasons.ReplayDetected,
                cancellationToken);
            await _authSessionRepository.RevokeAsync(effectiveSessionId, cancellationToken);
        }

        await _domainEventPublisher.PublishAsync(
            new RefreshTokenReplayDetectedDomainEvent
            {
                UserId = token.UserId,
                SessionId = effectiveSessionId,
                FamilyId = token.FamilyId,
                SourceIpHash = HashValue(request.ClientIpAddress)
            },
            cancellationToken);
    }

    private async Task TouchSessionAsync(
        Guid sessionId,
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (sessionId == Guid.Empty)
        {
            return;
        }

        await _authSessionRepository.TouchAsync(
            sessionId,
            HashValue(request.ClientIpAddress),
            request.UserAgentHash,
            cancellationToken);
    }

    private static string HashValue(string? raw)
    {
        var normalized = string.IsNullOrWhiteSpace(raw) ? "unknown" : raw.Trim();
        var bytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(normalized));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
