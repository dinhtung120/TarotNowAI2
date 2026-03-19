/*
 * ===================================================================
 * FILE: RevokeTokenCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Auth.Commands.RevokeToken
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh thủ hồi Access/Refresh Token trước thời hạn (Chức năng LOGOUT).
 *   - Logout cục bộ: Xoá bỏ 1 phiên trên đúng cái Browser/Thiết bị này.
 *   - Logout toàn thiết bị: Xoá tất cả phiên của người dùng đó (RevokeAll = true).
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

/// <summary>
/// Command báo hiệu tước quyền đăng nhập. Dùng khi user bấm Sign Out.
/// </summary>
public class RevokeTokenCommand : IRequest<bool>
{
    /// <summary>
    /// Mã Refresh Token cũ trên thiết bị hiện tại (Đọc từ HttpOnly Cookie).
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Chế độ Vô Hiệu Hoá Hàng Loạt (Logout Everywhere).
    /// </summary>
    public bool RevokeAll { get; set; } = false;
    
    /// <summary>
    /// Biến điều kiện khi RevokeAll=True. Phải biết User là ai thì mới xoá được tất cả thiết bị.
    /// Thông số này sẽ lấy từ HttpContext Claims thông qua JWT Token xác thực.
    /// </summary>
    public Guid? UserId { get; set; }
}
