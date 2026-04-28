using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Logging;
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
    private readonly IAiStreamTicketService _aiStreamTicketService;
    private readonly ILogger<AiController> _logger;

    /// <summary>
    /// Khởi tạo controller streaming AI.
    /// </summary>
    public AiController(
        IFeatureManagerSnapshot featureManager,
        IAiStreamSseOrchestrator aiStreamSseOrchestrator,
        IAiStreamTicketService aiStreamTicketService,
        ILogger<AiController> logger)
    {
        _featureManager = featureManager;
        _aiStreamSseOrchestrator = aiStreamSseOrchestrator;
        _aiStreamTicketService = aiStreamTicketService;
        _logger = logger;
    }

    public sealed record CreateAiStreamTicketBody(string? FollowUpQuestion, string? Language);

    /// <summary>
    /// Stream kết quả đọc bài theo chuẩn SSE.
    /// Luồng xử lý: gate feature flag + auth + idempotency guard, rồi dispatch cho orchestrator.
    /// </summary>
    [HttpGet("{sessionId}/stream")]
    [EnableRateLimiting("chat-standard")]
    public async Task StreamReading(
        string sessionId,
        [FromQuery] string? streamToken,
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

        if (Request.Query.ContainsKey("followupQuestion"))
        {
            _logger.LogWarning(
                "Rejected legacy follow-up query transport for session {SessionId} and user {UserId}.",
                sessionId,
                userId);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "Follow-up question must be sent via stream ticket.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            }, cancellationToken);
            return;
        }

        string? resolvedFollowUpQuestion = null;
        var resolvedLanguage = language;
        var idempotencyKey = Request.GetIdempotencyKeyOrEmpty();

        if (string.IsNullOrWhiteSpace(streamToken) == false)
        {
            if (!_aiStreamTicketService.TryRead(streamToken, out var ticketPayload))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = "Invalid or expired stream token.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
                }, cancellationToken);
                return;
            }

            if (ticketPayload.UserId != userId || string.Equals(ticketPayload.SessionId, sessionId, StringComparison.Ordinal) == false)
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                await Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden",
                    Detail = "Stream token does not match the authenticated user or session.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3"
                }, cancellationToken);
                return;
            }

            resolvedFollowUpQuestion = ticketPayload.FollowUpQuestion;
            resolvedLanguage = ticketPayload.Language;
            idempotencyKey = ticketPayload.IdempotencyKey;
        }

        if (string.IsNullOrWhiteSpace(resolvedFollowUpQuestion) == false && string.IsNullOrWhiteSpace(idempotencyKey))
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
                resolvedFollowUpQuestion,
                resolvedLanguage,
                idempotencyKey),
            cancellationToken);
    }

    [HttpPost("{sessionId}/stream-ticket")]
    [EnableRateLimiting("chat-standard")]
    public async Task<IActionResult> CreateStreamTicket(
        string sessionId,
        [FromBody] CreateAiStreamTicketBody body,
        CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AiStreamingEnabled))
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Title = "Service Unavailable",
                Detail = "AI streaming is temporarily disabled by feature flag.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4"
            });
        }

        if (!User.TryGetUserId(out var userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = "Authentication is required or token is invalid.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1"
            });
        }

        if (Guid.TryParse(sessionId, out var parsedSessionId) == false || parsedSessionId == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "Session id must be a valid GUID.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            });
        }

        var followUpQuestion = body.FollowUpQuestion?.Trim();
        if (string.IsNullOrWhiteSpace(followUpQuestion))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "Follow-up question is required.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            });
        }

        var idempotencyKey = Request.GetIdempotencyKeyOrEmpty();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = "Idempotency-Key header is required for follow-up stream ticket.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            });
        }

        var streamToken = _aiStreamTicketService.Create(new AiStreamTicketCreateRequest(
            userId,
            sessionId,
            followUpQuestion,
            string.IsNullOrWhiteSpace(body.Language) ? "en" : body.Language.Trim(),
            idempotencyKey));

        await Task.CompletedTask;
        return Ok(new { streamToken });
    }
}
