namespace TarotNow.Api.Services;

/// <summary>
/// Điều phối toàn bộ vòng đời SSE stream cho AI reading.
/// </summary>
public interface IAiStreamSseOrchestrator
{
    /// <summary>
    /// Thực thi khởi tạo, stream chunk và finalize trạng thái AI request.
    /// </summary>
    Task ExecuteAsync(
        HttpContext httpContext,
        AiStreamOrchestrationRequest request,
        CancellationToken cancellationToken);
}

/// <summary>
/// Request input cho orchestrator SSE.
/// </summary>
public readonly record struct AiStreamOrchestrationRequest(
    Guid UserId,
    string SessionId,
    string? FollowUpQuestion,
    string? Language,
    string? IdempotencyKey);
