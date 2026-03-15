using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TarotNow.Api.Contracts;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile;
using TarotNow.Application.Features.Profile.Queries.GetProfile;

namespace TarotNow.Api.Controllers;

[Route("api/v1/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IMediator Mediator;

    public ProfileController(IMediator mediator)
    {
        Mediator = mediator;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var query = new GetProfileQuery { UserId = userId };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var command = new UpdateProfileCommand
        {
            UserId = userId,
            DisplayName = request.DisplayName,
            AvatarUrl = request.AvatarUrl,
            DateOfBirth = request.DateOfBirth
        };

        var success = await Mediator.Send(command);
        return success ? Ok(new { success = true }) : BadRequest();
    }
}

