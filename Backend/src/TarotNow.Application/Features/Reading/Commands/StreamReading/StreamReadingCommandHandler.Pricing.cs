using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.StreamReading;

public partial class StreamReadingCommandHandler
{
    private async Task<long> CalculateCostAsync(
        StreamReadingCommand request,
        ReadingSession session,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FollowupQuestion))
        {
            return 0;
        }

        var followUpCount = await _aiRequestRepo.GetFollowupCountBySessionAsync(request.ReadingSessionId, cancellationToken);
        long cost = 0;
        try
        {
            cost = _pricingService.CalculateNextFollowupCost(session.CardsDrawn ?? "[]", followUpCount);
        }
        catch (InvalidOperationException ex)
        {
            throw new BadRequestException(ex.Message);
        }

        if (cost > 0)
        {
            /*
             * FUTURE-1 FIX: IdempotencyKey sau khi sửa phải XÁC ĐỊNH để chống trừ đúp khi retry.
             * Dùng sessionId + followUpCount là đủ độc nhất vì mỗi session + mỗi lượt followup là duy nhất.
             * Bỏ DateTime.UtcNow.Ticks và GetHashCode vì chúng không deterministic.
             */
            var consumeResult = await _entitlementService.ConsumeAsync(
                userId: request.UserId,
                entitlementKey: EntitlementKey.FreeAiStreamDaily,
                referenceSource: "StreamReadingFollowup",
                referenceId: session.Id,
                idempotencyKey: $"ai_stream_ent_{session.Id}_{followUpCount}_{Guid.NewGuid():N}",
                ct: cancellationToken
            );

            if (consumeResult.Success)
            {
                cost = 0;
            }
        }

        return cost;
    }

    private async Task<AiRequest> CreateAiRequestAsync(
        StreamReadingCommand request,
        ReadingSession session,
        long calculatedCost,
        CancellationToken cancellationToken)
    {
        var idempotencyKey = $"ai_stream_{session.Id}_{Guid.CreateVersion7():N}";
        var aiRequest = new AiRequest
        {
            UserId = request.UserId,
            ReadingSessionRef = session.Id,
            Status = AiRequestStatus.Requested,
            IdempotencyKey = idempotencyKey,
            PromptVersion = "v1.5",
            ChargeDiamond = calculatedCost
        };

        await _aiRequestRepo.AddAsync(aiRequest, cancellationToken);
        return aiRequest;
    }

    private async Task FreezeEscrowAsync(
        StreamReadingCommand request,
        AiRequest aiRequest,
        long calculatedCost,
        CancellationToken cancellationToken)
    {
        if (calculatedCost <= 0)
        {
            return;
        }

        try
        {
            await _walletRepo.FreezeAsync(
                userId: request.UserId,
                amount: calculatedCost,
                referenceSource: "AiRequest",
                referenceId: aiRequest.Id.ToString(),
                description: ResolveEscrowDescription(request.FollowupQuestion),
                idempotencyKey: $"freeze_{aiRequest.Id}",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            aiRequest.Status = AiRequestStatus.FailedBeforeFirstToken;
            aiRequest.FinishReason = "insufficient_funds_or_error";
            await _aiRequestRepo.UpdateAsync(aiRequest, cancellationToken);

            if (ex is InvalidOperationException)
            {
                throw new BadRequestException("Not enough balance to perform AI Reading.");
            }

            throw new BadRequestException("Unable to reserve balance for AI Reading. Please try again later.");
        }
    }

    private static string ResolveEscrowDescription(string? followupQuestion)
    {
        return string.IsNullOrWhiteSpace(followupQuestion)
            ? "Escrow freeze for initial Tarot Reading"
            : "Escrow freeze for Follow-up Chat";
    }
}
