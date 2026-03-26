namespace TarotNow.Api.Services;

public interface IRefreshTokenCookieService
{
    CookieOptions BuildOptions(HttpRequest request);
}
