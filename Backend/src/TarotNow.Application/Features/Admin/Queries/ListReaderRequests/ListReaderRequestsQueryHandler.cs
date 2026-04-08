using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Admin.Queries.ListReaderRequests;

// Handler truy vấn danh sách đơn đăng ký reader cho admin.
public class ListReaderRequestsQueryHandler : IRequestHandler<ListReaderRequestsQuery, ListReaderRequestsResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    /// <summary>
    /// Khởi tạo handler list reader requests.
    /// Luồng xử lý: nhận reader request repository để lấy dữ liệu phân trang theo trạng thái.
    /// </summary>
    public ListReaderRequestsQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    /// <summary>
    /// Xử lý query lấy danh sách reader requests.
    /// Luồng xử lý: truy vấn paginated từ repository và đóng gói kết quả vào DTO trả về.
    /// </summary>
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
