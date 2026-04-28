using System.Text.Json;
using TarotNow.Infrastructure.Security;

namespace TarotNow.Api.Services;

public sealed class AiStreamTicketService : IAiStreamTicketService
{
    private static readonly TimeSpan TicketLifetime = TimeSpan.FromMinutes(2);
    private readonly ISensitiveDataProtector _sensitiveDataProtector;

    public AiStreamTicketService(ISensitiveDataProtector sensitiveDataProtector)
    {
        _sensitiveDataProtector = sensitiveDataProtector;
    }

    public string Create(AiStreamTicketCreateRequest request)
    {
        var payload = new AiStreamTicketPayload(
            request.UserId,
            request.SessionId,
            request.FollowUpQuestion,
            request.Language,
            request.IdempotencyKey,
            DateTimeOffset.UtcNow.Add(TicketLifetime));

        var serialized = JsonSerializer.Serialize(payload);
        return _sensitiveDataProtector.Protect(serialized);
    }

    public bool TryRead(string token, out AiStreamTicketPayload payload)
    {
        payload = default;

        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            var serialized = _sensitiveDataProtector.Unprotect(token);
            if (string.IsNullOrWhiteSpace(serialized))
            {
                return false;
            }

            var parsed = JsonSerializer.Deserialize<AiStreamTicketPayload>(serialized);
            if (parsed.UserId == Guid.Empty
                || string.IsNullOrWhiteSpace(parsed.SessionId)
                || string.IsNullOrWhiteSpace(parsed.FollowUpQuestion)
                || string.IsNullOrWhiteSpace(parsed.Language)
                || string.IsNullOrWhiteSpace(parsed.IdempotencyKey)
                || parsed.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                return false;
            }

            payload = parsed;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
