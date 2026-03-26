/*
 * ===================================================================
 * FILE: RefreshTokenCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.RefreshToken
 * ===================================================================
 * MỤC ĐÍCH:
 *   Xử lý logic cấp phát lại Token. Đây là phòng tuyến bảo mật quan trọng thứ 2 
 *   (sau Đăng Nhập). Nó chứa logic phát hiện lừa đảo (Fraud Detection / Token Reuse).
 *
 * KIỂM SOÁT ROTATION VÀ PHÁT HIỆN TÁI SỬ DỤNG (REUSE DETECTION):
 *   Hệ thống dùng nguyên lý "Refresh Token Rotation":
 *   1. User dùng RefreshToken_A để đổi lấy AccessToken_2 + RefreshToken_B.
 *   2. RefreshToken_A LẬP TỨC bị đánh dấu là "đã thu hồi" (Revoked).
 *   3. Nếu ai đó (hacker) trộm được RefreshToken_A và xin đổi token, hệ thống 
 *      sẽ phát hiện A đã bị Revoked -> CÓ KẺ ĐANG TÁI SỬ DỤNG TOKEN!
 *   4. Cơ chế Fraud Protection kích hoạt: Hủy NGAY LẬP TỨC toàn bộ gia phả 
 *      Token đang có của User (log out all devices), buộc User đăng nhập lại thật an toàn.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Handler cấp phép Refresh Token an toàn với chống replay attacks.
/// </summary>
public partial class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (AuthResponse Response, string NewRefreshToken)>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IJwtTokenSettings _jwtTokenSettings;

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

    public async Task<(AuthResponse Response, string NewRefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await GetRefreshTokenOrThrowAsync(request.Token, cancellationToken);
        EnsureTokenMatches(token, request.Token);
        await EnsureTokenNotCompromisedAsync(token, cancellationToken);
        EnsureTokenNotExpired(token);
        await RotateTokenAsync(token, cancellationToken);

        var user = EnsureUserIsActive(token.User);
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = await IssueRefreshTokenAsync(user, request.ClientIpAddress, cancellationToken);
        var response = BuildAuthResponse(user, newAccessToken);
        return (response, newRefreshToken);
    }
}
