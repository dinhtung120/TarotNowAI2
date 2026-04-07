namespace TarotNow.Api.Contracts.Requests;

public class CreateWithdrawalBody
{
    public long AmountDiamond { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;

    public string BankName { get; set; } = string.Empty;

    public string BankAccountName { get; set; } = string.Empty;

    public string BankAccountNumber { get; set; } = string.Empty;

    public string MfaCode { get; set; } = string.Empty;
}
