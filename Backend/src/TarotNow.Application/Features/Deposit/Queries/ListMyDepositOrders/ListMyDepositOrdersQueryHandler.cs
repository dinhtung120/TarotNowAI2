using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Deposit.Queries.ListMyDepositOrders;

/// <summary>
/// Handler query phân trang lịch sử nạp của user.
/// </summary>
public sealed class ListMyDepositOrdersQueryHandler : IRequestHandler<ListMyDepositOrdersQuery, MyDepositOrderHistoryDto>
{
    private readonly IDepositOrderRepository _depositOrderRepository;

    /// <summary>
    /// Khởi tạo handler lịch sử nạp của user.
    /// </summary>
    public ListMyDepositOrdersQueryHandler(IDepositOrderRepository depositOrderRepository)
    {
        _depositOrderRepository = depositOrderRepository;
    }

    /// <summary>
    /// Xử lý query phân trang lịch sử nạp theo trạng thái.
    /// </summary>
    public async Task<MyDepositOrderHistoryDto> Handle(ListMyDepositOrdersQuery request, CancellationToken cancellationToken)
    {
        var normalizedPage = request.Page < 1 ? 1 : request.Page;
        var normalizedPageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 50);
        var normalizedStatus = string.IsNullOrWhiteSpace(request.Status)
            ? null
            : request.Status.Trim().ToLowerInvariant();

        var (orders, totalCount) = await _depositOrderRepository.GetPaginatedByUserAsync(
            request.UserId,
            normalizedPage,
            normalizedPageSize,
            normalizedStatus,
            cancellationToken);

        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)normalizedPageSize));

        return new MyDepositOrderHistoryDto
        {
            Items = orders.Select(MapItem).ToArray(),
            TotalCount = totalCount,
            Page = normalizedPage,
            PageSize = normalizedPageSize,
            TotalPages = totalPages
        };
    }

    private static MyDepositOrderHistoryItemDto MapItem(Domain.Entities.DepositOrder order)
    {
        return new MyDepositOrderHistoryItemDto
        {
            OrderId = order.Id,
            Status = order.Status,
            PackageCode = order.PackageCode,
            AmountVnd = order.AmountVnd,
            BaseDiamondAmount = order.BaseDiamondAmount,
            TotalDiamondAmount = order.DiamondAmount,
            BonusGoldAmount = order.BonusGoldAmount,
            TransactionId = order.TransactionId,
            FailureReason = order.FailureReason,
            CreatedAt = order.CreatedAt,
            ProcessedAt = order.ProcessedAt
        };
    }
}
