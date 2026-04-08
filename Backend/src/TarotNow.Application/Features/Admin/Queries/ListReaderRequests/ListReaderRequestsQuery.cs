using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Admin.Queries.ListReaderRequests;

// Query phân trang danh sách đơn đăng ký reader.
public class ListReaderRequestsQuery : IRequest<ListReaderRequestsResult>
{
    // Trang hiện tại (1-based).
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;

    // Bộ lọc trạng thái đơn reader (tùy chọn).
    public string? StatusFilter { get; set; }
}

// Kết quả trả về cho query danh sách đơn reader.
public class ListReaderRequestsResult
{
    // Danh sách đơn reader theo trang hiện tại.
    public IEnumerable<ReaderRequestDto> Requests { get; set; } = Enumerable.Empty<ReaderRequestDto>();

    // Tổng số đơn thỏa điều kiện lọc.
    public long TotalCount { get; set; }
}
