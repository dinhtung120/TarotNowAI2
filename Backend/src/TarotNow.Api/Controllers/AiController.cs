using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Extensions;
using TarotNow.Application.Features.Reading.Commands.StreamReading;

namespace TarotNow.Api.Controllers;

[ApiController]
[Route("api/v1/sessions")]
[Authorize]
public partial class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AiController> _logger;

    public AiController(IMediator mediator, ILogger<AiController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{sessionId}/stream")]
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? followUpQuestion,
        [FromQuery] string? language,
        CancellationToken cancellationToken)
    {
        if (!User.TryGetUserId(out var userId))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var streamResult = await TryStartStreamAsync(userId, sessionId, followUpQuestion, language, cancellationToken);
        if (streamResult == null)
        {
            return;
        }

        ConfigureSseHeaders(Response);
        await StreamAndFinalizeAsync(streamResult, userId, sessionId, followUpQuestion, cancellationToken);
    }
}
