using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Entity lệnh nạp tiền quản lý vòng đời giao dịch PayOS và cấp ví hậu thanh toán.
public class DepositOrder
{
    // Định danh lệnh nạp.
    public Guid Id { get; private set; }

    // Người dùng sở hữu lệnh nạp.
    public Guid UserId { get; private set; }

    // Mã gói nạp người dùng đã chọn.
    public string PackageCode { get; private set; } = string.Empty;

    // Số tiền nạp theo VND.
    public long AmountVnd { get; private set; }

    // Số Diamond cơ bản theo gói.
    public long BaseDiamondAmount { get; private set; }

    // Số Gold khuyến mãi theo campaign tại thời điểm tạo order.
    public long BonusGoldAmount { get; private set; }

    // Tổng Diamond được cộng cho order.
    public long DiamondAmount { get; private set; }

    // Trạng thái xử lý lệnh nạp.
    public string Status { get; private set; } = DepositOrderStatus.Pending;

    // Khóa idempotency phía client để chống tạo lệnh trùng.
    public string ClientRequestKey { get; private set; } = string.Empty;

    // Order code dùng khi gọi PayOS.
    public long PayOsOrderCode { get; private set; }

    // Định danh payment link từ PayOS.
    public string PayOsPaymentLinkId { get; private set; } = string.Empty;

    // URL checkout PayOS.
    public string CheckoutUrl { get; private set; } = string.Empty;

    // Chuỗi dữ liệu QR từ PayOS.
    public string QrCode { get; private set; } = string.Empty;

    // Transaction/reference từ webhook PayOS.
    public string? TransactionId { get; private set; }

    // Lý do thất bại của lệnh nạp.
    public string? FailureReason { get; private set; }

    // Thời điểm payment link hết hạn.
    public DateTime? ExpiresAtUtc { get; private set; }

    // Thời điểm tạo lệnh.
    public DateTime CreatedAt { get; private set; }

    // Thời điểm cập nhật gần nhất.
    public DateTime UpdatedAt { get; private set; }

    // Thời điểm webhook chốt trạng thái thành công/thất bại.
    public DateTime? ProcessedAt { get; private set; }

    // Thời điểm đã cấp ví thành công (diamond/gold).
    public DateTime? WalletGrantedAtUtc { get; private set; }

    // Navigation tới người dùng.
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor rỗng cho ORM materialization.
    /// </summary>
    protected DepositOrder() { }

    /// <summary>
    /// Khởi tạo lệnh nạp mới với trạng thái pending.
    /// </summary>
    public DepositOrder(
        Guid userId,
        string packageCode,
        long amountVnd,
        long baseDiamondAmount,
        long bonusGoldAmount,
        string clientRequestKey,
        long payOsOrderCode,
        string payOsPaymentLinkId,
        string checkoutUrl,
        string qrCode,
        DateTime? expiresAtUtc)
    {
        ValidateMoney(amountVnd, baseDiamondAmount, bonusGoldAmount);
        ValidateRequiredString(packageCode, nameof(packageCode));
        ValidateRequiredString(clientRequestKey, nameof(clientRequestKey));
        ValidateRequiredString(payOsPaymentLinkId, nameof(payOsPaymentLinkId));
        ValidateRequiredString(checkoutUrl, nameof(checkoutUrl));
        ValidateRequiredString(qrCode, nameof(qrCode));

        Id = Guid.NewGuid();
        UserId = userId;
        PackageCode = packageCode.Trim();
        AmountVnd = amountVnd;
        BaseDiamondAmount = baseDiamondAmount;
        BonusGoldAmount = bonusGoldAmount;
        DiamondAmount = baseDiamondAmount;
        ClientRequestKey = clientRequestKey.Trim();
        PayOsOrderCode = payOsOrderCode;
        PayOsPaymentLinkId = payOsPaymentLinkId.Trim();
        CheckoutUrl = checkoutUrl.Trim();
        QrCode = qrCode.Trim();
        ExpiresAtUtc = expiresAtUtc;
        Status = DepositOrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thành công sau khi webhook PayOS hợp lệ.
    /// </summary>
    public void MarkAsSuccess(string transactionId, DateTime? processedAtUtc = null)
    {
        ValidateRequiredString(transactionId, nameof(transactionId));

        if (Status == DepositOrderStatus.Success)
        {
            EnsureTransactionConsistency(transactionId);
            return;
        }

        if (Status == DepositOrderStatus.Failed)
        {
            throw new InvalidOperationException("Cannot mark a failed order as success.");
        }

        Status = DepositOrderStatus.Success;
        TransactionId = transactionId.Trim();
        FailureReason = null;
        ProcessedAt = processedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu lệnh nạp thất bại khi webhook trả trạng thái không thành công.
    /// </summary>
    public void MarkAsFailed(string? reason, string? transactionId = null, DateTime? processedAtUtc = null)
    {
        if (Status == DepositOrderStatus.Success)
        {
            throw new InvalidOperationException("Cannot fail a successful order.");
        }

        Status = DepositOrderStatus.Failed;
        FailureReason = NormalizeOptional(reason);
        TransactionId = NormalizeOptional(transactionId);
        ProcessedAt = processedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Đánh dấu lệnh đã cấp ví để chống credit lặp.
    /// </summary>
    public void MarkWalletGranted(DateTime? grantedAtUtc = null)
    {
        if (WalletGrantedAtUtc.HasValue)
        {
            return;
        }

        WalletGrantedAtUtc = grantedAtUtc ?? DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateMoney(long amountVnd, long baseDiamondAmount, long bonusGoldAmount)
    {
        if (amountVnd <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountVnd), "AmountVnd must be greater than zero.");
        }

        if (baseDiamondAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseDiamondAmount), "BaseDiamondAmount must be greater than zero.");
        }

        if (bonusGoldAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bonusGoldAmount), "BonusGoldAmount cannot be negative.");
        }
    }

    private static void ValidateRequiredString(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} is required.", parameterName);
        }
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private void EnsureTransactionConsistency(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(TransactionId))
        {
            return;
        }

        if (!string.Equals(TransactionId, transactionId.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Processed order transaction id mismatch.");
        }
    }
}
