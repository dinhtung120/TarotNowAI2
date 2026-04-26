using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

// Entity lệnh nạp tiền quản lý vòng đời giao dịch PayOS và cấp ví hậu thanh toán.
public partial class DepositOrder
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
    public string? PayOsPaymentLinkId { get; private set; }

    // URL checkout PayOS.
    public string? CheckoutUrl { get; private set; }

    // Chuỗi dữ liệu QR từ PayOS.
    public string? QrCode { get; private set; }

    // Trạng thái provisioning payment link.
    public string PaymentLinkStatus { get; private set; } = DepositPaymentLinkStatus.Provisioning;

    // Lý do lỗi gần nhất khi provision payment link thất bại.
    public string? PaymentLinkFailureReason { get; private set; }

    // Số lần worker đã thử provision payment link.
    public int PaymentLinkAttemptCount { get; private set; }

    // Thời điểm worker thử provision gần nhất.
    public DateTime? PaymentLinkLastAttemptAtUtc { get; private set; }

    // Thời điểm payment link được provision thành công.
    public DateTime? PaymentLinkProvisionedAtUtc { get; private set; }

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
    /// Khởi tạo lệnh nạp mới.
    /// </summary>
    public DepositOrder(
        Guid userId,
        string packageCode,
        long amountVnd,
        long baseDiamondAmount,
        long bonusGoldAmount,
        string clientRequestKey,
        long payOsOrderCode,
        string? payOsPaymentLinkId,
        string? checkoutUrl,
        string? qrCode,
        DateTime? expiresAtUtc)
    {
        ValidateMoney(amountVnd, baseDiamondAmount, bonusGoldAmount);
        ValidateRequiredString(packageCode, nameof(packageCode));
        ValidateRequiredString(clientRequestKey, nameof(clientRequestKey));

        Id = Guid.NewGuid();
        UserId = userId;
        PackageCode = packageCode.Trim();
        AmountVnd = amountVnd;
        BaseDiamondAmount = baseDiamondAmount;
        BonusGoldAmount = bonusGoldAmount;
        DiamondAmount = baseDiamondAmount;
        ClientRequestKey = clientRequestKey.Trim();
        PayOsOrderCode = payOsOrderCode;
        PayOsPaymentLinkId = NormalizeOptional(payOsPaymentLinkId);
        CheckoutUrl = NormalizeOptional(checkoutUrl);
        QrCode = NormalizeOptional(qrCode);
        ExpiresAtUtc = expiresAtUtc;
        Status = DepositOrderStatus.Pending;
        PaymentLinkStatus = HasPaymentLinkDetails()
            ? DepositPaymentLinkStatus.Ready
            : DepositPaymentLinkStatus.Provisioning;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;

        if (PaymentLinkStatus == DepositPaymentLinkStatus.Ready)
        {
            PaymentLinkAttemptCount = 1;
            PaymentLinkLastAttemptAtUtc = CreatedAt;
            PaymentLinkProvisionedAtUtc = CreatedAt;
        }
    }

}
