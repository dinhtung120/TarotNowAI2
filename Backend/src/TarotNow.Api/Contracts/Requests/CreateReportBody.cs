namespace TarotNow.Api.Contracts.Requests;

public class CreateReportBody
{
    public string TargetType { get; set; } = string.Empty;

    public string TargetId { get; set; } = string.Empty;

    public string? ConversationRef { get; set; }

    public string Reason { get; set; } = string.Empty;
}
