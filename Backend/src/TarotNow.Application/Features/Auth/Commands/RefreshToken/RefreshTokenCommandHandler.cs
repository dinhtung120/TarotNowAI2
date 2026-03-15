using MediatR;
using Microsoft.Extensions.Configuration;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities; // Needed for refresh token entity 

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (AuthResponse Response, string NewRefreshToken)>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<(AuthResponse Response, string NewRefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);

        if (existingToken == null)
        {
            throw new DomainException("INVALID_TOKEN", "Refresh token does not exist.");
        }

        // Reuse Detection: Nếu token đã bị thu hồi mà vẫn được sử dụng để xin token mới -> Fraud Alert!
        if (existingToken.IsRevoked)
        {
            // Thu hồi toàn bộ token của User vì có khả năng lộ thông tin
            await _refreshTokenRepository.RevokeAllByUserIdAsync(existingToken.UserId, cancellationToken);
            throw new DomainException("TOKEN_COMPROMISED", "Token reuse detected. All sessions have been revoked for security reasons. Please log in again.");
        }

        if (existingToken.IsExpired)
        {
            throw new DomainException("TOKEN_EXPIRED", "Refresh token has expired. Please log in again.");
        }

        // Token hợp lệ, ta sẽ thu hồi token cũ ngay lập tức (Rotation)
        existingToken.Revoke();
        await _refreshTokenRepository.UpdateAsync(existingToken, cancellationToken);

        var user = existingToken.User;
        if (user == null || user.Status == Domain.Enums.UserStatus.Banned || user.Status == Domain.Enums.UserStatus.Locked)
        {
            throw new DomainException("USER_BLOCKED", "User account is no longer active.");
        }

        // Generate cặp Token mới
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();
        
        var refreshTokenExpiryDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
        var newRefreshTokenEntity = new TarotNow.Domain.Entities.RefreshToken(
            userId: user.Id,
            token: newRefreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
            createdByIp: request.ClientIpAddress
        );

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        var resp = new AuthResponse
        {
            AccessToken = newAccessToken,
            ExpiresInMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15"),
            User = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Level = user.Level,
                Status = user.Status.ToString()
            }
        };

        return (resp, newRefreshTokenString);
    }
}
