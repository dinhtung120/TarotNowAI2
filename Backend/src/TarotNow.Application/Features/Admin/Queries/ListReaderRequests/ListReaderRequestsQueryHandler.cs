using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListReaderRequests;

/// <summary>
/// Handler trả về danh sách reader requests phân trang.
/// Delegate đơn giản sang repository — giữ handler mỏng.
/// </summary>
public class ListReaderRequestsQueryHandler : IRequestHandler<ListReaderRequestsQuery, ListReaderRequestsResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    public ListReaderRequestsQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    public async Task<ListReaderRequestsResult> Handle(ListReaderRequestsQuery request, CancellationToken cancellationToken)
    {
        var (requests, totalCount) = await _readerRequestRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.StatusFilter,
            cancellationToken);

        return new ListReaderRequestsResult
        {
            Requests = requests,
            TotalCount = totalCount
        };
    }
}
