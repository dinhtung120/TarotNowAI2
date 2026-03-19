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
using Microsoft.Extensions.Configuration;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Exceptions;
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Handler cấp phép Refresh Token an toàn với chống replay attacks.
/// </summary>
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
        // 1. Quét Database xem tồn tại dòng chữ Token giả mạo hay không.
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (existingToken == null)
        {
            throw new DomainException("INVALID_TOKEN", "Refresh token does not exist.");
        }

        // Domain Rule check: Kiểm tra Token có khớp 100% hay không.
        if (!existingToken.MatchesToken(request.Token))
        {
            throw new DomainException("INVALID_TOKEN", "Refresh token is invalid.");
        }

        // --------------------------------------------------------------------------
        // 2. NHẬN DIỆN LỪA ĐẢO (FRAUD DETECTION / TOKEN REUSE ALERT)
        // --------------------------------------------------------------------------
        // Nếu token đã được sử dụng (IsRevoked) mà lại bị đòi sử dụng tiếp...
        // Chứng tỏ có 2 thực thể đang cạnh tranh dùng Token, ÍT NHẤT 1 bên là Hacker trộm Cookie!
        if (existingToken.IsRevoked)
        {
            // Bắn phá toàn bộ rễ của Cookie: Xoá tất cả Token của User, log out tất cả thiết bị.
            await _refreshTokenRepository.RevokeAllByUserIdAsync(existingToken.UserId, cancellationToken);
            
            throw new DomainException(
                "TOKEN_COMPROMISED", 
                "Token reuse detected. All sessions have been revoked for security reasons. Please log in again."
            );
        }

        // 3. Kiểm tra Token còn thời hạn không. (Token thiu thì đuổi đi)
        if (existingToken.IsExpired)
        {
            throw new DomainException("TOKEN_EXPIRED", "Refresh token has expired. Please log in again.");
        }

        // --------------------------------------------------------------------------
        // 4. MỌI THỨ HỢP LỆ -> LUÂN CHUYỂN TOKEN (ROTATION)
        // Đốt bỏ chiếc vé Token cũ vừa dùng. (Mark "Revoked")
        // --------------------------------------------------------------------------
        existingToken.Revoke();
        await _refreshTokenRepository.UpdateAsync(existingToken, cancellationToken);

        var user = existingToken.User;
        
        // 5. Kiểm duyệt lần cuối: Có thể User bị Admin Banned trong lúc đang cầm Token hợp lệ.
        if (user == null || user.Status == Domain.Enums.UserStatus.Banned || user.Status == Domain.Enums.UserStatus.Locked)
        {
            throw new DomainException("USER_BLOCKED", "User account is no longer active.");
        }

        // --------------------------------------------------------------------------
        // 6. CẤP PHÁT (ISSUANCE) LẠI 2 LOẠI TOKEN MỚI
        // --------------------------------------------------------------------------
        // Tạo JWT ngắn hạn
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        
        // Tạo dây chuỗi Refresh dài hạn
        var newRefreshTokenString = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = ResolveRefreshTokenExpiryDays();
        
        var newRefreshTokenEntity = new TarotNow.Domain.Entities.RefreshToken(
            userId: user.Id,
            token: newRefreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(refreshTokenExpiryDays), // Chờ 7 ngày
            createdByIp: request.ClientIpAddress // Đính tên vị trí xin vé
        );

        // Đẩy xuống Database
        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

        var resp = new AuthResponse
        {
            AccessToken = newAccessToken,
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

        return (resp, newRefreshTokenString);
    }

    /// <summary>Tính tuổi thọ Token qua biến môi trường Settings</summary>
    private int ResolveAccessTokenExpiryMinutes()
    {
        var configured = _configuration["Jwt:ExpiryMinutes"]
                         ?? _configuration["Jwt:AccessTokenExpirationMinutes"];

        return int.TryParse(configured, out var value) && value > 0 ? value : 15;
    }

    /// <summary>Tính tuổi thọ Refresh qua biến môi trường Settings</summary>
    private int ResolveRefreshTokenExpiryDays()
    {
        var configured = _configuration["Jwt:RefreshExpiryDays"]
                         ?? _configuration["Jwt:RefreshTokenExpirationDays"];

        return int.TryParse(configured, out var value) && value > 0 ? value : 7;
    }
}
