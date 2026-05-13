namespace TarotNow.Application.Features.Reading.Commands.CompleteAiStream;

public sealed record AiStreamSettlementDecision(bool ShouldConsumeEscrow, string Reason)
{
    public static AiStreamSettlementDecision Consume(string reason)
    {
        return new AiStreamSettlementDecision(true, reason);
    }

    public static AiStreamSettlementDecision Refund(string reason)
    {
        return new AiStreamSettlementDecision(false, reason);
    }
}
