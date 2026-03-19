/*
 * FILE: JwtTokenService.cs
 * MỤC ĐÍCH: Implementation tạo/sinh JWT Access Token + Refresh Token.
 *
 *   ACCESS TOKEN (JWT):
 *   → Chứa thông tin User: sub (userId), email, username, status, role
 *   → Ký bằng HMAC-SHA256 với secret key từ appsettings.json
 *   → Ngắn hạn (mặc định 15 phút) → giảm thiểu rủi ro khi bị đánh cắp
 *   → Có issuer + audience → validate khi verify (chống token giả)
 *   → JTI (JWT ID) unique → có thể blacklist token cụ thể
 *
 *   REFRESH TOKEN:
 *   → Random 64 bytes (cryptographically secure) → Base64 string
 *   → Dài hạn (7 ngày) → lưu trong DB (hashed) → xoay token khi dùng
 *   → Không chứa thông tin User → không thể decode được → an toàn hơn JWT
 *
 *   CẤU HÌNH (appsettings.json):
 *   → Jwt:SecretKey: khóa bí mật ký token (≥256 bit)
 *   → Jwt:Issuer: ai phát hành token (vd: "TarotNow.Api")
 *   → Jwt:Audience: token dành cho ai (vd: "TarotNow.Client")
 *   → Jwt:ExpiryMinutes: thời hạn access token (mặc định 15 phút)
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

namespace TarotNow.Infrastructure.Security;

/// <summary>
/// Implement ITokenService — tạo JWT Access Token và Refresh Token.
/// </summary>
public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sinh JWT Access Token chứa claims của User.
    /// Claims bao gồm:
    ///   - sub: User ID (UUID) — định danh duy nhất
    ///   - email: email User
    ///   - username: tên đăng nhập
    ///   - status: trạng thái tài khoản (active, banned, v.v.)
    ///   - role: vai trò (user, reader, admin) — dùng cho policy-based authorization
    ///   - jti: JWT ID (UUID random) — chống replay attack, hỗ trợ blacklist
    /// </summary>
    public string GenerateAccessToken(User user)
    {
        // Đọc cấu hình bắt buộc — throw nếu thiếu
        var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is missing.");
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");
        var expirationMinutes = ResolveAccessTokenExpiryMinutes();

        // Tạo signing credentials từ secret key + HMAC-SHA256
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Định nghĩa claims nhúng vào token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username),
            new Claim("status", user.Status.ToString()),
            new Claim(ClaimTypes.Role, user.Role), // Role claim cho [Authorize(Policy = "Admin")]
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
        };

        // Tạo token với issuer, audience, claims, thời hạn, và chữ ký
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Đọc thời hạn access token từ config (hỗ trợ cả 2 key name để backward compatible).
    /// Mặc định: 15 phút nếu không cấu hình.
    /// </summary>
    private int ResolveAccessTokenExpiryMinutes()
    {
        var configured = _configuration["Jwt:ExpiryMinutes"]
                         ?? _configuration["Jwt:AccessTokenExpirationMinutes"];

        return int.TryParse(configured, out var value) && value > 0
            ? value
            : 15; // Mặc định 15 phút
    }

    /// <summary>
    /// Sinh Refresh Token — 64 bytes ngẫu nhiên (cryptographically secure) → Base64.
    /// Không chứa thông tin User → không decode được → an toàn.
    /// Token này sẽ được hash (SHA-256) trước khi lưu vào DB (RefreshTokenRepository).
    /// </summary>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64]; // 512 bits entropy — đủ an toàn
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
