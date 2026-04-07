

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.InitSession;

namespace TarotNow.Api.Controllers;


[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Reading)]
[Authorize] 
public class TarotController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarotController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpPost("init")]
    public async Task<IActionResult> InitSession([FromBody] InitReadingSessionCommand command)
    {
        
        
        
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();
        command.UserId = userId; 

        var result = await _mediator.Send(command);
        return Ok(result); 
    }

        [HttpPost("reveal")]
    public async Task<IActionResult> RevealCards([FromBody] TarotNow.Application.Features.Reading.Commands.RevealSession.RevealReadingSessionCommand command)
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();
        command.UserId = userId; 

        var result = await _mediator.Send(command);
        return Ok(result); 
    }

        [HttpGet("cards-catalog")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCardsCatalog(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCardsCatalog.GetCardsCatalogQuery(),
            cancellationToken
        );
        return Ok(result);
    }

        [HttpGet("collection")]
    public async Task<IActionResult> GetCollection()
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        var result = await _mediator.Send(
            new TarotNow.Application.Features.Reading.Queries.GetCollection.GetUserCollectionQuery { UserId = userId }
        );
        return Ok(result);
    }
}
