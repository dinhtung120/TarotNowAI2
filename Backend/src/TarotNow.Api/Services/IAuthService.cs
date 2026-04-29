using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;

namespace TarotNow.Api.Services;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(
        LoginCommand command,
        HttpContext httpContext,
        CancellationToken cancellationToken = default);

    Task<RefreshTokenResult> RefreshAsync(
        string refreshToken,
        string? idempotencyKey,
        HttpContext httpContext,
        CancellationToken cancellationToken = default);

    Task<bool> LogoutAsync(
        string refreshToken,
        bool revokeAll,
        Guid? authenticatedUserId,
        HttpContext httpContext,
        CancellationToken cancellationToken = default);
}
