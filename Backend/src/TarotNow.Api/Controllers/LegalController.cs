using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TarotNow.Application.Features.Legal.Commands.RecordConsent;
using TarotNow.Application.Features.Legal.Queries.CheckConsent;

namespace TarotNow.Api.Controllers;

[Route("api/v1/legal")]
[ApiController]
public class LegalController : ControllerBase
{
    private readonly IMediator Mediator;

    public LegalController(IMediator mediator)
    {
        Mediator = mediator;
    }
    [HttpGet("consent-status")]
    [Authorize]
    public async Task<IActionResult> CheckConsentStatus([FromQuery] string? documentType, [FromQuery] string? version)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();
 
        var query = new CheckConsentQuery 
        { 
            UserId = userId,
            DocumentType = documentType,
            Version = version
        };
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("consent")]
    [Authorize]
    public async Task<IActionResult> RecordConsent([FromBody] RecordConsentRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = Request.Headers["User-Agent"].ToString();

        var command = new RecordConsentCommand
        {
            UserId = userId,
            DocumentType = request.DocumentType,
            Version = request.Version,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await Mediator.Send(command);
        return Ok(new { success = true });
    }
}

public class RecordConsentRequest
{
    public string DocumentType { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
