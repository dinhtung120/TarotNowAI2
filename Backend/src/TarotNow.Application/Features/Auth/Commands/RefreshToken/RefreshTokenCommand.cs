/*
 * ===================================================================
 * FILE: RefreshTokenCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.RefreshToken
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command kích hoạt luồng "Xin cấp lại Access Token mới".
 *   Khi Access Token (JWT) cũ hết hạn (15 phút), Frontend tự động gọi API 
 *   này kèm theo Refresh Token để đổi lấy JWT mới mà không bắt User đăng nhập lại.
 *
 * LUỒNG HOẠT ĐỘNG:
 *   - Nhận Token (từ HttpOnly cookie).
 *   - Lấy Client IP để log lại luồng cấp phát rủi ro.
 *   - Trả về Tuple gồm AuthResponse mới (kèm User Profile) và Dây RefreshToken mới.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// DTO đầu vào cho chức năng Refresh Token.
/// </summary>
public class RefreshTokenCommand : IRequest<(AuthResponse Response, string NewRefreshToken)>
{
    /// <summary>
    /// Chuỗi mã Refresh Token trích xuất từ Cookie.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ IP máy trạm yêu cầu. Dùng để Audit Security (truy vết IP lạ).
    /// </summary>
    public string ClientIpAddress { get; set; } = string.Empty;
}
