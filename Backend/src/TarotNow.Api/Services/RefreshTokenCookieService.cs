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
            /* 
             * FIX: Bắt buộc phải set Path = "/" 
             * Nếu không set, cookie sẽ mặc định lấy path từ URL request (ví dụ: /api/v1/auth/login).
             * Khi đó, cookie CHỈ được trình duyệt gửi kèm các request có path bắt đầu bằng /api/v1/auth/login,
             * mà KHÔNG gửi cho /api/v1/auth/refresh hay /api/v1/auth/logout → gây lỗi refresh/logout thất bại.
             * Set Path = "/" đảm bảo cookie được gửi cho MỌI request tới cùng domain.
             */
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };
    }
}
