namespace TarotNow.Api.Contracts.Requests;

// Payload tạo yêu cầu rút kim cương về tài khoản ngân hàng.
public class CreateWithdrawalBody
{
    // Số kim cương người dùng muốn rút.
    public long AmountDiamond { get; set; }

    // Khóa idempotency để ngăn tạo trùng yêu cầu rút khi retry.
    public string IdempotencyKey { get; set; } = string.Empty;

    // Ghi chú bổ sung từ user.
    public string? UserNote { get; set; }
}
