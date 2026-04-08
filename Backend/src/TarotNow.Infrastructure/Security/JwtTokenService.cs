using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Security;

// Service phát hành access token và refresh token cho luồng xác thực.
public class JwtTokenService : ITokenService
{
    // Lưu cấu hình JWT để tạo token nhất quán theo môi trường triển khai.
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Khởi tạo service với cấu hình JWT từ hệ thống.
    /// Luồng dependency injection giúp thay đổi chính sách token mà không sửa code nghiệp vụ.
    /// </summary>
    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    /// <summary>
    /// Tạo access token cho người dùng sau khi đăng nhập hợp lệ.
    /// Luồng dựng claim chuẩn, ký token bằng HMAC-SHA256 và gắn thời hạn hết hạn theo cấu hình.
    /// </summary>
    public string GenerateAccessToken(User user)
    {
        // Validate cấu hình bắt buộc sớm để fail-fast thay vì tạo token thiếu thông tin bảo mật.
        var secretKey = _jwtOptions.SecretKey ?? throw new InvalidOperationException("Jwt:SecretKey is missing.");
        var issuer = _jwtOptions.Issuer ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = _jwtOptions.Audience ?? throw new InvalidOperationException("Jwt:Audience is missing.");
        var expirationMinutes = ResolveAccessTokenExpiryMinutes();

        // Tạo thông tin ký số để bảo đảm token không bị giả mạo.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Gắn claim cốt lõi phục vụ authorization và truy vết phiên đăng nhập.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("username", user.Username),
            new Claim("status", user.Status.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Tạo JWT đã ký để client dùng cho các API yêu cầu xác thực.
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Xác định thời hạn access token theo cấu hình hoặc fallback mặc định.
    /// Luồng fallback giúp hệ thống vẫn an toàn khi cấu hình chưa đầy đủ.
    /// </summary>
    private int ResolveAccessTokenExpiryMinutes()
    {
        return _jwtOptions.ExpiryMinutes > 0
            ? _jwtOptions.ExpiryMinutes
            : 15;
    }

    /// <summary>
    /// Tạo refresh token ngẫu nhiên có entropy cao.
    /// Luồng dùng RNG hệ thống để giảm rủi ro dự đoán token.
    /// </summary>
    public string GenerateRefreshToken()
    {
        // 64 byte ngẫu nhiên cho không gian khóa lớn, phù hợp token dài hạn.
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
