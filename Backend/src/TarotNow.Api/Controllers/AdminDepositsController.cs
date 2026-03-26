using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
public sealed class AdminDepositsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminDepositsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns paginated deposit requests for admin backoffice.
    /// </summary>
    [HttpGet("deposits")]
    public async Task<IActionResult> ListDeposits([FromQuery] TarotNow.Application.Features.Admin.Queries.ListDeposits.ListDepositsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Processes a deposit order by approving or rejecting the request.
    /// </summary>
    [HttpPatch("deposits/process")]
    public async Task<IActionResult> ProcessDeposit([FromBody] TarotNow.Application.Features.Admin.Commands.ProcessDeposit.ProcessDepositCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { success = true }) : BadRequest(new { msg = "Không thể xử lý đơn nạp tiền này." });
    }
}
