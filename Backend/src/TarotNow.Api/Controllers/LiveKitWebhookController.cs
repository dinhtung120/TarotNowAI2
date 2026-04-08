using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[ApiController]
[AllowAnonymous]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.LiveKitWebhook)]
public sealed class LiveKitWebhookController : ControllerBase
{
    private readonly ICallV2Service _callService;

    public LiveKitWebhookController(ICallV2Service callService)
    {
        _callService = callService;
    }

    /// <summary>
    /// Nhận webhook từ LiveKit để đồng bộ trạng thái room/participant.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ReceiveAsync(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync(cancellationToken);
        var authorization = Request.Headers.Authorization.ToString();

        await _callService.HandleWebhookAsync(authorization, payload, cancellationToken);
        return Ok(new { accepted = true });
    }
}
