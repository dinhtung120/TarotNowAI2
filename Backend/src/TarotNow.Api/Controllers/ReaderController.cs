using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Constants;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Interfaces;

namespace TarotNow.Api.Controllers;

[Route(ApiRoutes.Reader)]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[EnableRateLimiting("auth-session")]
public partial class ReaderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserPresenceTracker _presenceTracker;

    public ReaderController(IMediator mediator, IUserPresenceTracker presenceTracker)
    {
        _mediator = mediator;
        _presenceTracker = presenceTracker;
    }

    private void ApplyPresenceStatus(ReaderProfileDto profile)
    {
        if (_presenceTracker.IsOnline(profile.UserId) == false)
        {
            return;
        }

        if (string.Equals(
                ApiReaderStatusConstants.NormalizeOrDefault(profile.Status),
                ApiReaderStatusConstants.Offline,
                StringComparison.OrdinalIgnoreCase))
        {
            profile.Status = ApiReaderStatusConstants.Online;
        }
    }
}
