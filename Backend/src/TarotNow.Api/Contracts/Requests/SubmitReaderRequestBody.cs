namespace TarotNow.Api.Contracts.Requests;

public class SubmitReaderRequestBody
{
    public string IntroText { get; set; } = string.Empty;

    public List<string>? ProofDocuments { get; set; }
}
