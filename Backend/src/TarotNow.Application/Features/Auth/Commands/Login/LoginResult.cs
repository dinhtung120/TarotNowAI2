namespace TarotNow.Application.Features.Auth.Commands.Login;

/// <summary>
/// Kết quả command đăng nhập.
/// </summary>
public sealed class LoginResult
{
    /// <summary>
    /// Payload auth response cho client.
    /// </summary>
    public AuthResponse Response { get; init; } = new();

    /// <summary>
    /// Refresh token mới (raw) để set cookie.
    /// </summary>
    public string RefreshToken { get; init; } = string.Empty;
}
