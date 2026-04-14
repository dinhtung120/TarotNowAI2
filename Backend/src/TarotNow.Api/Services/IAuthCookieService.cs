namespace TarotNow.Api.Services;

/// <summary>
/// Trừu tượng hóa cấu hình cookie auth để controller không hard-code policy.
/// </summary>
public interface IAuthCookieService
{
    /// <summary>
    /// Set access token cookie.
    /// </summary>
    void SetAccessToken(HttpRequest request, HttpResponse response, string accessToken, int expiresInSeconds);

    /// <summary>
    /// Set refresh token cookie.
    /// </summary>
    void SetRefreshToken(HttpRequest request, HttpResponse response, string refreshToken);

    /// <summary>
    /// Xóa toàn bộ auth cookies.
    /// </summary>
    void ClearAuthCookies(HttpRequest request, HttpResponse response);
}
