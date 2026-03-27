using System.Collections.Generic;

namespace TarotNow.Application.Common;

public class ConversationActionResult
{
    public bool Success { get; set; } = true;

    public string Status { get; set; } = string.Empty;

    public string? Reason { get; set; }

    public Dictionary<string, object>? Metadata { get; set; }
}

public class ConversationCompleteRespondResult : ConversationActionResult
{
    public bool Accepted { get; set; }
}

public class ConversationAddMoneyRequestResult
{
    public bool Success { get; set; } = true;

    public string MessageId { get; set; } = string.Empty;
}

public class ConversationAddMoneyRespondResult
{
    public bool Success { get; set; } = true;

    public bool Accepted { get; set; }

    public Guid? ItemId { get; set; }

    public string MessageId { get; set; } = string.Empty;
}
