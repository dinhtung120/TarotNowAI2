/*
 * ===================================================================
 * FILE: LoginCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.Login
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command đóng gói dữ liệu đầu vào cho yêu cầu Đăng Nhập.
 *
 * KIỂU TRẢ VỀ CỦA IRequest<(AuthResponse, string)>:
 *   Đăng nhập trả về TUPLE (Tuple trong C#).
 *   - Phần 1 (AuthResponse): Là DTO json để trả về body (AccessToken, User Profile).
 *   - Phần 2 (string): Là RefreshToken. Nó được tách ra để Controller lấy 
 *     gắn vào Secure HttpOnly Cookie thay vì chèn vào Body JSON.
 * ===================================================================
 */

using System.Text.Json.Serialization;
using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Input model cho Endpoint Login. Hỗ trợ hệ thống đăng nhập đa kênh:
/// Có thể dùng Email hoặc Username.
/// </summary>
public class LoginCommand : IRequest<(AuthResponse Response, string RefreshToken)>
{
    /// <summary>
    /// Người dùng điền chuỗi này từ form. Có thể là "nguyenvanA" (username)
    /// hoặc "nguyenvanA@gmail.com" (email). Handler sẽ tự phân tích để query database.
    /// </summary>
    public string EmailOrUsername { get; set; } = string.Empty;

    /// <summary>
    /// Mật khẩu người dùng nhập (chuỗi plaintext trơn).
    /// Khi so sánh, backend sẽ đem chuỗi này mã hóa (hash) và so với chuỗi hash trong DB.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// IP của thiết bị client (điện thoại/trình duyệt). 
    /// Lấy tại Middleware/Controller (`HttpContext.Connection.RemoteIpAddress`) và gán vào đây.
    /// Dùng cho nghiệp vụ Security Audit Logs (theo dõi thiết bị/vị trí đăng nhập chặn giả mạo).
    /// [JsonIgnore] nghĩa là Swagger và ModelBinding sẽ không map field này từ payload JSON của request HTTP.
    /// </summary>
    [JsonIgnore]
    public string ClientIpAddress { get; set; } = string.Empty;
}
