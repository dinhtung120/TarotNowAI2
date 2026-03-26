using MediatR;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Auth.Commands.ForgotPassword;
using TarotNow.Application.Features.Auth.Commands.ResetPassword;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthPasswordController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthPasswordController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email exists, a password reset OTP has been sent." });
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Password has been successfully reset. All existing devices have been logged out." });
    }
}
