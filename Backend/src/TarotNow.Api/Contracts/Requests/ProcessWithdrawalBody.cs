namespace TarotNow.Api.Contracts.Requests;

// Payload admin xử lý một yêu cầu rút tiền.
public class ProcessWithdrawalBody
{
    // Định danh yêu cầu rút tiền cần thao tác.
    public Guid WithdrawalId { get; set; }

    // Hành động xử lý của admin (approve/reject...).
    public string Action { get; set; } = string.Empty;

    // Ghi chú nội bộ của admin cho quyết định xử lý.
    public string? AdminNote { get; set; }

    // Khóa idempotency chống xử lý lặp.
    public string IdempotencyKey { get; set; } = string.Empty;
}
