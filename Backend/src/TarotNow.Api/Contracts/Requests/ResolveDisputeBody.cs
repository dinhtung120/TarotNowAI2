namespace TarotNow.Api.Contracts.Requests;

public class ResolveDisputeBody
{
    public Guid ItemId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? AdminNote { get; set; }
}
