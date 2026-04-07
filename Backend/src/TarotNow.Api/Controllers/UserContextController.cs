using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.UserContext)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
public class UserContextController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserContextController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet("metadata")]
    [Authorize]
    public async Task<IActionResult> GetInitialMetadata()
    {
        if (!User.TryGetUserId(out var userId))
            return this.UnauthorizedProblem();

        var query = new GetInitialMetadataQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
