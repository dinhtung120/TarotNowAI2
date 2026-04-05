/*
 * ===================================================================
 * FILE: GetModerationQueueQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Community.Queries.GetModerationQueue
 * ===================================================================
 * MỤC ĐÍCH:
 *   Lấy danh sách các báo cáo vi phạm ở trạng thái Pending dành cho Admin xemét.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Queries.GetModerationQueue;

public class GetModerationQueueQuery : IRequest<(IEnumerable<ReportDto> Items, long TotalCount)>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? StatusFilter { get; set; }
}

public class GetModerationQueueQueryHandler : IRequestHandler<GetModerationQueueQuery, (IEnumerable<ReportDto> Items, long TotalCount)>
{
    private readonly IReportRepository _reportRepo;

    public GetModerationQueueQueryHandler(IReportRepository reportRepo)
    {
        _reportRepo = reportRepo;
    }

    public async Task<(IEnumerable<ReportDto> Items, long TotalCount)> Handle(GetModerationQueueQuery request, CancellationToken cancellationToken)
    {
        // Lấy queue từ Repository chung của Report
        // NOTE: Hiện IReportRepository/GetPaginatedAsync chưa hỗ trợ lọc riêng type="post"
        // Ở phiên bản thực tế, cần thêm filter type="post" vào repository để phân loại 
        // Report Message chat và Report Post cộng đồng. 
        // Hiện tại ta re-use hàm hiện có để demo luồng.
        return await _reportRepo.GetPaginatedAsync(request.Page, request.PageSize, request.StatusFilter, cancellationToken);
    }
}
