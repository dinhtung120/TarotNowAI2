using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using RefreshTokenEntity = TarotNow.Domain.Entities.RefreshToken;

namespace TarotNow.Application.Features.Auth.Commands.Login;

public partial class LoginCommandHandler
{
    private async Task<User?> GetUserByIdentityAsync(string emailOrUsername, CancellationToken cancellationToken)
    {
        return emailOrUsername.Contains('@')
            ? await _userRepository.GetByEmailAsync(emailOrUsername, cancellationToken)
            : await _userRepository.GetByUsernameAsync(emailOrUsername, cancellationToken);
    }

    private User EnsureValidCredentials(User? user, string rawPassword)
    {
        var validPassword = user != null && _passwordHasher.VerifyPassword(user.PasswordHash, rawPassword);
        if (!validPassword)
        {
            throw new BusinessRuleException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        return user!;
    }

    private static void EnsureUserCanLogin(User user)
    {
        if (user.Status == UserStatus.Pending)
        {
            throw new BusinessRuleException("USER_PENDING", "Please verify your email address to log in.");
        }

        if (user.Status == UserStatus.Banned || user.Status == UserStatus.Locked)
        {
            throw new BusinessRuleException("USER_BLOCKED", "Your account is temporarily locked or banned.");
        }
    }

    private async Task RehashPasswordIfNeededAsync(User user, string rawPassword, CancellationToken cancellationToken)
    {
        if (!_passwordHasher.NeedsRehash(user.PasswordHash))
        {
            return;
        }

        user.UpdatePassword(_passwordHasher.HashPassword(rawPassword));
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    private async Task<string> CreateRefreshTokenAsync(User user, string? clientIpAddress, CancellationToken cancellationToken)
    {
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshTokenEntity(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(_jwtTokenSettings.RefreshTokenExpiryDays),
            createdByIp: clientIpAddress ?? "unknown");

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        return refreshTokenString;
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
