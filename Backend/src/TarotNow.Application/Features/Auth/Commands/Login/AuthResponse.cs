/*
 * ===================================================================
 * FILE: AuthResponse.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Login
 * ===================================================================
 * MỤC ĐÍCH:
 *   DTO chung trả về kết quả cấu trúc chuẩn khi User Đăng Nhập (Login) 
 *   hoặc Làm mới token (RefreshToken) thành công.
 *
 * TẠI SAO KHÔNG CÓ REFRESH TOKEN TRONG CLASS NÀY?
 *   Bảo mật! Refresh Token là token có thời hạn rất dài, nếu để ở body JSON,
 *   kẻ cắp có thể chèn mã JS độc hại (XSS) để lấy cắp từ LocalStorage của Frontend.
 *   → Refresh token được set ở HttpOnly Cookie tại Controller (nằm ngoài body).
 *   → Trong file này chỉ chứa Access Token (lưu ở Memory/Zustand của Frontend).
 * ===================================================================
 */

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Data transfer object chứa kết quả payload trả về client (Frontend APP / Mobile)
/// Gồm Access Token (ngắn hạn) và thông tin cơ bản người dùng (Profile).
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// JWT Token (mã thông báo) dùng để gắn vào Header: Authorization: Bearer {token}
    /// Có thời hạn ngắn (vd: 15-30 phút).
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Loại token. Chuẩn OAuth2 quy định truy cập API thường dùng "Bearer" token.
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Thời gian Access Token sẽ hết hạn (để Frontend tự tính toán khi nào cần refresh).
    /// </summary>
    public int ExpiresInMinutes { get; set; }
    
    /// <summary>
    /// Thông tin vắn tắt của user (được phép hiển thị lên UI ngay lúc đăng nhập).
    /// </summary>
    public UserProfileDto User { get; set; } = new();
}

/// <summary>
/// Model chứa thông tin profile cơ bản. 
/// KHÔNG CHỨA DỮ LIỆU NHẠY CẢM (Pass, Salt, Balance chi tiết).
/// </summary>
public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Level { get; set; } // Gamification level
    public string Role { get; set; } = string.Empty; // "admin", "user", "tarot_reader"
    public string Status { get; set; } = string.Empty; // "Active", "Locked", "Banned"
}
