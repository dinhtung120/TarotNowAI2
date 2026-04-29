using MediatR;

namespace TarotNow.Application.Features.Auth.Commands.RevokeRefreshToken;

/// <summary>
/// Command thu hồi refresh token theo token cụ thể hoặc toàn bộ token của một user.
/// </summary>
public sealed class RevokeRefreshTokenCommand : IRequest<bool>
{
    /// <summary>
    /// Refresh token cần revoke khi RevokeAll = false.
    /// </summary>
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// Cờ bật chế độ revoke toàn bộ token của user.
    /// </summary>
    public bool RevokeAll { get; init; }

    /// <summary>
    /// UserId bắt buộc khi RevokeAll = true (khi không có token).
    /// </summary>
    public Guid? UserId { get; init; }
}
