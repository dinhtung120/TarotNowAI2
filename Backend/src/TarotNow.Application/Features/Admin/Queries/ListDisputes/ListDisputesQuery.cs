using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListDisputes;

// Query phân trang danh sách dispute items.
public class ListDisputesQuery : IRequest<ListDisputesResult>
{
    // Trang hiện tại (1-based).
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;
}

// Kết quả trả về cho truy vấn danh sách dispute.
public class ListDisputesResult
{
    // Danh sách dispute item của trang hiện tại.
    public IReadOnlyList<DisputeItemDto> Items { get; set; } = Array.Empty<DisputeItemDto>();

    // Tổng số dispute item thỏa điều kiện truy vấn.
    public long TotalCount { get; set; }

    // Trang hiện tại.
    public int Page { get; set; }

    // Kích thước trang hiện tại.
    public int PageSize { get; set; }
}

// DTO hiển thị một dispute item trong trang admin.
public class DisputeItemDto
{
    // Định danh question item tranh chấp.
    public Guid Id { get; set; }

    // Định danh finance session chứa item tranh chấp.
    public Guid FinanceSessionId { get; set; }

    // Định danh người trả tiền.
    public Guid PayerId { get; set; }

    // Định danh reader nhận tiền.
    public Guid ReceiverId { get; set; }

    // Số kim cương của item tranh chấp.
    public long AmountDiamond { get; set; }

    // Trạng thái hiện tại của item.
    public string Status { get; set; } = string.Empty;

    // Thời điểm tạo item.
    public DateTime CreatedAt { get; set; }

    // Thời điểm cập nhật item gần nhất.
    public DateTime? UpdatedAt { get; set; }
}

// Handler truy vấn danh sách dispute từ finance repository.
public class ListDisputesQueryHandler : IRequestHandler<ListDisputesQuery, ListDisputesResult>
{
    private readonly IChatFinanceRepository _financeRepository;

    /// <summary>
    /// Khởi tạo handler list disputes.
    /// Luồng xử lý: nhận finance repository để truy vấn item tranh chấp theo phân trang.
    /// </summary>
    public ListDisputesQueryHandler(IChatFinanceRepository financeRepository)
    {
        _financeRepository = financeRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách dispute items.
    /// Luồng xử lý: truy vấn paginated từ repository, map sang DTO, trả thông tin phân trang.
    /// </summary>
    public async Task<ListDisputesResult> Handle(ListDisputesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _financeRepository.GetDisputedItemsPaginatedAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        return new ListDisputesResult
        {
            // Map explicit để tách schema trả về khỏi entity tài chính nội bộ.
            Items = items.Select(item => new DisputeItemDto
            {
                Id = item.Id,
                FinanceSessionId = item.FinanceSessionId,
                PayerId = item.PayerId,
                ReceiverId = item.ReceiverId,
                AmountDiamond = item.AmountDiamond,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
