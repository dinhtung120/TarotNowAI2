

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Chat.Commands.CreateReport;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Reports)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize] 
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReportBody body)
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        var command = new CreateReportCommand
        {
            ReporterId = userId,                   
            TargetType = body.TargetType,           
            TargetId = body.TargetId,               
            ConversationRef = body.ConversationRef, 
            Reason = body.Reason                    
        };

        
        var result = await _mediator.Send(command);
        return Ok(new { success = true, reportId = result.Id });
    }
}
