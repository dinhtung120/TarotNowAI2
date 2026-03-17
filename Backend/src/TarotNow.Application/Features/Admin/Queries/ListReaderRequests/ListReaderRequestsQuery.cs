using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Admin.Queries.ListReaderRequests;

/// <summary>
/// Query phân trang danh sách đơn xin Reader — dùng cho Admin approval queue.
/// </summary>
public class ListReaderRequestsQuery : IRequest<ListReaderRequestsResult>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? StatusFilter { get; set; }
}

/// <summary>Kết quả phân trang.</summary>
public class ListReaderRequestsResult
{
    public IEnumerable<ReaderRequestDto> Requests { get; set; } = Enumerable.Empty<ReaderRequestDto>();
    public long TotalCount { get; set; }
}
