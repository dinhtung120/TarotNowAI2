

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Auth.Commands.Login;

// Handler chính cho luồng đăng nhập và cấp token phiên.
public partial class LoginCommandHandler : IRequestHandler<LoginCommand, (AuthResponse Response, string RefreshToken)>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    /// Khởi tạo handler đăng nhập.
    /// Luồng xử lý: nhận user repo, password hasher, token service, jwt settings và refresh token repo.
    /// </summary>
    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
        _refreshTokenRepository = refreshTokenRepository;
    }

    /// <summary>
    /// Xử lý command đăng nhập.
    /// Luồng xử lý: tìm user theo identity, xác thực mật khẩu + trạng thái, rehash nếu cần, cấp access/refresh token.
    /// </summary>
    public async Task<(AuthResponse Response, string RefreshToken)> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Bước xác thực credential + trạng thái account trước khi phát hành token.
        var user = EnsureValidCredentials(
            await GetUserByIdentityAsync(request.EmailOrUsername, cancellationToken),
            request.Password);
        EnsureUserCanLogin(user);
        // Nâng cấp hash mật khẩu ngầm khi thuật toán/hash parameters đã thay đổi.
        await RehashPasswordIfNeededAsync(user, request.Password, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(user);
        // Tạo refresh token mới gắn với IP client để phục vụ vòng đời phiên.
        var refreshTokenString = await CreateRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, accessToken);
        return (response, refreshTokenString);
    }
}
