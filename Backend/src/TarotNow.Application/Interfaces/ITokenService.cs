
using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

// Contract sinh token xác thực để chuẩn hóa cơ chế cấp access/refresh token.
public interface ITokenService
{
    /// <summary>
    /// Sinh access token cho người dùng sau khi xác thực thành công.
    /// Luồng xử lý: đọc thông tin user, đóng gói claims cần thiết và trả JWT ngắn hạn.
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Sinh refresh token ngẫu nhiên để gia hạn phiên đăng nhập an toàn.
    /// Luồng xử lý: tạo chuỗi token khó đoán dùng cho lưu trữ và đối soát phiên.
    /// </summary>
    string GenerateRefreshToken();
}
