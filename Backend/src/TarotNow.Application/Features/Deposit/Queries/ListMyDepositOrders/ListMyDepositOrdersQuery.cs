using MediatR;

namespace TarotNow.Application.Features.Deposit.Queries.ListMyDepositOrders;

/// <summary>
/// Query phân trang lịch sử nạp của user hiện tại.
/// </summary>
public sealed class ListMyDepositOrdersQuery : IRequest<MyDepositOrderHistoryDto>
{
    /// <summary>
    /// User cần xem lịch sử nạp.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Trang hiện tại (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Kích thước trang.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Bộ lọc trạng thái (pending/success/failed), bỏ trống để lấy tất cả.
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// Kết quả phân trang lịch sử nạp của user.
/// </summary>
public sealed class MyDepositOrderHistoryDto
{
    /// <summary>
    /// Danh sách đơn nạp theo trang hiện tại.
    /// </summary>
    public IReadOnlyCollection<MyDepositOrderHistoryItemDto> Items { get; set; } = Array.Empty<MyDepositOrderHistoryItemDto>();

    /// <summary>
    /// Tổng số đơn nạp theo bộ lọc hiện tại.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Trang hiện tại.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Kích thước trang hiện tại.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Tổng số trang.
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Một dòng dữ liệu lịch sử nạp.
/// </summary>
public sealed class MyDepositOrderHistoryItemDto
{
    /// <summary>
    /// Id đơn nạp.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Trạng thái đơn nạp.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Mã gói nạp.
    /// </summary>
    public string PackageCode { get; set; } = string.Empty;

    /// <summary>
    /// Giá trị nạp theo VND.
    /// </summary>
    public long AmountVnd { get; set; }

    /// <summary>
    /// Diamond cơ bản theo gói.
    /// </summary>
    public long BaseDiamondAmount { get; set; }

    /// <summary>
    /// Tổng Diamond nhận theo đơn.
    /// </summary>
    public long TotalDiamondAmount { get; set; }

    /// <summary>
    /// Gold khuyến mãi theo campaign.
    /// </summary>
    public long BonusGoldAmount { get; set; }

    /// <summary>
    /// Mã giao dịch ngân hàng/gateway (nếu có).
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// Lý do thất bại (nếu có).
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Thời điểm tạo đơn.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Thời điểm xử lý xong đơn.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
}
