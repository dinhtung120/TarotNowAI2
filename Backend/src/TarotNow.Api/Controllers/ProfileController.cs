

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Threading.Tasks;

using TarotNow.Api.Contracts; 
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Profile.Commands.UpdateProfile; 
using TarotNow.Application.Features.Profile.Commands.UploadAvatar;  
using TarotNow.Application.Features.Profile.Queries.GetProfile;     
using Microsoft.AspNetCore.Http;

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Profile)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
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
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        
        var query = new GetProfileQuery { UserId = userId };

        
        var result = await Mediator.Send(query);
        return Ok(result);
    }

        [HttpPatch]
    [Authorize] 
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        
        
        var command = new UpdateProfileCommand
        {
            UserId = userId,                    
            DisplayName = request.DisplayName,  
            AvatarUrl = request.AvatarUrl,      
            DateOfBirth = request.DateOfBirth    
        };

        
        var success = await Mediator.Send(command);

        
        return success
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot update profile",
                detail: "Không thể cập nhật hồ sơ người dùng.");
    }

        [HttpPost("avatar")]
    [Authorize] 
    [RequestSizeLimit(10 * 1024 * 1024)] 
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        if (file == null || file.Length == 0)
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid avatar file",
                detail: "File ảnh không được để trống.");

        using var stream = file.OpenReadStream();
        
        var command = new UploadAvatarCommand
        {
            UserId = userId,
            ImageStream = stream,
            FileName = file.FileName,
            ContentType = file.ContentType
        };

        var relativeUrl = await Mediator.Send(command);

        return Ok(new { success = true, avatarUrl = relativeUrl });
    }
}
