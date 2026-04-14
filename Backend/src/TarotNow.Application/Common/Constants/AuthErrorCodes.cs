namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Tập trung mã lỗi auth để tránh magic string rải rác trong handlers/controllers.
/// </summary>
public static class AuthErrorCodes
{
    /// <summary>
    /// Sai credential hoặc token không hợp lệ.
    /// </summary>
    public const string Unauthorized = "AUTH_UNAUTHORIZED";

    /// <summary>
    /// Refresh token đã hết hạn.
    /// </summary>
    public const string TokenExpired = "AUTH_TOKEN_EXPIRED";

    /// <summary>
    /// Refresh token bị replay/reuse bất thường.
    /// </summary>
    public const string TokenReplay = "AUTH_TOKEN_REPLAY";

    /// <summary>
    /// Account bị khóa/banned hoặc không đủ điều kiện đăng nhập.
    /// </summary>
    public const string UserBlocked = "AUTH_USER_BLOCKED";

    /// <summary>
    /// Yêu cầu auth vượt giới hạn tần suất.
    /// </summary>
    public const string RateLimited = "AUTH_RATE_LIMITED";
}
