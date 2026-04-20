namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event yêu cầu tạo đơn nạp tiền và payment link PayOS.
/// </summary>
public sealed class DepositOrderCreateRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// User khởi tạo đơn nạp.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Mã gói nạp được chọn.
    /// </summary>
    public string PackageCode { get; init; } = string.Empty;

    /// <summary>
    /// Khóa idempotency từ client.
    /// </summary>
    public string IdempotencyKey { get; init; } = string.Empty;

    /// <summary>
    /// Id đơn nạp đã tạo.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Trạng thái đơn nạp.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Giá trị gói theo VND.
    /// </summary>
    public long AmountVnd { get; set; }

    /// <summary>
    /// Diamond cơ bản theo gói.
    /// </summary>
    public long BaseDiamondAmount { get; set; }

    /// <summary>
    /// Gold khuyến mãi tại thời điểm tạo đơn.
    /// </summary>
    public long BonusGoldAmount { get; set; }

    /// <summary>
    /// Tổng Diamond nhận được.
    /// </summary>
    public long TotalDiamondAmount { get; set; }

    /// <summary>
    /// PayOS order code.
    /// </summary>
    public long PayOsOrderCode { get; set; }

    /// <summary>
    /// URL checkout PayOS.
    /// </summary>
    public string CheckoutUrl { get; set; } = string.Empty;

    /// <summary>
    /// QR code từ PayOS.
    /// </summary>
    public string QrCode { get; set; } = string.Empty;

    /// <summary>
    /// Payment link id từ PayOS.
    /// </summary>
    public string PaymentLinkId { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm payment link hết hạn.
    /// </summary>
    public DateTime? ExpiresAtUtc { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
