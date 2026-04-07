using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Extensions;
using TarotNow.Api.Services;
using TarotNow.Application.Features.Auth.Commands.Login;
using TarotNow.Application.Features.Auth.Commands.RefreshToken;
using TarotNow.Application.Features.Auth.Commands.RevokeToken;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
public sealed class AuthSessionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRefreshTokenCookieService _cookieService;

    public AuthSessionController(
        IMediator mediator,
        IRefreshTokenCookieService cookieService)
    {
        _mediator = mediator;
        _cookieService = cookieService;
    }

    /// <summary>
    /// Authenticates user credentials and issues access/refresh tokens.
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var result = await _mediator.Send(command);

        Response.Cookies.Append("refreshToken", result.RefreshToken, _cookieService.BuildOptions(Request));
        return Ok(result.Response);
    }

    /// <summary>
    /// Rotates access and refresh tokens using refresh token cookie.
    /// </summary>
    [HttpPost("refresh")]
    [EnableRateLimiting("auth-session")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshTokens()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Missing refresh token",
                detail: "Refresh token is missing.");
        }

        var command = new RefreshTokenCommand
        {
            Token = refreshToken,
            ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        };

        var result = await _mediator.Send(command);

        Response.Cookies.Append("refreshToken", result.NewRefreshToken, _cookieService.BuildOptions(Request));
        return Ok(result.Response);
    }

    /// <summary>
    /// Revokes current session token or all sessions for authenticated user.
    /// </summary>
    [HttpPost("logout")]
    [EnableRateLimiting("auth-session")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromQuery] bool revokeAll = false)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var command = new RevokeTokenCommand
        {
            Token = refreshToken ?? string.Empty,
            RevokeAll = revokeAll
        };

        if (revokeAll)
        {
            if (!User.TryGetUserId(out var userId))
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Authentication required",
                    detail: "Must be authenticated to revoke all sessions.");
            }

            command.UserId = userId;
        }
        else if (string.IsNullOrEmpty(command.Token))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Missing refresh token",
                detail: "No refresh token provided.");
        }

        await _mediator.Send(command);
        Response.Cookies.Delete("refreshToken", _cookieService.BuildOptions(Request));
        return Ok(new { message = "Logged out successfully." });
    }
}
