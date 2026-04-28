using System.Security.Claims;

namespace TarotNow.Api.Services;

public interface IAiStreamEndpointService
{
    Task<AiStreamExecutionPlan> PrepareStreamAsync(
        AiStreamReadEndpointRequest request,
        CancellationToken cancellationToken);

    Task<AiStreamTicketPlan> CreateStreamTicketAsync(
        AiStreamTicketEndpointRequest request,
        CancellationToken cancellationToken);
}

public sealed record AiStreamReadEndpointRequest(
    HttpRequest HttpRequest,
    ClaimsPrincipal User,
    string SessionId,
    string? StreamToken,
    string? Language);

public sealed record AiStreamTicketEndpointRequest(
    HttpRequest HttpRequest,
    ClaimsPrincipal User,
    string SessionId,
    string? FollowUpQuestion,
    string? Language);

public readonly record struct AiStreamExecutionPlan(
    AiStreamOrchestrationRequest? Request,
    AiStreamEndpointProblem? Problem);

public readonly record struct AiStreamTicketPlan(
    string? StreamToken,
    AiStreamEndpointProblem? Problem);
