using TarotNow.Application.Features.Auth.Commands.Login;

namespace TarotNow.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Kết quả command refresh token.
/// </summary>
public sealed class RefreshTokenResult
{
    /// <summary>
    /// Payload auth response trả client.
    /// </summary>
    public AuthResponse Response { get; init; } = new();

    /// <summary>
    /// Refresh token mới (raw) để set cookie.
    /// </summary>
    public string NewRefreshToken { get; init; } = string.Empty;

    /// <summary>
    /// Cờ idempotent response.
    /// </summary>
    public bool IsIdempotent { get; init; }

    /// <summary>
    /// Thời điểm hết hạn refresh token hiện tại đã cấp lại.
    /// </summary>
    public DateTime RefreshTokenExpiresAtUtc { get; init; }
}
