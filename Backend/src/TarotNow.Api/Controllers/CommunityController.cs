using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Community)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Authorize]
public partial class CommunityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private Guid GetRequiredUserId()
    {
        return User.GetUserIdOrNull() ?? throw new UnauthorizedAccessException();
    }
}
