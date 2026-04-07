

using MediatR;                 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using TarotNow.Api.Extensions;


using TarotNow.Application.Features.History.Queries.GetReadingDetail;  
using TarotNow.Application.Features.History.Queries.GetReadingHistory; 
using TarotNow.Application.Features.History.Queries.GetAllReadings;    

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Controller)]
[Authorize] 
public class HistoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingHistoryResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSessions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return this.UnauthorizedProblem();

        var query = new GetReadingHistoryQuery
        {
            UserId = userId,
            
            Page = page > 0 ? page : 1,

            
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

        [HttpGet("sessions/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetReadingDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionDetails([FromRoute] string id)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            return this.UnauthorizedProblem();

        var query = new GetReadingDetailQuery
        {
            UserId = userId,   
            SessionId = id     
        };

        var result = await _mediator.Send(query);

        
        if (result == null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Reading session not found",
                detail: "Reading session not found.");
        }

        return Ok(result);
    }

        [HttpGet("admin/all-sessions")]
    [Authorize(Roles = "admin")] 
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllReadingsResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)] 
    public async Task<IActionResult> GetAllSessions(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? username = null,    
        [FromQuery] string? spreadType = null,  
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)   
    {
        var query = new GetAllReadingsQuery
        {
            Page = page > 0 ? page : 1,
            PageSize = pageSize is > 0 and <= 50 ? pageSize : 10,
            Username = username,       
            SpreadType = spreadType,   
            StartDate = startDate,     
            EndDate = endDate          
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
