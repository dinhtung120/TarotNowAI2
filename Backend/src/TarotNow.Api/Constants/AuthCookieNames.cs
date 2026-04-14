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

    /// <summary>
    /// Cookie chứa định danh thiết bị do frontend tạo để hỗ trợ multi-device isolation.
    /// </summary>
    public const string DeviceId = "deviceId";
}
