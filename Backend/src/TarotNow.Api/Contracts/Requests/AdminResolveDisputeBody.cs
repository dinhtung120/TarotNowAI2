namespace TarotNow.Api.Contracts.Requests;

public class AdminResolveDisputeBody
{
    public string Action { get; set; } = string.Empty;

    public int? SplitPercentToReader { get; set; }

    public string? AdminNote { get; set; }
}
