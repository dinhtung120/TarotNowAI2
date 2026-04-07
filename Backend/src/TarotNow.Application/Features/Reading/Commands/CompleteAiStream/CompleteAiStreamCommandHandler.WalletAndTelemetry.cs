using TarotNow.Domain.Entities;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandler
{
    private Task ConsumeEscrowAsync(
        Guid userId,
        AiRequest record,
        string referenceSource,
        CancellationToken cancellationToken)
    {
        return _walletRepo.ConsumeAsync(
            userId: userId,
            amount: record.ChargeDiamond,
            referenceSource: referenceSource,
            referenceId: record.Id.ToString(),
            description: "Diamond consumed for completed AI Stream",
            idempotencyKey: $"consume_{record.Id}",
            cancellationToken: cancellationToken);
    }

    private Task RefundEscrowAsync(
        Guid userId,
        AiRequest record,
        string description,
        CancellationToken cancellationToken)
    {
        return _walletRepo.RefundAsync(
            userId: userId,
            amount: record.ChargeDiamond,
            referenceSource: "AiRequestAutoRefund",
            referenceId: record.Id.ToString(),
            description: description,
            idempotencyKey: $"refund_{record.Id}",
            cancellationToken: cancellationToken);
    }

    private async Task LogTelemetrySafeAsync(CompleteAiStreamCommand request, CompletionContext context)
    {
        try
        {
            await _aiProvider.LogRequestAsync(new AiProviderRequestLog
            {
                UserId = request.UserId,
                SessionId = context.SessionRef,
                RequestId = context.RequestId,
                InputTokens = 0,
                OutputTokens = request.OutputTokens,
                LatencyMs = request.LatencyMs,
                Status = context.TelemetryStatus,
                ErrorCode = context.TelemetryErrorCode
            });
        }
        catch
        {
            
        }
    }

    private Task PublishReadingBillingEventAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        bool wasRefunded,
        CancellationToken cancellationToken)
    {
        if (record.ChargeDiamond <= 0)
        {
            return Task.CompletedTask;
        }

        return _domainEventPublisher.PublishAsync(new ReadingBillingCompletedDomainEvent
        {
            UserId = request.UserId,
            AiRequestId = record.Id,
            ReadingSessionRef = record.ReadingSessionRef ?? string.Empty,
            ChargeDiamond = record.ChargeDiamond,
            FinalStatus = request.FinalStatus,
            WasRefunded = wasRefunded
        }, cancellationToken);
    }

    private static string? NormalizeFinishReason(string? finishReason)
    {
        if (string.IsNullOrWhiteSpace(finishReason))
        {
            return null;
        }

        var normalized = finishReason.Trim();
        return normalized.Length <= 50 ? normalized : normalized[..50];
    }
}
