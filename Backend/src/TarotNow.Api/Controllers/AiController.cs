using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? followUpQuestion,
        [FromQuery] string? language,
        CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AiStreamingEnabled))
        {
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await Response.WriteAsync("data: AI streaming is temporarily disabled by feature flag.\n\n", cancellationToken);
            return;
        }

        if (!User.TryGetUserId(out var userId))
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var idempotencyKey = Request.GetIdempotencyKeyOrEmpty();
        if (string.IsNullOrWhiteSpace(followUpQuestion) == false && string.IsNullOrWhiteSpace(idempotencyKey))
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("data: Idempotency-Key header is required for follow-up stream.\n\n", cancellationToken);
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
