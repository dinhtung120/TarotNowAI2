namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public sealed class AiStreamSettlementPolicy
{
    private const int MinimumBillableOutputTokens = 40;
    private const int MinimumBillableChunks = 3;

    public AiStreamSettlementDecision Decide(CompleteAiStreamCommand command)
    {
        if (command.FinalStatus == AiStreamFinalStatuses.Completed)
        {
            return AiStreamSettlementDecision.Consume("completed");
        }

        if (command.FinalStatus == AiStreamFinalStatuses.FailedBeforeFirstToken)
        {
            return AiStreamSettlementDecision.Refund("failed_before_first_token");
        }

        if (command.FinalStatus == AiStreamFinalStatuses.FailedAfterFirstToken)
        {
            if (!command.IsClientDisconnect)
            {
                return AiStreamSettlementDecision.Refund("provider_failed_after_first_token");
            }

            var meetsThreshold = command.OutputTokens >= MinimumBillableOutputTokens
                || command.EmittedChunkCount >= MinimumBillableChunks;

            return meetsThreshold
                ? AiStreamSettlementDecision.Consume("client_disconnect_after_billable_threshold")
                : AiStreamSettlementDecision.Refund("client_disconnect_below_billable_threshold");
        }

        return AiStreamSettlementDecision.Refund("unsupported_final_status");
    }
}
