using MediatR;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Auth.Commands.Register;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;
using TarotNow.Application.Features.Auth.Commands.VerifyEmail;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Auth)]
public sealed class AuthRegistrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthRegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var userId = await _mediator.Send(command);

        var response = new RegisterResponse
        {
            UserId = userId,
            Message = "Registration successful. Please verify your email to activate your account."
        };

        return CreatedAtAction("GetProfile", "Profile", null, response);
    }

        [HttpPost("send-verification-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendVerificationEmail([FromBody] SendEmailVerificationOtpCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If the email is valid and unverified, an OTP has been sent." });
    }

        [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Email verified successfully. Account is now active." });
    }
}
