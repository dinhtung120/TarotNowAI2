using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Contracts.Requests;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
public sealed class AdminReaderRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminReaderRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

        [HttpGet("reader-requests")]
    public async Task<IActionResult> ListReaderRequests([FromQuery] TarotNow.Application.Features.Admin.Queries.ListReaderRequests.ListReaderRequestsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

        [HttpPatch("reader-requests/process")]
    public async Task<IActionResult> ProcessReaderRequest([FromBody] ProcessReaderRequestBody body)
    {
        if (!User.TryGetUserId(out var adminId))
            return this.UnauthorizedProblem();

        var command = new TarotNow.Application.Features.Admin.Commands.ApproveReader.ApproveReaderCommand
        {
            RequestId = body.RequestId,
            Action = body.Action,
            AdminNote = body.AdminNote,
            AdminId = adminId
        };

        var result = await _mediator.Send(command);
        return result
            ? Ok(new { success = true })
            : Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Cannot process reader request",
                detail: "Không thể xử lý đơn xin Reader.");
    }
}
