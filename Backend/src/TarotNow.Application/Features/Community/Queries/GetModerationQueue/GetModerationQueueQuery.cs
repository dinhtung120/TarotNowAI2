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
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);

        // Chỉ lấy report target = "post" cho moderation queue community.
        return await _reportRepo.GetPaginatedAsync(
            page,
            pageSize,
            request.StatusFilter,
            targetType: "post",
            cancellationToken: cancellationToken);
    }
}
