

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;


using TarotNow.Application.Features.Mfa.Commands.MfaChallenge; 
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;     
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;    
using TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;  

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Controller)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
public class MfaController : ControllerBase
{
    private readonly IMediator _mediator;

    
    public MfaController(IMediator mediator) => _mediator = mediator;

        private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }

        [HttpPost("setup")]
    public async Task<IActionResult> Setup()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        
        var result = await _mediator.Send(new MfaSetupCommand { UserId = userId.Value });
        return Ok(result);
    }

        [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] MfaVerifyBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        
        await _mediator.Send(new MfaVerifyCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "MFA đã được bật thành công." });
    }

        [HttpPost("challenge")]
    public async Task<IActionResult> Challenge([FromBody] MfaChallengeBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _mediator.Send(new MfaChallengeCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "Xác thực MFA thành công." });
    }

        [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _mediator.Send(new GetMfaStatusQuery { UserId = userId.Value });
        return Ok(new { mfaEnabled = result.MfaEnabled });
    }
}
