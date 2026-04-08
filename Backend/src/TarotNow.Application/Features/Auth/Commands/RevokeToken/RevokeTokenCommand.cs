

using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RevokeToken;

// Command thu hồi refresh token theo token cụ thể hoặc toàn bộ token của một user.
public class RevokeTokenCommand : IRequest<bool>
{
    // Refresh token cần revoke khi RevokeAll = false.
    public string Token { get; set; } = string.Empty;

    // Cờ bật chế độ revoke toàn bộ token của user.
    public bool RevokeAll { get; set; } = false;

    // UserId bắt buộc khi RevokeAll = true.
    public Guid? UserId { get; set; }
}
