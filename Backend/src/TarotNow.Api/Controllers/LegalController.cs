

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Threading.Tasks;


using TarotNow.Api.Contracts; 
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Legal.Commands.RecordConsent;
using TarotNow.Application.Features.Legal.Queries.CheckConsent;

namespace TarotNow.Api.Controllers;


[Route(ApiRoutes.Legal)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
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
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();
 
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
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        
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
