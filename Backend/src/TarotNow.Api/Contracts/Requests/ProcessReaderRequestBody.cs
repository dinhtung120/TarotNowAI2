namespace TarotNow.Api.Contracts.Requests;

public class ProcessReaderRequestBody
{
    public string RequestId { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string? AdminNote { get; set; }
}
