using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Admin.Commands.ResolveDispute;
using TarotNow.Application.Features.Admin.Queries.ListDisputes;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.AdminDisputes)]
[Authorize(Roles = "admin")]
public sealed class AdminDisputesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDisputesController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new ListDisputesQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }

        [HttpPost("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(Guid id, [FromBody] AdminResolveDisputeBody body)
    {
        if (!User.TryGetUserId(out var adminId))
            return this.UnauthorizedProblem();

        await _mediator.Send(new ResolveDisputeCommand
        {
            ItemId = id,
            AdminId = adminId,
            Action = body.Action,
            SplitPercentToReader = body.SplitPercentToReader,
            AdminNote = body.AdminNote
        });

        return Ok(new { success = true, itemId = id, action = body.Action, splitPercentToReader = body.SplitPercentToReader });
    }
}
