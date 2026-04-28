namespace TarotNow.Api.Services;

public interface IAiStreamTicketService
{
    string Create(AiStreamTicketCreateRequest request);

    bool TryRead(string token, out AiStreamTicketPayload payload);
}

public readonly record struct AiStreamTicketCreateRequest(
    Guid UserId,
    string SessionId,
    string FollowUpQuestion,
    string Language,
    string IdempotencyKey);

public readonly record struct AiStreamTicketPayload(
    Guid UserId,
    string SessionId,
    string FollowUpQuestion,
    string Language,
    string IdempotencyKey,
    DateTimeOffset ExpiresAtUtc);
