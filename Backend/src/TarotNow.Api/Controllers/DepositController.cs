using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Deposits)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
public partial class DepositController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DepositController> _logger;

    public DepositController(IMediator mediator, ILogger<DepositController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}
