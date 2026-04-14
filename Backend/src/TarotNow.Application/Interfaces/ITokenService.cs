
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract sinh token xác thực để chuẩn hóa cơ chế cấp access/refresh token.
public interface ITokenService
{
    /// <summary>
    /// Sinh access token gắn session id và trả metadata để theo dõi security.
    /// </summary>
    string GenerateAccessToken(User user, Guid sessionId, out DateTime expiresAtUtc, out string jti);

    /// <summary>
    /// Sinh refresh token ngẫu nhiên để gia hạn phiên đăng nhập an toàn.
    /// Luồng xử lý: tạo chuỗi token khó đoán dùng cho lưu trữ và đối soát phiên.
    /// </summary>
    string GenerateRefreshToken();
}
