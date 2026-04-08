

using System;

namespace TarotNow.Domain.Entities;

// Entity lệnh nạp tiền để quản lý vòng đời giao dịch từ lúc tạo đến khi chốt kết quả.
public class DepositOrder
{
    // Định danh lệnh nạp.
    public Guid Id { get; private set; }

    // Người dùng sở hữu lệnh nạp.
    public Guid UserId { get; private set; }

    // Số tiền nạp theo VND.
    public long AmountVnd { get; private set; }

    // Số Diamond quy đổi từ lệnh nạp.
    public long DiamondAmount { get; private set; }

    // Trạng thái xử lý lệnh nạp.
    public string Status { get; private set; } = string.Empty;

    // Mã giao dịch từ cổng thanh toán hoặc token client.
    public string? TransactionId { get; private set; }

    // Snapshot tỷ giá phục vụ đối soát tại thời điểm xử lý.
    public string? FxSnapshot { get; private set; }

    // Thời điểm tạo lệnh nạp.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm lệnh được xử lý thành công/thất bại.
    public DateTime? ProcessedAt { get; private set; }

    // Navigation tới người dùng.
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// Luồng xử lý: để EF khởi tạo entity từ dữ liệu lưu trữ.
    /// </summary>
    protected DepositOrder() { }

    /// <summary>
    /// Khởi tạo lệnh nạp mới với trạng thái Pending trước khi gửi sang cổng thanh toán.
    /// Luồng xử lý: sinh id, gán dữ liệu đầu vào, đặt trạng thái ban đầu và mốc thời gian tạo.
    /// </summary>
    public DepositOrder(Guid userId, long amountVnd, long diamondAmount)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AmountVnd = amountVnd;
        DiamondAmount = diamondAmount;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thành công sau khi nhận xác nhận hợp lệ từ cổng thanh toán.
    /// Luồng xử lý: chặn trạng thái Success lặp, cập nhật status/transaction và mốc processed.
    /// </summary>
    public void MarkAsSuccess(string transactionId, string? fxSnapshot = null)
    {
        if (Status == "Success")
        {
            // Edge case: callback lặp cho giao dịch đã success, chặn để bảo vệ tính idempotent của domain.
            throw new InvalidOperationException("This order is already marked as success.");
        }

        Status = "Success";
        TransactionId = transactionId;
        FxSnapshot = fxSnapshot;
        ProcessedAt = DateTime.UtcNow;
        // Chốt trạng thái thành công và khóa thông tin đối soát tại thời điểm xử lý.
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thất bại khi cổng thanh toán trả kết quả không thành công.
    /// Luồng xử lý: chặn hạ trạng thái từ Success, sau đó ghi trạng thái Failed và transaction liên quan.
    /// </summary>
    public void MarkAsFailed(string transactionId)
    {
        if (Status == "Success")
        {
            // Business rule: giao dịch đã success không được phép chuyển ngược về failed.
            throw new InvalidOperationException("Cannot fail a successful order.");
        }

        Status = "Failed";
        TransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
        // Cập nhật trạng thái failed để job đối soát và UI đọc đúng kết quả cuối cùng.
    }

    /// <summary>
    /// Gán transaction token phía client cho lệnh pending để liên kết trước khi callback về.
    /// Luồng xử lý: validate token đầu vào, kiểm tra trạng thái pending/chưa có token, rồi lưu token đã chuẩn hóa.
    /// </summary>
    public void SetClientTransactionToken(string transactionToken)
    {
        if (string.IsNullOrWhiteSpace(transactionToken))
        {
            // Edge case: token rỗng gây mất khả năng đối soát callback nên chặn ngay ở domain.
            throw new ArgumentException("Transaction token is required.", nameof(transactionToken));
        }

        if (Status != "Pending")
        {
            // Business rule: chỉ cho phép gán token trong giai đoạn pending để tránh ghi đè hậu xử lý.
            throw new InvalidOperationException("Client transaction token can only be set on pending order.");
        }

        if (!string.IsNullOrWhiteSpace(TransactionId))
        {
            // Chặn gán lại token để giữ idempotency khi client gửi request lặp.
            throw new InvalidOperationException("Transaction token is already set.");
        }

        TransactionId = transactionToken.Trim();
        // Chuẩn hóa token bằng Trim để thống nhất giá trị lưu trữ và so khớp callback.
    }
}
