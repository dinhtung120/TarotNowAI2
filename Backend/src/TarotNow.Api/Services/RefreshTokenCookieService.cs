using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Api.Services;

public sealed class RefreshTokenCookieService : IRefreshTokenCookieService
{
    private readonly IWebHostEnvironment _environment;
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenCookieService(IWebHostEnvironment environment, IOptions<JwtOptions> jwtOptions)
    {
        _environment = environment;
        _jwtOptions = jwtOptions.Value;
    }

    public CookieOptions BuildOptions(HttpRequest request)
    {
        var shouldUseSecureCookie = !_environment.IsDevelopment() || request.IsHttps;
        var expiryDays = _jwtOptions.RefreshExpiryDays > 0 ? _jwtOptions.RefreshExpiryDays : 7;

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = shouldUseSecureCookie,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };
    }
}
