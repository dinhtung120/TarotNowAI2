using Microsoft.Extensions.Options;
using TarotNow.Api.Constants;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Api.Services;

/// <summary>
/// Dịch vụ tập trung policy cookie cho access token và refresh token.
/// </summary>
public sealed class AuthCookieService : IAuthCookieService
{
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Khởi tạo auth cookie service.
    /// </summary>
    public AuthCookieService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
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
    public void SetRefreshToken(HttpRequest request, HttpResponse response, string refreshToken)
    {
        var refreshExpiryDays = _jwtOptions.RefreshExpiryDays > 0 ? _jwtOptions.RefreshExpiryDays : 30;
        var maxAgeSeconds = refreshExpiryDays * 24 * 60 * 60;
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
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromSeconds(maxAgeSeconds)
        };
    }
}
