using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler
{
    private async Task<AiRequest> CreateAiRequestAsync(
        StreamReadingCommand request,
        ReadingSession session,
        long calculatedCost,
        CancellationToken cancellationToken)
    {
        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(
            request.ReadingSessionId,
            cancellationToken);

        var aiRequest = new AiRequest
        {
            UserId = request.UserId,
            ReadingSessionRef = session.Id,
            FollowupSequence = ResolveFollowupSequence(request.FollowupQuestion, followUpCount),
            Status = AiRequestStatus.Requested,
            IdempotencyKey = $"ai_stream_{session.Id}_{Guid.CreateVersion7():N}",
            PromptVersion = "v1.5",
            ChargeDiamond = calculatedCost
        };

        await _aiRequestRepo.AddAsync(aiRequest, cancellationToken);
        return aiRequest;
    }

    private static short? ResolveFollowupSequence(string? followupQuestion, int followUpCount)
    {
        if (string.IsNullOrWhiteSpace(followupQuestion))
        {
            return null;
        }

        return (short)(followUpCount + 1);
    }
}
