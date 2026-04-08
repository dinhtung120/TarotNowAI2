namespace TarotNow.Api.Contracts.Requests;

// Payload tạo yêu cầu rút kim cương về tài khoản ngân hàng.
public class CreateWithdrawalBody
{
    // Số kim cương người dùng muốn rút.
    public long AmountDiamond { get; set; }

    // Khóa idempotency để ngăn tạo trùng yêu cầu rút khi retry.
    public string IdempotencyKey { get; set; } = string.Empty;

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Tên chủ tài khoản nhận tiền.
    public string BankAccountName { get; set; } = string.Empty;

    // Số tài khoản nhận tiền.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Mã MFA xác thực giao dịch rút tiền.
    public string MfaCode { get; set; } = string.Empty;
}
