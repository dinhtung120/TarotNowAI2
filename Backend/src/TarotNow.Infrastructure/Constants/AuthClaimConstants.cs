namespace TarotNow.Infrastructure.Constants;

/// <summary>
/// Hằng tên claim auth dùng chung giữa phát hành JWT và validate token.
/// </summary>
internal static class AuthClaimConstants
{
    /// <summary>
    /// Claim định danh session đăng nhập theo thiết bị.
    /// </summary>
    public const string SessionId = "sid";
}

