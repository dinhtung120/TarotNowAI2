namespace TarotNow.Application.Common.Constants;

/// <summary>
/// Ngưỡng bảo mật cho luồng auth nhằm giảm brute-force theo identity và IP hash.
/// </summary>
public static class AuthSecurityPolicyConstants
{
    /// <summary>
    /// Cửa sổ đếm thất bại đăng nhập.
    /// </summary>
    public static readonly TimeSpan LoginFailureWindow = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Số lần sai tối đa theo identity trong cửa sổ trước khi tạm khóa.
    /// </summary>
    public const int LoginIdentityFailureLimit = 10;

    /// <summary>
    /// Số lần sai tối đa theo IP hash trong cửa sổ trước khi tạm khóa.
    /// </summary>
    public const int LoginIpFailureLimit = 20;
}
