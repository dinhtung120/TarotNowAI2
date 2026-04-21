
namespace TarotNow.Domain.Entities;

// Entity yêu cầu rút tiền để lưu thông tin đối soát ngân hàng và trạng thái xử lý.
public class WithdrawalRequest
{
    // Định danh yêu cầu rút.
    public Guid Id { get; set; }

    // Người dùng tạo yêu cầu.
    public Guid UserId { get; set; }

    // Tuần nghiệp vụ UTC (mốc thứ Hai) dùng cho giới hạn 1 lần/tuần.
    public DateOnly BusinessWeekStartUtc { get; set; }

    // Số Diamond quy đổi để rút.
    public long AmountDiamond { get; set; }

    // Số tiền VND gốc trước phí.
    public long AmountVnd { get; set; }

    // Phí rút tiền theo VND.
    public long FeeVnd { get; set; }

    // Số tiền thực nhận sau phí.
    public long NetAmountVnd { get; set; }

    // Tên ngân hàng nhận tiền.
    public string BankName { get; set; } = string.Empty;

    // Mã BIN ngân hàng theo chuẩn NAPAS/VietQR.
    public string BankBin { get; set; } = string.Empty;

    // Tên chủ tài khoản ngân hàng.
    public string BankAccountName { get; set; } = string.Empty;

    // Số tài khoản ngân hàng.
    public string BankAccountNumber { get; set; } = string.Empty;

    // Trạng thái xử lý yêu cầu (pending/approved/rejected/...).
    public string Status { get; set; } = TarotNow.Domain.Enums.WithdrawalRequestStatus.Pending;

    // Idempotency key của thao tác tạo request.
    public string RequestIdempotencyKey { get; set; } = string.Empty;

    // Idempotency key của thao tác process request (approve/reject).
    public string? ProcessIdempotencyKey { get; set; }

    // Ghi chú user khi tạo yêu cầu.
    public string? UserNote { get; set; }

    // Admin xử lý yêu cầu (nếu có).
    public Guid? AdminId { get; set; }

    // Ghi chú xử lý của admin.
    public string? AdminNote { get; set; }

    // Thời điểm yêu cầu được xử lý.
    public DateTime? ProcessedAt { get; set; }

    // Thời điểm tạo yêu cầu.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Thời điểm cập nhật gần nhất.
    public DateTime? UpdatedAt { get; set; }
}
