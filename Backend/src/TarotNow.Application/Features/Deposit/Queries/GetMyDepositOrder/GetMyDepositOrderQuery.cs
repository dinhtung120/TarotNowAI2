using MediatR;

namespace TarotNow.Application.Features.Deposit.Queries.GetMyDepositOrder;

// Query lấy trạng thái một lệnh nạp của user hiện tại.
public class GetMyDepositOrderQuery : IRequest<MyDepositOrderDto>
{
    // User yêu cầu dữ liệu.
    public Guid UserId { get; set; }

    // Id đơn nạp cần truy vấn.
    public Guid OrderId { get; set; }
}

// DTO trạng thái đơn nạp của user.
public class MyDepositOrderDto
{
    // Id đơn nạp.
    public Guid OrderId { get; set; }

    // Trạng thái đơn.
    public string Status { get; set; } = string.Empty;

    // Mã gói nạp đã chọn.
    public string PackageCode { get; set; } = string.Empty;

    // Giá trị gói theo VND.
    public long AmountVnd { get; set; }

    // Diamond cơ bản theo gói.
    public long BaseDiamondAmount { get; set; }

    // Gold khuyến mãi theo campaign lúc tạo đơn.
    public long BonusGoldAmount { get; set; }

    // Tổng Diamond nhận được.
    public long TotalDiamondAmount { get; set; }

    // PayOS order code.
    public long PayOsOrderCode { get; set; }

    // Trạng thái provisioning payment link.
    public string PaymentLinkStatus { get; set; } = string.Empty;

    // Url checkout.
    public string? CheckoutUrl { get; set; }

    // Chuỗi QR code.
    public string? QrCode { get; set; }

    // Payment link id.
    public string? PaymentLinkId { get; set; }

    // Mã giao dịch từ gateway nếu đã có.
    public string? TransactionId { get; set; }

    // Lý do thất bại (nếu có).
    public string? FailureReason { get; set; }

    // Thời điểm xử lý xong đơn.
    public DateTime? ProcessedAt { get; set; }

    // Thời điểm payment link hết hạn.
    public DateTime? ExpiresAtUtc { get; set; }

    // Lý do lỗi provisioning payment link gần nhất (nếu có).
    public string? PaymentLinkFailureReason { get; set; }
}
