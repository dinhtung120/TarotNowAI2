namespace TarotNow.Api.Contracts.Requests;

public class OpenDisputeBody
{
    public Guid ItemId { get; set; }

    public string Reason { get; set; } = string.Empty;
}
