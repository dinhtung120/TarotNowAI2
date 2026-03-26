using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Admin)]
[Authorize(Roles = "admin")]
public sealed class AdminReconciliationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminReconciliationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns detected wallet ledger mismatches for reconciliation.
    /// </summary>
    [HttpGet("reconciliation/wallet")]
    public async Task<IActionResult> GetWalletMismatches()
    {
        var result = await _mediator.Send(new GetLedgerMismatchQuery());
        return Ok(result);
    }
}
