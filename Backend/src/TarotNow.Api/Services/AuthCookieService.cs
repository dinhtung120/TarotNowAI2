using Microsoft.Extensions.Configuration;
using TarotNow.Api.Constants;

namespace TarotNow.Api.Services;

/// <summary>
/// Dịch vụ tập trung policy cookie cho access token và refresh token.
/// </summary>
public sealed class AuthCookieService : IAuthCookieService
{
    private readonly string? _cookieDomain;

    /// <summary>
    /// Khởi tạo policy cookie auth từ cấu hình runtime.
    /// </summary>
    public AuthCookieService(IConfiguration configuration)
    {
        var configuredDomain = configuration["Auth:CookieDomain"]?.Trim();
        _cookieDomain = string.IsNullOrWhiteSpace(configuredDomain) ? null : configuredDomain;
    }

    /// <inheritdoc />
    public void SetAccessToken(HttpRequest request, HttpResponse response, string accessToken, int expiresInSeconds)
    {
        response.Cookies.Append(
            AuthCookieNames.AccessToken,
            accessToken,
            BuildBaseOptions(request, Math.Max(1, expiresInSeconds)));
    }

    /// <inheritdoc />
    public void SetRefreshToken(HttpRequest request, HttpResponse response, string refreshToken, DateTime expiresAtUtc)
    {
        var remaining = expiresAtUtc - DateTime.UtcNow;
        var maxAgeSeconds = Math.Max(1, (int)Math.Ceiling(remaining.TotalSeconds));
        response.Cookies.Append(
            AuthCookieNames.RefreshToken,
            refreshToken,
            BuildBaseOptions(request, maxAgeSeconds));
    }

    /// <inheritdoc />
    public void ClearAuthCookies(HttpRequest request, HttpResponse response)
    {
        var options = BuildBaseOptions(request, 1);
        response.Cookies.Delete(AuthCookieNames.AccessToken, options);
        response.Cookies.Delete(AuthCookieNames.RefreshToken, options);
    }

    private CookieOptions BuildBaseOptions(HttpRequest request, int maxAgeSeconds)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromSeconds(maxAgeSeconds)
        };

        if (!string.IsNullOrWhiteSpace(_cookieDomain))
        {
            options.Domain = _cookieDomain;
        }

        return options;
    }
}
