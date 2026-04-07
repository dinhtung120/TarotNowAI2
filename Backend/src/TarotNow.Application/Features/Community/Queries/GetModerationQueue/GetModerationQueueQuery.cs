

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

        
        return await _reportRepo.GetPaginatedAsync(
            page,
            pageSize,
            request.StatusFilter,
            targetType: "post",
            cancellationToken: cancellationToken);
    }
}
