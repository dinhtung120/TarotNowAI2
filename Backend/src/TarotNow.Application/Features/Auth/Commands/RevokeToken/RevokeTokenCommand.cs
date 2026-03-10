using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

/// <summary>
/// Dùng để đăng xuất / thu hồi Refresh Token hiện tại.
/// Hoặc Logout Everywhere (truyền userId và thu hồi tất cả).
/// </summary>
public class RevokeTokenCommand : IRequest<bool>
{
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Nếu true, sẽ thực hiện Logout All Devices, thu hồi mọi token của User này.
    /// Nếu false, chỉ thu hồi token cụ thể được cấp.
    /// </summary>
    public bool RevokeAll { get; set; } = false;
    
    // Nếu RevokeAll = true, cần UserId (trích xuất từ HttpContext User Claims)
    public Guid? UserId { get; set; }
}
