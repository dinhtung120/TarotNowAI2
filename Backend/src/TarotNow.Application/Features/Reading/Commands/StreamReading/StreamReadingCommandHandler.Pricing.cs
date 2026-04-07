using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
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
            
            var consumeResult = await _entitlementService.ConsumeAsync(
                new EntitlementConsumeRequest(
                    request.UserId,
                    EntitlementKey.FreeAiStreamDaily,
                    "StreamReadingFollowup",
                    session.Id,
                    $"ai_stream_ent_{request.UserId:N}_{session.Id}_{followUpCount + 1}"),
                cancellationToken);

            if (consumeResult.Success)
            {
                cost = 0;
            }
        }

        return cost;
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
