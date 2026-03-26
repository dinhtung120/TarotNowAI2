namespace TarotNow.Api.Contracts.Requests;

public class ProcessWithdrawalBody
{
    public Guid WithdrawalId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? AdminNote { get; set; }

    public string MfaCode { get; set; } = string.Empty;
}
