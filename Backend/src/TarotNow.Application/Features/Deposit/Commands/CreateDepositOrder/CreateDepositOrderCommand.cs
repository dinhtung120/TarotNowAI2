using MediatR;

namespace TarotNow.Application.Features.Deposit.Commands.CreateDepositOrder;

// Command tạo đơn nạp tiền theo gói preset.
public class CreateDepositOrderCommand : IRequest<CreateDepositOrderResponse>
{
    // Định danh user tạo đơn nạp.
    public Guid UserId { get; set; }

    // Mã gói nạp mà user chọn.
    public string PackageCode { get; set; } = string.Empty;

    // Khóa idempotency phía client để chống tạo đơn trùng.
    public string IdempotencyKey { get; set; } = string.Empty;
}

// DTO phản hồi sau khi tạo đơn nạp.
public class CreateDepositOrderResponse
{
    // Định danh đơn nạp.
    public Guid OrderId { get; set; }

    // Trạng thái đơn hiện tại.
    public string Status { get; set; } = string.Empty;

    // Số tiền VND của đơn nạp.
    public long AmountVnd { get; set; }

    // Kim cương cơ bản của gói.
    public long BaseDiamondAmount { get; set; }

    // Gold khuyến mãi áp dụng.
    public long BonusGoldAmount { get; set; }

    // Tổng kim cương user nhận.
    public long TotalDiamondAmount { get; set; }

    // PayOS order code.
    public long PayOsOrderCode { get; set; }

    // Trạng thái provisioning payment link.
    public string PaymentLinkStatus { get; set; } = string.Empty;

    // Url checkout PayOS (có thể chưa sẵn sàng nếu đang provisioning).
    public string? CheckoutUrl { get; set; }

    // Chuỗi QR code thanh toán (có thể chưa sẵn sàng nếu đang provisioning).
    public string? QrCode { get; set; }

    // Payment link id từ PayOS (có thể chưa sẵn sàng nếu đang provisioning).
    public string? PaymentLinkId { get; set; }

    // Thời điểm payment link hết hạn.
    public DateTime? ExpiresAtUtc { get; set; }

    // Lý do lỗi provisioning payment link gần nhất (nếu có).
    public string? PaymentLinkFailureReason { get; set; }
}
