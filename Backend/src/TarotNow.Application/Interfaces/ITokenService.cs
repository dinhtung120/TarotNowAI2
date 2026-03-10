using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface trừu tượng quản lý quá trình sinh JWT (Access Token)
/// và các chuỗi ngẫu nhiên (dùng làm Refresh Token hoặc OTP).
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Sinh Access Token (JWT) theo thông tin User.
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Sinh một chuỗi base64 URL safe độ dài lớn dùng làm Refresh Token.
    /// </summary>
    string GenerateRefreshToken();
}
