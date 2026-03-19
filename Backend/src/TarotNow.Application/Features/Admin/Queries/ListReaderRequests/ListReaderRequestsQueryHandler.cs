/*
 * ===================================================================
 * FILE: ListReaderRequestsQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Admin.Queries.ListReaderRequests
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý query lấy danh sách đơn xin reader.
 *   THIN HANDLER (handler "mỏng"): chỉ delegate sang repository.
 *
 * THIN HANDLER LÀ GÌ?
 *   Handler không chứa logic phức tạp, chỉ:
 *   1. Gọi repository lấy dữ liệu
 *   2. Đóng gói kết quả vào response DTO
 *   
 *   Vì query ĐỌC dữ liệu (không thay đổi) → logic đơn giản.
 *   Khác với command handler (phải validate, check rules, save).
 * ===================================================================
 */

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
        /*
         * GetPaginatedAsync: trả về tuple (requests, totalCount).
         * Repository (MongoDB) thực hiện:
         *   1. Filter theo status (nếu có)
         *   2. Sort theo CreatedAt descending (mới nhất trước)
         *   3. Skip + Limit cho phân trang
         *   4. Count total documents cho pagination UI
         *
         * Tuple deconstruction: var (a, b) = ... tách thành 2 biến.
         */
        var (requests, totalCount) = await _readerRequestRepository.GetPaginatedAsync(
            request.Page,
            request.PageSize,
            request.StatusFilter,
            cancellationToken);

        // Đóng gói vào response DTO và trả về
        return new ListReaderRequestsResult
        {
            Requests = requests,
            TotalCount = totalCount
        };
    }
}
