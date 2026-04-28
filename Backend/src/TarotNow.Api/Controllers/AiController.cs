using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TarotNow.Api.Services;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Sessions)]
[Authorize]
// API streaming AI cho reading session.
// Luồng chính: controller chỉ gate HTTP contract và ủy quyền orchestration cho endpoint service + SSE orchestrator.
public sealed class AiController : ControllerBase
{
    private readonly IAiStreamEndpointService _aiStreamEndpointService;
    private readonly IAiStreamSseOrchestrator _aiStreamSseOrchestrator;

    /// <summary>
    /// Khởi tạo controller streaming AI.
    /// </summary>
    public AiController(
        IAiStreamEndpointService aiStreamEndpointService,
        IAiStreamSseOrchestrator aiStreamSseOrchestrator)
    {
        _aiStreamEndpointService = aiStreamEndpointService;
        _aiStreamSseOrchestrator = aiStreamSseOrchestrator;
    }

    public sealed record CreateAiStreamTicketBody(string? FollowUpQuestion, string? Language);

    /// <summary>
    /// Stream kết quả đọc bài theo chuẩn SSE.
    /// Luồng xử lý: resolve request plan ở API layer, rồi dispatch sang SSE orchestrator khi request hợp lệ.
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    [EnableRateLimiting("chat-standard")]
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? streamToken,
        [FromQuery] string? language,
        CancellationToken cancellationToken)
    {
        var plan = await _aiStreamEndpointService.PrepareStreamAsync(
            new AiStreamReadEndpointRequest(Request, User, sessionId, streamToken, language),
            cancellationToken);

        if (plan.Problem is not null)
        {
            await plan.Problem.WriteAsync(Response, cancellationToken);
            return;
        }

        await _aiStreamSseOrchestrator.ExecuteAsync(HttpContext, plan.Request!.Value, cancellationToken);
    }

    /// <summary>
    /// Tạo stream token cho follow-up AI stream.
    /// Luồng xử lý: xác thực request ticket và trả token opaque để client mở SSE mà không đưa prompt lên URL.
    /// </summary>
    [HttpPost("{sessionId}/stream-ticket")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> CreateStreamTicket(
        string sessionId,
        [FromBody] CreateAiStreamTicketBody body,
        CancellationToken cancellationToken)
    {
        var plan = await _aiStreamEndpointService.CreateStreamTicketAsync(
            new AiStreamTicketEndpointRequest(Request, User, sessionId, body.FollowUpQuestion, body.Language),
            cancellationToken);

        return plan.Problem is not null
            ? plan.Problem.ToActionResult()
            : Ok(new { streamToken = plan.StreamToken });
    }
}
