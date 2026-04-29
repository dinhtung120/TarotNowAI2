using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TarotNow.Api.Constants;

namespace TarotNow.Api.Services;

/// <summary>
/// Dịch vụ tập trung policy cookie cho access token và refresh token.
/// </summary>
public sealed class AuthCookieService : IAuthCookieService
{
    private readonly string? _cookieDomain;
    private readonly bool? _cookieSecureOverride;
    private readonly IHostEnvironment _hostEnvironment;

    public AuthCookieService(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _cookieDomain = NormalizeNullable(configuration["Auth:CookieDomain"]);
        _cookieSecureOverride = ParseBoolean(configuration["Auth:CookieSecure"]);
    }

    public void SetAccessToken(HttpRequest request, HttpResponse response, string accessToken, int expiresInSeconds)
    {
        response.Cookies.Append(
            AuthCookieNames.AccessToken,
            accessToken,
            BuildCookieOptions(request, Math.Max(1, expiresInSeconds)));
    }

    public void SetRefreshToken(HttpRequest request, HttpResponse response, string refreshToken, DateTime expiresAtUtc)
    {
        var remaining = expiresAtUtc - DateTime.UtcNow;
        var maxAgeSeconds = Math.Max(1, (int)Math.Ceiling(remaining.TotalSeconds));
        response.Cookies.Append(
            AuthCookieNames.RefreshToken,
            refreshToken,
            BuildCookieOptions(request, maxAgeSeconds));
    }

    public void ClearAuthCookies(HttpRequest request, HttpResponse response)
    {
        var options = BuildCookieOptions(request, 1);
        response.Cookies.Delete(AuthCookieNames.AccessToken, options);
        response.Cookies.Delete(AuthCookieNames.RefreshToken, options);
    }

    private CookieOptions BuildCookieOptions(HttpRequest request, int maxAgeSeconds)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = ShouldUseSecureCookie(request),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            MaxAge = TimeSpan.FromSeconds(maxAgeSeconds)
        };

        var resolvedDomain = ResolveCookieDomain(request);
        if (!string.IsNullOrWhiteSpace(resolvedDomain))
        {
            options.Domain = resolvedDomain;
        }

        return options;
    }

    private string? ResolveCookieDomain(HttpRequest request)
    {
        if (string.IsNullOrWhiteSpace(_cookieDomain))
        {
            return null;
        }

        var host = ResolveRequestHost(request);
        if (string.IsNullOrWhiteSpace(host))
        {
            return _cookieDomain;
        }

        var normalizedDomain = _cookieDomain.Trim().ToLowerInvariant();
        if (host == normalizedDomain || host.EndsWith($".{normalizedDomain}", StringComparison.Ordinal))
        {
            return _cookieDomain;
        }

        return null;
    }

    private bool ShouldUseSecureCookie(HttpRequest request)
    {
        if (_cookieSecureOverride.HasValue)
        {
            return _cookieSecureOverride.Value;
        }

        if (_hostEnvironment.IsProduction())
        {
            return true;
        }

        if (request.IsHttps)
        {
            return true;
        }

        var forwardedProto = request.Headers["x-forwarded-proto"].ToString();
        if (string.IsNullOrWhiteSpace(forwardedProto))
        {
            return false;
        }

        return string.Equals(forwardedProto.Split(',')[0].Trim(), "https", StringComparison.OrdinalIgnoreCase);
    }

    private static string? ResolveRequestHost(HttpRequest request)
    {
        var forwardedHost = request.Headers["x-forwarded-host"].ToString();
        if (!string.IsNullOrWhiteSpace(forwardedHost))
        {
            return forwardedHost.Split(',')[0].Trim().Split(':')[0].Trim().ToLowerInvariant();
        }

        var host = request.Host.Host?.Trim();
        if (string.IsNullOrWhiteSpace(host))
        {
            return null;
        }

        return host.ToLowerInvariant();
    }

    private static string? NormalizeNullable(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static bool? ParseBoolean(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        return bool.TryParse(raw.Trim(), out var parsed) ? parsed : null;
    }
}
