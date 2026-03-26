using TarotNow.Domain.Entities;

namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public partial class CompleteAiStreamCommandHandler
{
    private async Task ProcessCompletionAsync(
        CompleteAiStreamCommand request,
        CompletionContext context,
        CancellationToken cancellationToken)
    {
        var record = await _aiRequestRepo.GetByIdAsync(request.AiRequestId, cancellationToken);
        if (record == null)
        {
            return;
        }

        ApplyRecordFinalState(record, request);
        var wasRefunded = await ApplyWalletSettlementAsync(request, record, cancellationToken);

        await _aiRequestRepo.UpdateAsync(record, cancellationToken);
        await UpdateReadingSessionContentAsync(request, record, cancellationToken);
        await PublishReadingBillingEventAsync(request, record, wasRefunded, cancellationToken);

        context.Processed = true;
        context.RequestId = record.Id.ToString();
        context.SessionRef = record.ReadingSessionRef;
        context.TelemetryStatus = request.FinalStatus == AiStreamFinalStatuses.Completed ? "completed" : "failed";
        context.TelemetryErrorCode = context.TelemetryStatus == "failed" ? request.ErrorMessage : null;
    }

    private static void ApplyRecordFinalState(AiRequest record, CompleteAiStreamCommand request)
    {
        var now = DateTimeOffset.UtcNow;
        record.Status = request.FinalStatus;
        record.FinishReason = NormalizeFinishReason(request.ErrorMessage);
        record.UpdatedAt = now;

        if (request.FinalStatus == AiStreamFinalStatuses.Completed)
        {
            record.CompletionMarkerAt = now;
        }

        if (request.FirstTokenAt.HasValue)
        {
            record.FirstTokenAt = request.FirstTokenAt;
        }
    }

    private async Task<bool> ApplyWalletSettlementAsync(
        CompleteAiStreamCommand request,
        AiRequest record,
        CancellationToken cancellationToken)
    {
        if (record.ChargeDiamond <= 0)
        {
            return false;
        }

        switch (request.FinalStatus)
        {
            case var status when status == AiStreamFinalStatuses.Completed:
                await ConsumeEscrowAsync(request.UserId, record, "AiRequestCompletedConsume", cancellationToken);
                return false;

            case var status when status == AiStreamFinalStatuses.FailedBeforeFirstToken:
                await RefundEscrowAsync(request.UserId, record, "Auto refund for AI stream failure before first token", cancellationToken);
                return true;

            case var status when status == AiStreamFinalStatuses.FailedAfterFirstToken:
                if (request.IsClientDisconnect)
                {
                    await ConsumeEscrowAsync(request.UserId, record, "AiRequestDisconnectConsume", cancellationToken);
                    return false;
                }

                await RefundEscrowAsync(request.UserId, record, "Auto refund for AI stream failure after first token", cancellationToken);
                return true;

            default:
                return false;
        }
    }
}
