using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.FeatureManagement;
using TarotNow.Api.Extensions;
using TarotNow.Api.Services;

namespace TarotNow.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route(ApiRoutes.Sessions)]
[Authorize]
// API streaming AI cho reading session.
// Luồng chính: kiểm tra feature flag + quyền truy cập, rồi ủy quyền toàn bộ orchestration cho service chuyên trách.
public sealed class AiController : ControllerBase
{
    private readonly IFeatureManagerSnapshot _featureManager;
    private readonly IAiStreamSseOrchestrator _aiStreamSseOrchestrator;

    /// <summary>
    /// Khởi tạo controller streaming AI.
    /// </summary>
    public AiController(
        IFeatureManagerSnapshot featureManager,
        IAiStreamSseOrchestrator aiStreamSseOrchestrator)
    {
        _featureManager = featureManager;
        _aiStreamSseOrchestrator = aiStreamSseOrchestrator;
    }

    /// <summary>
    /// Stream kết quả đọc bài theo chuẩn SSE.
    /// Luồng xử lý: gate feature flag + auth + idempotency guard, rồi dispatch cho orchestrator.
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    [EnableRateLimiting("chat-standard")]
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? followUpQuestion,
        [FromQuery] string? language,
        CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AiStreamingEnabled))
        {
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Title = "Service Unavailable",
                Detail = "AI streaming is temporarily disabled by feature flag.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4"
            }, cancellationToken);
            return;
        }

        if (!User.TryGetUserId(out var userId))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Authentication is required or token is invalid.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1"
            }, cancellationToken);
            return;
        }

        var idempotencyKey = Request.GetIdempotencyKeyOrEmpty();
        if (string.IsNullOrWhiteSpace(followUpQuestion) == false && string.IsNullOrWhiteSpace(idempotencyKey))
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "Idempotency-Key header is required for follow-up stream.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            }, cancellationToken);
            return;
        }

        await _aiStreamSseOrchestrator.ExecuteAsync(
            HttpContext,
            new AiStreamOrchestrationRequest(
                userId,
                sessionId,
                followUpQuestion,
                language,
                idempotencyKey),
            cancellationToken);
    }
}
