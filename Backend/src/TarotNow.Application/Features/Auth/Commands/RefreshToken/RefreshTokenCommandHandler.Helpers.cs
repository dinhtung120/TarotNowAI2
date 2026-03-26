using TarotNow.Application.Exceptions;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Domain.Entities;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public partial class RefreshTokenCommandHandler
{
    private async Task<RefreshTokenEntity> GetRefreshTokenOrThrowAsync(string token, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepository.GetByTokenAsync(token, cancellationToken)
            ?? throw new BusinessRuleException("INVALID_TOKEN", "Refresh token does not exist.");
    }

    private static void EnsureTokenMatches(RefreshTokenEntity token, string rawToken)
    {
        if (!token.MatchesToken(rawToken))
        {
            throw new BusinessRuleException("INVALID_TOKEN", "Refresh token is invalid.");
        }
    }

    private async Task EnsureTokenNotCompromisedAsync(RefreshTokenEntity token, CancellationToken cancellationToken)
    {
        if (!token.IsRevoked)
        {
            return;
        }

        await _refreshTokenRepository.RevokeAllByUserIdAsync(token.UserId, cancellationToken);
        throw new BusinessRuleException(
            "TOKEN_COMPROMISED",
            "Token reuse detected. All sessions have been revoked for security reasons. Please log in again.");
    }

    private static void EnsureTokenNotExpired(RefreshTokenEntity token)
    {
        if (token.IsExpired)
        {
            throw new BusinessRuleException("TOKEN_EXPIRED", "Refresh token has expired. Please log in again.");
        }
    }

    private async Task RotateTokenAsync(RefreshTokenEntity token, CancellationToken cancellationToken)
    {
        token.Revoke();
        await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
    }

    private static User EnsureUserIsActive(User? user)
    {
        if (user == null || user.Status == Domain.Enums.UserStatus.Banned || user.Status == Domain.Enums.UserStatus.Locked)
        {
            throw new BusinessRuleException("USER_BLOCKED", "User account is no longer active.");
        }

        return user;
    }

    private async Task<string> IssueRefreshTokenAsync(User user, string? clientIpAddress, CancellationToken cancellationToken)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        var entity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshToken,
            expiresAt: DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
            createdByIp: clientIpAddress ?? "unknown");

        await _refreshTokenRepository.AddAsync(entity, cancellationToken);
        return refreshToken;
    }

    private AuthResponse BuildAuthResponse(User user, string accessToken)
    {
        return new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresInMinutes = _jwtTokenSettings.AccessTokenExpiryMinutes,
            User = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Level = user.Level,
                Exp = user.Exp,
                Role = user.Role,
                Status = user.Status.ToString()
            }
        };
    }
}
