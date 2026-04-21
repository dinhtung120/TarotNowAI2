namespace TarotNow.Application.Common.Helpers;

/// <summary>
/// Helper kiểm tra và chuẩn hóa URL mạng xã hội cho hồ sơ Reader.
/// </summary>
public static class ReaderSocialUrlValidator
{
    private static readonly HashSet<string> FacebookHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "facebook.com",
        "www.facebook.com",
        "m.facebook.com",
        "fb.com",
        "www.fb.com"
    };

    private static readonly HashSet<string> InstagramHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "instagram.com",
        "www.instagram.com"
    };

    private static readonly HashSet<string> TikTokHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "tiktok.com",
        "www.tiktok.com",
        "m.tiktok.com",
        "vt.tiktok.com"
    };

    /// <summary>
    /// Chuẩn hóa URL đầu vào; chuỗi trắng trả về null.
    /// </summary>
    public static string? NormalizeOptionalUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        return url.Trim();
    }

    /// <summary>
    /// Kiểm tra URL Facebook hợp lệ.
    /// </summary>
    public static bool IsValidFacebookUrl(string? url)
        => IsValidPlatformUrl(url, FacebookHosts);

    /// <summary>
    /// Kiểm tra URL Instagram hợp lệ.
    /// </summary>
    public static bool IsValidInstagramUrl(string? url)
        => IsValidPlatformUrl(url, InstagramHosts);

    /// <summary>
    /// Kiểm tra URL TikTok hợp lệ.
    /// </summary>
    public static bool IsValidTikTokUrl(string? url)
        => IsValidPlatformUrl(url, TikTokHosts);

    /// <summary>
    /// Kiểm tra ít nhất một link social đã được cung cấp.
    /// </summary>
    public static bool HasAtLeastOneSocialLink(string? facebookUrl, string? instagramUrl, string? tikTokUrl)
        => string.IsNullOrWhiteSpace(facebookUrl) == false
           || string.IsNullOrWhiteSpace(instagramUrl) == false
           || string.IsNullOrWhiteSpace(tikTokUrl) == false;

    private static bool IsValidPlatformUrl(string? url, HashSet<string> supportedHosts)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return true;
        }

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.Scheme is not ("http" or "https"))
        {
            return false;
        }

        return supportedHosts.Contains(uri.Host);
    }
}
