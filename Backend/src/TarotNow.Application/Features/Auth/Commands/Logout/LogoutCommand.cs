using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.Logout;

/// <summary>
/// Command đăng xuất session hiện tại hoặc toàn bộ sessions.
/// </summary>
public sealed class LogoutCommand : IRequest<bool>
{
    /// <summary>
    /// Refresh token hiện tại (bắt buộc khi logout single-session).
    /// </summary>
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// Cờ đăng xuất toàn bộ thiết bị.
    /// </summary>
    public bool RevokeAll { get; init; }

    /// <summary>
    /// User id đã xác thực để phục vụ revoke-all.
    /// </summary>
    public Guid? UserId { get; init; }
}
