namespace TarotNow.Api.Contracts.Requests;

public class ConversationSendMessageBody
{
    public string Type { get; set; } = "text";

    public string Content { get; set; } = string.Empty;

    public TarotNow.Application.Common.MediaPayloadDto? MediaPayload { get; set; }
}

public class ConversationRejectBody
{
    public string? Reason { get; set; }
}

public class ConversationCompleteRespondBody
{
    public bool Accept { get; set; }
}

public class ConversationAddMoneyRequestBody
{
    public long AmountDiamond { get; set; }

    public string? Description { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;
}

public class ConversationAddMoneyRespondBody
{
    public bool Accept { get; set; }

    public string OfferMessageId { get; set; } = string.Empty;

    public string? RejectReason { get; set; }
}

public class ConversationDisputeBody
{
    public Guid? ItemId { get; set; }

    public string Reason { get; set; } = string.Empty;
}
