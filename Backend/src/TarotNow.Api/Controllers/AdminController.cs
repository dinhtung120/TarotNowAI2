using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;

namespace TarotNow.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Roles = "admin")] // Chỉ dành cho Admin
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("reconciliation/wallet")]
    public async Task<IActionResult> GetWalletMismatches()
    {
        var query = new GetLedgerMismatchQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
