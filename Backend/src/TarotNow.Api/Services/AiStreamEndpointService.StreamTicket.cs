using System.Security.Claims;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Services;

public sealed partial class AiStreamEndpointService
{
    public async Task<AiStreamTicketPlan> CreateStreamTicketAsync(
        AiStreamTicketEndpointRequest request,
        CancellationToken cancellationToken)
    {
        var access = await EnsureStreamingAccessAsync(request.User, cancellationToken);
        if (access.Problem is not null)
        {
            return new AiStreamTicketPlan(null, access.Problem);
        }

        return TryCreateStreamToken(request, access.UserId, out var token, out var problem)
            ? new AiStreamTicketPlan(token, null)
            : new AiStreamTicketPlan(null, problem);
    }

    private bool TryCreateStreamToken(
        AiStreamTicketEndpointRequest request,
        Guid userId,
        out string? streamToken,
        out AiStreamEndpointProblem? problem)
    {
        streamToken = null;
        problem = null;

        if (!Guid.TryParse(request.SessionId, out var parsedSessionId) || parsedSessionId == Guid.Empty)
        {
            problem = CreateBadRequest("Session id must be a valid GUID.");
            return false;
        }

        var normalizedFollowUpQuestion = request.FollowUpQuestion?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedFollowUpQuestion))
        {
            problem = CreateBadRequest("Follow-up question is required.");
            return false;
        }

        var idempotencyKey = request.HttpRequest.GetIdempotencyKeyOrEmpty();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            problem = CreateBadRequest("Idempotency-Key header is required for follow-up stream ticket.");
            return false;
        }

        streamToken = _aiStreamTicketService.Create(new AiStreamTicketCreateRequest(
            userId,
            request.SessionId,
            normalizedFollowUpQuestion,
            NormalizeLanguage(request.Language),
            idempotencyKey));
        return true;
    }
}
