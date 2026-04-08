namespace TarotNow.Api.Controllers;

public sealed class StartCallV2Request
{
    public string ConversationId { get; set; } = string.Empty;

    public string Type { get; set; } = "audio";
}

public sealed class EndCallV2Request
{
    public string Reason { get; set; } = "normal";
}
