using System.Security.Claims;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Services;

public sealed partial class AiStreamEndpointService
{
    public async Task<AiStreamExecutionPlan> PrepareStreamAsync(
        AiStreamReadEndpointRequest request,
        CancellationToken cancellationToken)
    {
        var access = await EnsureStreamingAccessAsync(request.User, cancellationToken);
        if (access.Problem is not null)
        {
            return new AiStreamExecutionPlan(null, access.Problem);
        }

        if (request.HttpRequest.Query.ContainsKey("followupQuestion"))
        {
            _logger.LogWarning(
                "Rejected legacy follow-up query transport for session {SessionId} and user {UserId}.",
                request.SessionId,
                access.UserId);
            return new AiStreamExecutionPlan(null, CreateBadRequest("Follow-up question must be sent via stream ticket."));
        }

        return TryBuildStreamRequest(request, access.UserId, out var streamRequest, out var problem)
            ? new AiStreamExecutionPlan(streamRequest, null)
            : new AiStreamExecutionPlan(null, problem);
    }

    private bool TryBuildStreamRequest(
        AiStreamReadEndpointRequest request,
        Guid userId,
        out AiStreamOrchestrationRequest streamRequest,
        out AiStreamEndpointProblem? problem)
    {
        streamRequest = default;
        problem = null;

        if (!TryResolveStreamToken(request, userId, out var resolved, out problem))
        {
            return false;
        }

        if (!ValidateFollowUpIdempotency(resolved.FollowUpQuestion, resolved.IdempotencyKey, out problem))
        {
            return false;
        }

        streamRequest = new AiStreamOrchestrationRequest(
            userId,
            request.SessionId,
            resolved.FollowUpQuestion,
            resolved.Language,
            resolved.IdempotencyKey);
        return true;
    }

    private bool TryResolveStreamToken(
        AiStreamReadEndpointRequest request,
        Guid userId,
        out StreamTokenResolution resolved,
        out AiStreamEndpointProblem? problem)
    {
        resolved = new StreamTokenResolution(
            null,
            NormalizeLanguage(request.Language),
            request.HttpRequest.GetIdempotencyKeyOrEmpty());
        problem = null;

        if (string.IsNullOrWhiteSpace(request.StreamToken))
        {
            return true;
        }

        if (!_aiStreamTicketService.TryRead(request.StreamToken, out var payload))
        {
            problem = CreateBadRequest("Invalid or expired stream token.");
            return false;
        }

        if (payload.UserId != userId || !string.Equals(payload.SessionId, request.SessionId, StringComparison.Ordinal))
        {
            problem = CreateForbidden("Stream token does not match the authenticated user or session.");
            return false;
        }

        resolved = new StreamTokenResolution(payload.FollowUpQuestion, payload.Language, payload.IdempotencyKey);
        return true;
    }

    private static bool ValidateFollowUpIdempotency(
        string? followUpQuestion,
        string? idempotencyKey,
        out AiStreamEndpointProblem? problem)
    {
        problem = null;
        if (string.IsNullOrWhiteSpace(followUpQuestion) || !string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return true;
        }

        problem = CreateBadRequest("Idempotency-Key header is required for follow-up stream.");
        return false;
    }

    private readonly record struct StreamTokenResolution(
        string? FollowUpQuestion,
        string Language,
        string? IdempotencyKey);
}
