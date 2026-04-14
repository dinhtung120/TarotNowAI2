namespace TarotNow.Api.Constants;

/// <summary>
/// Tên cookie auth dùng thống nhất giữa API và frontend.
/// </summary>
public static class AuthCookieNames
{
    /// <summary>
    /// Cookie chứa access token JWT ngắn hạn.
    /// </summary>
    public const string AccessToken = "accessToken";

    /// <summary>
    /// Cookie chứa refresh token dùng cho rotation.
    /// </summary>
    public const string RefreshToken = "refreshToken";
}
