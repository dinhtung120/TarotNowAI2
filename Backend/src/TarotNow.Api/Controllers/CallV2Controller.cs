using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarotNow.Api.Constants;
using TarotNow.Application.Interfaces;

namespace TarotNow.Api.Controllers;

[ApiController]
[Authorize(Policy = ApiAuthorizationPolicies.AuthenticatedUser)]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Calls)]
public sealed class CallV2Controller : ControllerBase
{
    private readonly ICallV2Service _callService;

    public CallV2Controller(ICallV2Service callService)
    {
        _callService = callService;
    }

    /// <summary>
    /// Khởi tạo phiên gọi LiveKit mới cho hội thoại.
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> StartAsync([FromBody] StartCallV2Request request, CancellationToken cancellationToken)
    {
        var requesterId = GetUserIdOrThrow();
        var ticket = await _callService.StartAsync(requesterId, request.ConversationId, request.Type, cancellationToken);
        return Ok(ticket);
    }

    /// <summary>
    /// Chấp nhận cuộc gọi và nhận join token của participant hiện tại.
    /// </summary>
    [HttpPost("{callSessionId}/accept")]
    public async Task<IActionResult> AcceptAsync([FromRoute] string callSessionId, CancellationToken cancellationToken)
    {
        var requesterId = GetUserIdOrThrow();
        var ticket = await _callService.AcceptAsync(requesterId, callSessionId, cancellationToken);
        return Ok(ticket);
    }

    /// <summary>
    /// Kết thúc cuộc gọi theo cách idempotent.
    /// </summary>
    [HttpPost("{callSessionId}/end")]
    public async Task<IActionResult> EndAsync(
        [FromRoute] string callSessionId,
        [FromBody] EndCallV2Request? request,
        CancellationToken cancellationToken)
    {
        var requesterId = GetUserIdOrThrow();
        var session = await _callService.EndAsync(requesterId, callSessionId, request?.Reason ?? "normal", cancellationToken);
        return Ok(session);
    }

    /// <summary>
    /// Cấp lại join token khi client cần reconnect vào room hiện tại.
    /// </summary>
    [HttpPost("{callSessionId}/token")]
    public async Task<IActionResult> IssueTokenAsync([FromRoute] string callSessionId, CancellationToken cancellationToken)
    {
        var requesterId = GetUserIdOrThrow();
        var ticket = await _callService.IssueTokenAsync(requesterId, callSessionId, cancellationToken);
        return Ok(ticket);
    }

    private Guid GetUserIdOrThrow()
    {
        var userIdValue = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdValue, out var userId)) return userId;
        throw new UnauthorizedAccessException();
    }
}
