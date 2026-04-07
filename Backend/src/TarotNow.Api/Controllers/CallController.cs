using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Features.Call.Queries.GetCallHistory;

namespace TarotNow.Api.Controllers;

[ApiController]
[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.ConversationCalls)]
public class CallController : ControllerBase
{
    private readonly IMediator _mediator;

    public CallController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet]
    [EnableRateLimiting("call-history")]
    public async Task<IActionResult> GetCallHistory(
        [FromRoute] string conversationId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        var participantIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(participantIdStr, out var participantId))
            throw new UnauthorizedAccessException();

        var query = new GetCallHistoryQuery
        {
            ConversationId = conversationId,
            ParticipantId = participantId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return Ok(new
        {
            TotalCount = result.TotalCount,
            Items = result.Items
        });
    }

    
    
    
}
