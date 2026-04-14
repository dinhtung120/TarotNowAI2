using Microsoft.Extensions.Options;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Api.Services;

// Tập trung rule cấu hình cookie refresh token để đảm bảo nhất quán bảo mật giữa các endpoint auth.
public sealed class RefreshTokenCookieService : IRefreshTokenCookieService
{
    private readonly IWebHostEnvironment _environment;
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Khởi tạo dịch vụ tạo cookie refresh token.
    /// Luồng xử lý: nhận environment và JwtOptions để quyết định secure flag và thời gian hết hạn phù hợp.
    /// </summary>
    public RefreshTokenCookieService(IWebHostEnvironment environment, IOptions<JwtOptions> jwtOptions)
    {
        _environment = environment;
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Tạo CookieOptions cho refresh token theo môi trường runtime và request hiện tại.
    /// Luồng xử lý: xác định có bắt buộc Secure hay không, tính expiry days hợp lệ, rồi trả cấu hình cookie chuẩn.
    /// </summary>
    public CookieOptions BuildOptions(HttpRequest request)
    {
        // Rule bảo mật: production luôn bắt Secure, development cho phép HTTP local khi chưa có TLS.
        var shouldUseSecureCookie = !_environment.IsDevelopment() || request.IsHttps;
        // Edge case cấu hình sai: fallback 7 ngày để tránh refresh token hết hạn ngay lập tức.
        var expiryDays = _jwtOptions.RefreshExpiryDays > 0 ? _jwtOptions.RefreshExpiryDays : 7;

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = shouldUseSecureCookie,
            SameSite = SameSiteMode.Lax,
            // Dùng root path để refresh token khả dụng cho toàn bộ API auth flow.
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(expiryDays)
        };
    }
}
