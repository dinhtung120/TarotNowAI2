using MediatR;
using Microsoft.Extensions.Configuration;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Exceptions;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Xử lý nghiệp vụ Đăng nhập:
/// 1. Tìm User bằng Email/Username
/// 2. Xác minh mật khẩu
/// 3. Cấp phát JWT Access Token
/// 4. Tạo Refresh Token mới và lưu vào db
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, (AuthResponse Response, string RefreshToken)>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    // Cần Interface riêng cho RefreshToken repository, tạm thời ta sẽ inject sau.
    // Tạm thời nếu _ApplicationDbContext_ bị lộ ở Application thì không đúng chuẩn Clean Architecture.
    // Lát sau sẽ tạo IRefreshTokenRepository ở Domain.
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LoginCommandHandler(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService, 
        IConfiguration configuration,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<(AuthResponse Response, string RefreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Tìm kiếm User bằng Email hoặc Username
        User? user = null;
        if (request.EmailOrUsername.Contains('@'))
        {
            user = await _userRepository.GetByEmailAsync(request.EmailOrUsername, cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByUsernameAsync(request.EmailOrUsername, cancellationToken);
        }

        if (user == null)
        {
            throw new DomainException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        // 2. Validate Password
        if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new DomainException("INVALID_CREDENTIALS", "Invalid email/username or password.");
        }

        // Kiểm tra Status (Lock, Ban, Pending)
        if (user.Status == UserStatus.Pending)
        {
            throw new DomainException("USER_PENDING", "Please verify your email address to log in.");
        }
        else if (user.Status == UserStatus.Banned || user.Status == UserStatus.Locked)
        {
            throw new DomainException("USER_BLOCKED", "Your account is temporarily locked or banned.");
        }

        // 3. Generate Access Token
        var accessToken = _tokenService.GenerateAccessToken(user);

        // 4. Generate Refresh Token
        var refreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = ResolveRefreshTokenExpiryDays();
        var refreshTokenEntity = new TarotNow.Domain.Entities.RefreshToken(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
            createdByIp: request.ClientIpAddress
        );

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        // 5. Mappings
        var resp = new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresInMinutes = ResolveAccessTokenExpiryMinutes(),
            User = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Level = user.Level,
                Role = user.Role,
                Status = user.Status.ToString()
            }
        };

        return (resp, refreshTokenString);
    }

    private int ResolveAccessTokenExpiryMinutes()
    {
        var configured = _configuration["Jwt:ExpiryMinutes"]
                         ?? _configuration["Jwt:AccessTokenExpirationMinutes"];

        return int.TryParse(configured, out var value) && value > 0
            ? value
            : 15;
    }

    private int ResolveRefreshTokenExpiryDays()
    {
        var configured = _configuration["Jwt:RefreshExpiryDays"]
                         ?? _configuration["Jwt:RefreshTokenExpirationDays"];

        return int.TryParse(configured, out var value) && value > 0
            ? value
            : 7;
    }
}
