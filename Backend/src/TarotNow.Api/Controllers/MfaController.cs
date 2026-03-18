using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotNow.Application.Features.Mfa.Commands.MfaChallenge;
using TarotNow.Application.Features.Mfa.Commands.MfaSetup;
using TarotNow.Application.Features.Mfa.Commands.MfaVerify;
using TarotNow.Application.Features.Mfa.Queries.GetMfaStatus;

namespace TarotNow.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MfaController : ControllerBase
{
    private readonly IMediator _mediator;

    public MfaController(IMediator mediator) => _mediator = mediator;

    private Guid? GetUserId()
    {
        var str = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(str, out var id) ? id : null;
    }

    /// <summary>
    /// Bước 1: Lấy thông tin thiết lập MFA (URI + Secret).
    /// </summary>
    [HttpPost("setup")]
    public async Task<IActionResult> Setup()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _mediator.Send(new MfaSetupCommand { UserId = userId.Value });
        return Ok(result);
    }

    /// <summary>
    /// Bước 2: Verify TOTP để Enable MFA.
    /// </summary>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] MfaVerifyBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _mediator.Send(new MfaVerifyCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "MFA đã được bật thành công." });
    }

    /// <summary>
    /// Khớp MFA cho các hành động quan trọng (nhạy cảm).
    /// </summary>
    [HttpPost("challenge")]
    public async Task<IActionResult> Challenge([FromBody] MfaChallengeBody body)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        await _mediator.Send(new MfaChallengeCommand { UserId = userId.Value, Code = body.Code });
        return Ok(new { success = true, msg = "Xác thực MFA thành công." });
    }

    /// <summary>Lấy trạng thái MFA.</summary>
    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var result = await _mediator.Send(new GetMfaStatusQuery { UserId = userId.Value });
        return Ok(new { mfaEnabled = result.MfaEnabled });
    }
}

public class MfaVerifyBody
{
    public string Code { get; set; } = string.Empty;
}

public class MfaChallengeBody
{
    public string Code { get; set; } = string.Empty;
}
