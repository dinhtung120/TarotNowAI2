namespace TarotNow.Api.Contracts.Requests;

public class AddQuestionBody
{
    public string ConversationRef { get; set; } = string.Empty;

    public long AmountDiamond { get; set; }

    public string? ProposalMessageRef { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}
