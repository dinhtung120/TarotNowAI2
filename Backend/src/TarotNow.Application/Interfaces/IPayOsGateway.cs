namespace TarotNow.Application.Interfaces;

/// <summary>
/// Adapter contract gọi PayOS để tạo payment link và xác thực webhook.
/// </summary>
public interface IPayOsGateway
{
    /// <summary>
    /// Tạo payment link từ PayOS.
    /// </summary>
    Task<PayOsCreatePaymentLinkResult> CreatePaymentLinkAsync(
        PayOsCreatePaymentLinkRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Xác thực và đọc dữ liệu webhook PayOS.
    /// </summary>
    Task<PayOsVerifiedWebhookData> VerifyWebhookAsync(
        string rawPayload,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy trạng thái payment link theo order code để reconcile khi webhook bị trễ/mất.
    /// </summary>
    Task<PayOsPaymentLinkInformation> GetPaymentLinkInformationAsync(
        long orderCode,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Payload tạo payment link PayOS.
/// </summary>
public sealed class PayOsCreatePaymentLinkRequest
{
    /// <summary>
    /// Order code theo chuẩn PayOS.
    /// </summary>
    public long OrderCode { get; init; }

    /// <summary>
    /// Số tiền thanh toán theo VND.
    /// </summary>
    public long Amount { get; init; }

    /// <summary>
    /// Mô tả thanh toán.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Url callback khi người dùng hủy thanh toán.
    /// </summary>
    public string CancelUrl { get; init; } = string.Empty;

    /// <summary>
    /// Url callback khi người dùng quay lại sau thanh toán.
    /// </summary>
    public string ReturnUrl { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm hết hạn payment link theo Unix time.
    /// </summary>
    public long? ExpiredAtUnix { get; init; }

    /// <summary>
    /// Danh sách item hiển thị trên trang checkout.
    /// </summary>
    public IReadOnlyList<PayOsPaymentItem> Items { get; init; } = Array.Empty<PayOsPaymentItem>();
}

/// <summary>
/// Item hiển thị khi tạo payment link.
/// </summary>
public sealed class PayOsPaymentItem
{
    /// <summary>
    /// Tên item.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Số lượng item.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Đơn giá item.
    /// </summary>
    public long Price { get; init; }
}

/// <summary>
/// Kết quả tạo payment link từ PayOS.
/// </summary>
public sealed class PayOsCreatePaymentLinkResult
{
    /// <summary>
    /// Payment link id do PayOS cấp.
    /// </summary>
    public string PaymentLinkId { get; init; } = string.Empty;

    /// <summary>
    /// Url checkout.
    /// </summary>
    public string CheckoutUrl { get; init; } = string.Empty;

    /// <summary>
    /// Chuỗi QR thanh toán.
    /// </summary>
    public string QrCode { get; init; } = string.Empty;

    /// <summary>
    /// Trạng thái payment link ban đầu.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Thời điểm hết hạn payment link theo UTC.
    /// </summary>
    public DateTime? ExpiresAtUtc { get; init; }
}

/// <summary>
/// Dữ liệu webhook đã verify chữ ký.
/// </summary>
public sealed class PayOsVerifiedWebhookData
{
    /// <summary>
    /// Order code của payment link.
    /// </summary>
    public long OrderCode { get; init; }

    /// <summary>
    /// Số tiền thanh toán.
    /// </summary>
    public long Amount { get; init; }

    /// <summary>
    /// Cờ webhook báo thanh toán thành công.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Mã giao dịch từ ngân hàng.
    /// </summary>
    public string Reference { get; init; } = string.Empty;

    /// <summary>
    /// Payment link id gửi trong webhook.
    /// </summary>
    public string PaymentLinkId { get; init; } = string.Empty;

    /// <summary>
    /// Mã trạng thái gateway trong payload data.
    /// </summary>
    public string GatewayCode { get; init; } = string.Empty;

    /// <summary>
    /// Lý do lỗi nếu webhook thất bại.
    /// </summary>
    public string? FailureReason { get; init; }

    /// <summary>
    /// Thời điểm giao dịch trong webhook (UTC nếu parse được).
    /// </summary>
    public DateTime? TransactionAtUtc { get; init; }
}

/// <summary>
/// Snapshot trạng thái payment link lấy trực tiếp từ PayOS.
/// </summary>
public sealed class PayOsPaymentLinkInformation
{
    /// <summary>
    /// Order code của payment link.
    /// </summary>
    public long OrderCode { get; init; }

    /// <summary>
    /// Tổng số tiền của payment link (VND).
    /// </summary>
    public long Amount { get; init; }

    /// <summary>
    /// Số tiền đã thanh toán.
    /// </summary>
    public long AmountPaid { get; init; }

    /// <summary>
    /// Trạng thái payment link theo PayOS (PAID/PENDING/CANCELLED/EXPIRED...).
    /// </summary>
    public string PaymentStatus { get; init; } = string.Empty;

    /// <summary>
    /// Reference giao dịch gần nhất (nếu có).
    /// </summary>
    public string? LatestReference { get; init; }

    /// <summary>
    /// Thời điểm giao dịch gần nhất theo UTC (nếu parse được).
    /// </summary>
    public DateTime? LatestTransactionAtUtc { get; init; }

    /// <summary>
    /// Lý do hủy/thất bại theo PayOS (nếu có).
    /// </summary>
    public string? FailureReason { get; init; }
}
