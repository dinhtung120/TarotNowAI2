using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route("api/v1/escrow")]
[ApiController]
[Authorize]
public partial class EscrowController : ControllerBase
{
    private readonly IMediator _mediator;

    public EscrowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid? GetUserId()
    {
        return User.GetUserIdOrNull();
    }
}
