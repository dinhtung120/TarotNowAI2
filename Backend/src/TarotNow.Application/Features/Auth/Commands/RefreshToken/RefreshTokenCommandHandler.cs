

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

// Handler chính cho luồng refresh token và rotate phiên đăng nhập.
public partial class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (AuthResponse Response, string NewRefreshToken)>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;

    /// <summary>
    /// Khởi tạo handler refresh token.
    /// Luồng xử lý: nhận các repository/service cần để validate token, rotate token và phát access token mới.
    /// </summary>
    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        ITokenService tokenService,
        IJwtTokenSettings jwtTokenSettings)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtTokenSettings = jwtTokenSettings;
    }

    /// <summary>
    /// Xử lý command refresh token.
    /// Luồng xử lý: kiểm tra token tồn tại/hợp lệ/chưa bị compromise/chưa hết hạn, rotate token cũ, cấp token mới.
    /// </summary>
    public async Task<(AuthResponse Response, string NewRefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await GetRefreshTokenOrThrowAsync(request.Token, cancellationToken);
        EnsureTokenMatches(token, request.Token);
        await EnsureTokenNotCompromisedAsync(token, cancellationToken);
        EnsureTokenNotExpired(token);
        // Thu hồi token cũ trước khi cấp token mới để enforce nguyên tắc one-time refresh token.
        await RotateTokenAsync(token, cancellationToken);

        var user = EnsureUserIsActive(token.User);
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        // Cấp refresh token mới cho vòng đời phiên kế tiếp.
        var newRefreshToken = await IssueRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, newAccessToken);
        return (response, newRefreshToken);
    }
}
